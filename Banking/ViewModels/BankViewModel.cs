using Banking.Models;
using Banking.Views;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;

namespace Banking.ViewModels
{
	public class BankViewModel : INotifyPropertyChanged
	{

		#region [ Fields ]

		private readonly bool HasMissedTallies;
		private readonly MainViewModel MainVM;
		private readonly OptionViewModel Options;

		#endregion

		#region [ Properties ]

		private BankWindow View { get; set; }
		public BankingDbContext Db { get; set; }
		private ObservableCollection<Bank> Accounts { get; set; }
		private ObservableCollection<Bank> FilteredAccounts { get; set; }
		public List<string> Tallies { get; set; } = new List<string>();
		public string AccountFilter { get; set; }

		#endregion

		#region [ Constructions ]

		public BankViewModel(OptionViewModel options, MainWindow parent, MainViewModel mainVM, bool hasMissedTallies = false)
		{
			Options = options;
			MainVM = mainVM;
			HasMissedTallies = hasMissedTallies;

			OpenBankTable();
			View = new BankWindow(this)
			{
				Top = parent.Top + 20,
				Left = parent.Left + 20
			};

			bool? Result = View.ShowDialog();
			if ((bool)Result)
			{
				Db.Dispose();
			}
		}

		public BankViewModel(OptionViewModel options, OverviewWindow parent, MainViewModel mainVM, string tallyName, string month)
		{
			Options = options;
			MainVM = mainVM;

			OpenBankTable(tallyName, month);
			View = new BankWindow(this)
			{
				Top = parent.Top + 20,
				Left = parent.Left + 20
			};

			bool? Result = View.ShowDialog();
			if ((bool)Result)
			{
				Db.Dispose();
			}
		}

		#endregion

		#region [ Public methods ]

		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion

		public async void OpenBankTable(string TallyName = null, string Month = null)
		{
			Db = new BankingDbContext(Options.DbConnection);
			{
				List<Bank> accounts = await (from a in Db.Accounts
																		 orderby a.Date descending
																		 select a).ToListAsync();
				Accounts = new ObservableCollection<Bank>(accounts);

				Tallies = Accounts
					.Select(x => x.TallyName)
					.Distinct()
					.OrderBy(x => x)
					.ToList();

				if (HasMissedTallies)
				{
					List<Bank> filteredAccounts = Accounts
						.Where(x => x.TallyName is null)
						.ToList();

					FilteredAccounts = new ObservableCollection<Bank>(filteredAccounts);
					View.BankingDataGrid.ItemsSource = FilteredAccounts;
					View.Title = "Gemiste markeringen";
				}
				else if (!(string.IsNullOrWhiteSpace(TallyName) && string.IsNullOrWhiteSpace(Month)))
				{
					List<Bank> filteredAccounts = Accounts
						.Where(x => x.TallyName == TallyName && x.Month == Month)
						.ToList();

					FilteredAccounts = new ObservableCollection<Bank>(filteredAccounts);
					View.BankingDataGrid.ItemsSource = FilteredAccounts;
				}
				else if (!string.IsNullOrWhiteSpace(AccountFilter))
				{
					List<Bank> filteredAccounts = Accounts
						.Where(x => x.RawText.ToLower().Contains(AccountFilter.ToLower()) ||
							x.Name.ToLower().Contains(AccountFilter.ToLower()))
						.ToList();

					FilteredAccounts = new ObservableCollection<Bank>(filteredAccounts);
					View.BankingDataGrid.ItemsSource = FilteredAccounts;
				}
				else
				{
					View.BankingDataGrid.ItemsSource = Accounts;
				}
			}
		}

		public void OpenAccount(Bank account)
		{
			BankAccountViewModel accountMV = new BankAccountViewModel(MainVM);
			bool? Result = accountMV.ShowAccount(View, account, Tallies);

			if ((bool)Result)
			{
				Db.SaveChanges();

				View.BankingDataGrid.ItemsSource = null;
				if (HasMissedTallies)
				{
					FilteredAccounts = null;
					var filteredAccounts = Accounts
						.Where(x => x.TallyName is null)
						.ToList();

					FilteredAccounts = new ObservableCollection<Bank>(filteredAccounts);
					View.BankingDataGrid.ItemsSource = FilteredAccounts;
				}
				else
				{
					View.BankingDataGrid.ItemsSource = Accounts;
				}
			}
		}
	}

}
