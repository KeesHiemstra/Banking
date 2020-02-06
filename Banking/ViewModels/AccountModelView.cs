using Banking.Models;
using Banking.Views;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace Banking.ModelViews
{
	public class AccountModelView : INotifyPropertyChanged
	{
		public Bank Account { get; set; }
		public List<string> Tallies { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(string propertyName = "")
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public bool? ShowAccount(BankWindow parent, Bank account, List<string> tallies)
		{
			Account = account;
			Tallies = tallies;

			AccountWindow view = new AccountWindow(this)
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

	}
}
