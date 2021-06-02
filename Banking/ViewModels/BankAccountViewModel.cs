using Banking.Models;
using Banking.Views;

using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace Banking.ViewModels
{
	public class BankAccountViewModel : INotifyPropertyChanged
	{
		#region [ Fields ]

		private BankAccountWindow View;
		private readonly MainViewModel VM;

		#endregion

		#region [ Properties ]

		public Bank Account { get; set; }
		public List<string> Tallies { get; set; }

		#endregion

		#region [ Notification method ]

		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		}
		#endregion

		#region [ Construction ]

		public BankAccountViewModel(MainViewModel mainVM)
		{
			VM = mainVM;
		}

		#endregion

		public bool? ShowAccount(BankWindow parent, Bank account, List<string> tallies)
		{
			Account = account;
			Tallies = tallies;

			View = new BankAccountWindow(VM, this)
			{
				Top = parent.Top + 20,
				Left = parent.Left + 20
			};

			if (string.IsNullOrWhiteSpace(Account.TallyName) && Account.Mutation == "Incasso")
			{
				View.MutationTextBox.FontWeight = FontWeights.Bold;
				View.MutationTextBox.Foreground = Brushes.Red;
			}

			View.Owner = parent;
			return View.ShowDialog();
		}

		internal bool CanSave()
		{
			return !string.IsNullOrEmpty(Account.Mutation) && !string.IsNullOrEmpty(Account.TallyName);
		}

		internal bool CanProposal()
		{
			return !string.IsNullOrEmpty(Account.TallyName);
		}

		internal void Proposal()
		{
			TalliesRulesViewModel missed = new TalliesRulesViewModel(View, VM)
			{
				SelectedAccount = new Bank()
				{
					Account = Account.Account,
					Mutation = Account.Mutation,
					Name = Account.Name,
					CounterAccount = Account.CounterAccount,
					Text = Account.Text,
					TallyName = Account.TallyName
				}
			};

			_ = missed.ShowTalliesRules();
		}
	}
}
