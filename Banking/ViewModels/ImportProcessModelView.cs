using Banking.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Banking.ModelViews
{
	public partial class ImportProcessModelView
	{
		private OptionModelView Options { get; }
		private MainModelView MainMV { get; }
		private int MissedTalliesCount;

		public ImportProcessModelView(OptionModelView options, MainModelView mainMV)
		{
			Options = options;
			MainMV = mainMV;

			ProcessImportToBankAsync();
			ProcessMissedTallies();
			ProcessPostImportAsync();

		}

		private int CheckMissedTallies()
		{

			List<Bank> Missed = MainMV.Accounts
				.Where(x => x.TallyName is null)
				.ToList();

			return Missed.Count;

		}
		private string Log(string message)
		{

			string LogFileName = Assembly.GetEntryAssembly().Location.Replace(".exe", ".log");

			using (StreamWriter stream = new StreamWriter(LogFileName, true))
			{
				stream.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")} {message}");
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

			await MainMV.GetAccountSummaryAsync();
			MissedTalliesCount = CheckMissedTallies();
			Log($"After ProcessImportToBank() has {MissedTalliesCount} missed tallies");

		}

		private async Task ProcessMissedTallies()
		{

			int updates;
			int totalUpdates = 0;

			Log($"Start ProcessMissedTallies() with {CheckMissedTallies()} missed tallies");

			DictMissedTallies();

			using (BankingDbContext db = new BankingDbContext(Options.DbConnection))
			{
				foreach (var item in MissedTallies)
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
			MissedTallies.Clear();

			await MainMV.GetAccountSummaryAsync();
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
