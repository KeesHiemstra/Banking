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
		private BankingDbContext db;
		private bool HasMissedTallies;

    private BankWindow View { get; set; }
		public BankingDbContext Db { get => db; set => db = value; }
		private ObservableCollection<Bank> Accounts { get; set; }
		private ObservableCollection<Bank> FilteredAccounts { get; set; }
		private readonly OptionViewModel Options;

    public List<string> Tallies { get; set; } = new List<string>();
		public string AccountFilter { get; set; }


		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(string propertyName = "")
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public BankViewModel(OptionViewModel options, MainWindow parent, bool hasMissedTallies = false)
    {
      Options = options;
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
				db.Dispose();
			}

		}

		public BankViewModel(OptionViewModel options, OverviewWindow parent, string tallyName, string month)
		{
			Options = options;

			OpenBankTable(tallyName, month);
			View = new BankWindow(this)
			{
				Top = parent.Top + 20,
				Left = parent.Left + 20
			};

			bool? Result = View.ShowDialog();
			if ((bool)Result)
			{
				db.Dispose();
			}

		}

		public async void OpenBankTable(string TallyName = null, string Month = null)
    {
			Db = new BankingDbContext(Options.DbConnection);
      {
        var accounts = await(from a in Db.Accounts
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
					var filteredAccounts = Accounts
						.Where(x => x.TallyName is null)
						.ToList();

					FilteredAccounts = new ObservableCollection<Bank>(filteredAccounts);
					View.BankingDataGrid.ItemsSource = FilteredAccounts;
				}
				else if (!(string.IsNullOrWhiteSpace(TallyName) && string.IsNullOrWhiteSpace(Month)))
				{
					var filteredAccounts = Accounts
						.Where(x => (x.TallyName == TallyName && x.Month == Month))
						.ToList();

					FilteredAccounts = new ObservableCollection<Bank>(filteredAccounts);
					View.BankingDataGrid.ItemsSource = FilteredAccounts;
				}
				else if (!string.IsNullOrWhiteSpace(AccountFilter))
				{
					var filteredAccounts = Accounts
						.Where(x => (x.RawText.ToLower().Contains(AccountFilter.ToLower()) || 
							x.Name.ToLower().Contains(AccountFilter.ToLower())))
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
      BankAccountViewModel accountMV = new BankAccountViewModel();
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
