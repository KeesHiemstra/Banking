using Banking.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Banking.ViewModels
{
	public partial class ImportProcessViewModel
	{
		private OptionViewModel Options { get; }
		private MainViewModel MainVM { get; }
		private int MissedTalliesCount;

		public ImportProcessViewModel(OptionViewModel options, MainViewModel mainVM)
		{
			Options = options;
			MainVM = mainVM;

			ProcessImportToBankAsync();
			ProcessMissedTallies();
			ProcessPostImportAsync();
		}

		private int CheckMissedTallies()
		{
			List<Bank> Missed = MainVM.Accounts
				.Where(x => x.TallyName is null)
				.ToList();

			return Missed.Count;
		}

		private string Log(string message)
		{
			string LogFileName = Assembly.GetEntryAssembly().Location.Replace(".exe", ".log");

			using (StreamWriter stream = new StreamWriter(LogFileName, true))
			{
				stream.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {message}");
			}

			return message;
		}

		private string Log(string process, int updates)
		{
			string message = $"'{process}' resulted in {updates} updates";
			Log(message);

			return message;
		}

		private async Task ProcessImportToBankAsync()
		{
			int updates;

			Log($"Start ProcessImportToBank() with {CheckMissedTallies()} missed tallies");

			DictImportToBank();

			using (BankingDbContext db = new BankingDbContext(Options.DbConnection))
			{
				foreach (var item in ImportToBank)
				{
					try
					{
						updates = await db.Database.ExecuteSqlCommandAsync(item.Value);
						Log(item.Key, updates);
					}
					catch (Exception ex)
					{
						Log($"Error in SQL '{item.Key}' with [{ex}]");
					}
				}
			}

			ImportToBank.Clear();

			await MainVM.GetAccountSummaryAsync();
			MissedTalliesCount = CheckMissedTallies();
			Log($"After ProcessImportToBank() has {MissedTalliesCount} missed tallies");
		}

		private async Task ProcessMissedTallies()
		{
			int updates;
			int totalUpdates = 0;

			Log($"Start ProcessMissedTallies() with {CheckMissedTallies()} missed tallies");

      if (MainVM.TalliesRules.Count == 0)
      {
        MainVM.ReadTalliesRules();
      }

			using (BankingDbContext db = new BankingDbContext(Options.DbConnection))
			{
				foreach (var item in MainVM.TalliesRules)
				{
					try
					{
						updates = await db.Database.ExecuteSqlCommandAsync(item.Value);
						totalUpdates += updates;

						if (updates > 0)
						{
							Log(item.Key, updates);
						}
					}
					catch (Exception ex)
					{
						Log($"Error in SQL '{item.Key}' with [{ex}]");
					}
				}
			}

			Log($"Total {totalUpdates} updates");
			//MainVM.TalliesRules.Clear();

			await MainVM.GetAccountSummaryAsync();
			MissedTalliesCount = CheckMissedTallies();
			Log($"After ProcessImportToBank() has {MissedTalliesCount} missed tallies");
		}

		private async Task ProcessPostImportAsync()
		{
			int updates;

			DictPostImport();

			using (BankingDbContext db = new BankingDbContext(Options.DbConnection))
			{
				foreach (var item in PostImport)
				{
					try
					{
						updates = await db.Database.ExecuteSqlCommandAsync(item.Value);
						Log(item.Key, updates);
					}
					catch (Exception ex)
					{
						Log($"Error in SQL '{item.Key}' with [{ex}]");
					}
				}
			}

			PostImport.Clear();
		}

	}
}
