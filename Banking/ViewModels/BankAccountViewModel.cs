using Banking.Models;
using Banking.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace Banking.ViewModels
{
	public class BankAccountViewModel : INotifyPropertyChanged
	{
		public Bank Account { get; set; }
		public List<string> Tallies { get; set; }

    #region [ Method ]
    public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(string propertyName = "")
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
    #endregion

    public bool? ShowAccount(BankWindow parent, Bank account, List<string> tallies)
		{
			Account = account;
			Tallies = tallies;

			BankAccountWindow view = new BankAccountWindow(this)
			{
				Top = parent.Top + 20,
				Left = parent.Left + 20
			};

			if (string.IsNullOrWhiteSpace(Account.TallyName) && Account.Mutation == "Incasso")
			{
				view.MutationTextBox.FontWeight = FontWeights.Bold;
				view.MutationTextBox.Foreground = Brushes.Red;
			}

			view.Owner = parent;
			return view.ShowDialog();
		}

		internal bool CanSave()
		{
			if (Account.Mutation == "Incasso")
			{
				return false;
			}

			if (string.IsNullOrEmpty(Account.TallyName))
			{
				return false;
			}

			return true;
		}
	}
}
