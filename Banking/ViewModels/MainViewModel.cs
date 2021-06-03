using Banking.Models;
using Banking.Views;

using CHi.Extensions;
using CHi.Log;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Banking.ViewModels
{
	public partial class MainViewModel : INotifyPropertyChanged
	{
		public MainWindow View;
		private bool hasMissedTallies;
		private int missedTalliesCount;
		public Dictionary<string, string> AccountNames =
			new Dictionary<string, string>();

#if DEBUG
		public readonly string BALANCE = "%OneDrive%\\Tmp\\Banking\\Balance.json".TranslatePath();
#else
		public readonly string BALANCE = "%OneDrive%\\Data\\Banking\\Balance.json".TranslatePath();
#endif

		#region [ Properties ] 

		public OptionViewModel Options { get; set; }
		public ObservableCollection<Bank> Accounts { get; set; } = new ObservableCollection<Bank>();
		public ObservableCollection<Import> Imports { get; set; } = new ObservableCollection<Import>();
		public List<string> Origins { get; set; }
		public bool ToSaveBalances { get; set; } = false;
		public ObservableCollection<Balance> Balances { get; set; } =
			new ObservableCollection<Balance>();
		public ObservableCollection<CurrentBalance> CurrentBalances { get; set; } =
			new ObservableCollection<CurrentBalance>();


		public int AccountCount
		{
			get => Accounts.Count;
		}

		public DateTime? AccountMaxDate
		{
			get
			{
				if (AccountCount == 0)
				{
					return null;
				}
				return Accounts.Max(x => x.Date);
			}
		}

		public int ImportCount
		{
			get => Imports.Count;
		}

		public DateTime? ImportMaxDate
		{
			get
			{
				if (ImportCount == 0)
				{
					return null;
				}
				return Imports.Max(x => x.Date);
			}
		}

		public DateTime? ImportMinDate
		{
			get
			{
				if (ImportCount == 0)
				{
					return null;
				}
				return Imports.Min(x => x.Date);
			}
		}

		public DateTime? ImportDate
		{
			get
			{
				if (ImportCount == 0)
				{
					return null;
				}
				return Imports.Max(x => x.ImportDate);
			}
		}

		public int MissedTalliesCount
		{
			get => missedTalliesCount;
			set
			{
				if (missedTalliesCount != value)
				{
					missedTalliesCount = value;
					NotifyPropertyChanged();
				}
			}
		}

		public bool HasMissedTallies
		{
			get => hasMissedTallies;
			set
			{
				if (hasMissedTallies != value)
				{
					hasMissedTallies = value;
					NotifyPropertyChanged();
				}
			}
		}

		#endregion

		#region INotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(string propertyName = "") =>
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		#endregion

		public MainViewModel(MainWindow mainWindow)
		{
			View = mainWindow;

			Options = new OptionViewModel();
			SetDbConnection();

			if (Options.DbName != "_")
			{
				GetSummaries();
			}

			ImportFileFilters.Add("ABN", "ABN import file (*.tab)|*.tab");
			ImportFileFilters.Add("ING", "ING import file (*.csv)|*.csv");

			string jsonPath = "%OneDrive%\\Data\\Banking\\AccountNames.json".TranslatePath();
			if (File.Exists(jsonPath))
			{
				using StreamReader stream = File.OpenText(jsonPath);
				string json = stream.ReadToEnd();
				AccountNames = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
			}
			else
			{
				MessageBox.Show($"The file 'jsonPath' doesn't exist",
					"AccountNames json",
					MessageBoxButton.OK,
					MessageBoxImage.Exclamation);
				Application.Current.Shutdown();
			}

			OpenBalances();
			GetCurrentBalances();
		}

		private async void GetSummaries()
		{
			await GetAccountSummaryAsync();
			await GetImportSummaryAsync();
			CheckMissedTallies();
		}

		public async Task GetAccountSummaryAsync()
		{
			using (BankingDbContext db = new BankingDbContext(Options.DbConnection))
			{
				List<Bank> accounts = await (from a in db.Accounts
																		 select a).ToListAsync();
				Accounts = new ObservableCollection<Bank>(accounts);
			}
#if DEBUG
			Thread.Sleep(1000);
#endif
			GetOrigins();
			CheckMissedTallies();
			NotifyPropertyChanged();
		}

		private async Task GetImportSummaryAsync()
		{
			using (BankingDbContext db = new BankingDbContext(Options.DbConnection))
			{
				List<Import> imports = await (from a in db.Imports
																			select a).ToListAsync();
				Imports = new ObservableCollection<Import>(imports);
			}
			NotifyPropertyChanged();
		}

		private void GetOrigins()
		{
			Origins = new List<string>(Accounts
				.Select(x => x.Origin)
				.OrderBy(x => x)
				.Distinct()
				.ToList());
		}

		private int CheckMissedTallies()
		{
			List<Bank> Missed = Accounts
				.Where(x => x.TallyName is null)
				.OrderBy(x => (x.Account, x.Date))
				.ToList();

			MissedTalliesCount = Missed.Count;
			HasMissedTallies = MissedTalliesCount > 0;
			return MissedTalliesCount;
		}

		private void SetDbConnection()
		{
			if (File.Exists(Options.JsonPath))
			{
				using StreamReader stream = File.OpenText(Options.JsonPath);
				string json = stream.ReadToEnd();
				Options = JsonConvert.DeserializeObject<OptionViewModel>(json);
			}
		}

		public void ShowBankList()
		{
			_ = new BankViewModel(Options, View, this);
		}

		public void ShowImportList()
		{
			_ = new ImportViewModel(Options, View);
		}

		public async Task ShowOverviewAsync()
		{
			List<Bank> missed = Accounts
				.Where(x => x.TallyName is null)
				.ToList();

			if (missed.Count > 0)
			{
				_ = new BankViewModel(Options, View, this, true);

				await GetAccountSummaryAsync();

				missed = Accounts
					.Where(x => x.TallyName is null)
					.ToList();
				HasMissedTallies = missed.Count > 0;
				NotifyPropertyChanged();
			}
			else
			{
				_ = new OverviewWindow(this);
			}
		}

		public void ShowOverview()
		{
			List<Bank> missed = Accounts
				.Where(x => x.TallyName is null)
				.ToList();

			if (missed.Count > 0)
			{
				_ = new BankViewModel(Options, View, this, true);

				_ = GetAccountSummaryAsync();

				missed = Accounts
					.Where(x => x.TallyName is null)
					.ToList();
				HasMissedTallies = missed.Count > 0;
				NotifyPropertyChanged();
			}
			else
			{
				_ = new OverviewWindow(this);
			}
		}

		private void OpenBalances()
		{
			if (File.Exists(BALANCE))
			{
				try
				{
					using StreamReader stream = File.OpenText(BALANCE);
					string json = stream.ReadToEnd();
					Balances = JsonConvert.DeserializeObject<ObservableCollection<Balance>>(json);
				}
				catch (Exception ex)
				{
					Log.Write($"Error opening balances: {ex.Message}");
					MessageBox.Show(ex.Message,
						"Corrupt Balance.json",
						MessageBoxButton.OK,
						MessageBoxImage.Warning);
				}
			}
		}

		public void ShowBalances()
		{
			BalanceWindow balanceWindow = new BalanceWindow(this);
			balanceWindow.ShowDialog();
		}

		public void CloseWindow()
		{
			if (ToSaveBalances)
			{
				SaveBalances();
				ToSaveBalances = false;
			}

			Log.Write("Closing banking");
		}

		public void SaveBalances()
		{
			string json = JsonConvert.SerializeObject(Balances, Formatting.Indented);
			using StreamWriter stream = new StreamWriter(BALANCE);
			stream.Write(json);
			Log.Write("Balances are saved");
		}

		public void GetCurrentBalances()
		{
			CurrentBalances.Clear();

			foreach (Balance balance in Balances)
			{
				List<BalanceAmount> current = balance.Amounts
					.OrderByDescending(x => x.Date)
					.ToList();

				decimal lastAmount = current[0].Amount;
				for (int i = 1; i < current.Count; i++)
				{
					current[i - 1].Difference = lastAmount - current[i].Amount;
					lastAmount = current[i].Amount;
				}

				CurrentBalances.Add(new CurrentBalance
				{
					Name = balance.Name,
					Date = current[0].Date,
					Amount = current[0].Amount,
					Diffence = current[0].Difference
				});
			}
		}

		public void Backup()
		{
			if (!Directory.Exists(Options.BackupPath.TranslatePath()))
			{
				Log.Write($"Folder '{Options.BackupPath}' doesn't exist");
				MessageBox.Show($"Folder '{Options.BackupPath}' doesn't exist",
					"Backup path",
					MessageBoxButton.OK,
					MessageBoxImage.Warning);
				return;
			}

			string backupDate = DateTime.Now.ToString("yyyy-MM-dd_HHmmss");
			string backupFolder = $"{Options.BackupPath.TranslatePath()}\\{Options.DbName}\\{backupDate}";
			Directory.CreateDirectory(backupFolder);

			string backupFile = $"{backupFolder}\\{backupDate}.bak";

			string sql = $"BACKUP DATABASE [{Options.DbName}] TO DISK = '{backupFile}' WITH NOFORMAT, " +
					$"NOFORMAT, NOINIT, SKIP, NOREWIND, NOUNLOAD, STATS = 10, NAME = " +
					$"N'Banking-Full Database Backup';\n" +
				$"BACKUP DATABASE [{Options.DbName}] TO DISK = '{backupFile}' WITH DIFFERENTIAL, NOFORMAT, " +
					$"NOINIT, SKIP, NOREWIND, NOUNLOAD, STATS = 10, NAME = N'Banking-Diff Database Backup';\n" +
				$"BACKUP LOG [{Options.DbName}] TO DISK = '{backupFile}' WITH NOFORMAT, NOINIT, SKIP, " +
					$"NOREWIND, NOUNLOAD, STATS = 10, NAME = N'Banking-Log Database Backup';\n";

			try
			{
				using BankingDbContext db = new BankingDbContext(Options.DbConnection);
				db.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, sql);
				Log.Write($"Database '{Options.DbConnection}' is backed up");
			}
			catch (Exception ex)
			{
				Log.Write($"Error backup: {ex.Message}");
				MessageBox.Show($"{ex.Message}",
					"Backup exception",
					MessageBoxButton.OK,
					MessageBoxImage.Exclamation);
				return;
			}

			backupFile = $"{backupFolder}\\Balance.json";
			if (File.Exists(BALANCE))
			{
				File.Copy(BALANCE, backupFile);
				Log.Write("Balance file is copied");
			}

			backupFile = $"{backupFolder}\\TalliesRules.json";
			string TalliesRulesJson = "%OneDrive%\\Data\\Banking\\TalliesRules.json".TranslatePath();
			if (File.Exists(TalliesRulesJson))
			{
				File.Copy(TalliesRulesJson, backupFile);
				Log.Write("Tallies rules file is copied");
			}

			MessageBox.Show($"Backup created successful",
				"Backup",
				MessageBoxButton.OK,
				MessageBoxImage.Information);
		}

		public void ShowOVCardList()
		{
			OVCardList window = new OVCardList(Options);
			window.ShowDialog();
		}

		public void ShowHistory() => _ = new HistoryWindow()
		{
			Left = View.Left + 20,
			Top = View.Top + 20
		}.ShowDialog();

		public void ShowLastBackup()
		{
			DateTime backupFileTime;
			var backups = Directory.EnumerateFiles(Options.BackupPath.TranslatePath(), $"{Options.DbName}-*.bak").OrderByDescending(x => x);
			if (backups.Count() > 0)
			{
				backupFileTime = File.GetLastAccessTime(backups.FirstOrDefault());
				MessageBox.Show($"The last backup is made on {backupFileTime.ToString("yyyy-MM-dd HH:mm")}.\n" +
					$"{backups.Count()} backups are made.",
					"Last backup",
					MessageBoxButton.OK,
					MessageBoxImage.Information);
			}
			else
			{
				MessageBox.Show($"There no backups in the folder '{Options.BackupPath}'",
					"Warning backups",
					MessageBoxButton.OK,
					MessageBoxImage.Warning);
			}
		}

		internal void ShowTalliesRules()
		{
			TalliesRulesViewModel missed = new TalliesRulesViewModel((Window)View, this);
			missed.ShowTalliesRules();
		}

	}
}
