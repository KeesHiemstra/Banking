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
		private readonly MainViewModel MainVM;

    #endregion

    public Bank Account { get; set; }
    public List<string> Tallies { get; set; }

    #region [ Method ]

    public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(string propertyName = "")
    {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		
    }
    #endregion

    #region [ Construction ]

    public BankAccountViewModel(MainViewModel mainVM)
    {
      MainVM = mainVM;
    }

    #endregion

    public bool? ShowAccount(BankWindow parent, Bank account, List<string> tallies)
    {
      Account = account;
      Tallies = tallies;

      View = new BankAccountWindow(this)
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
      if (string.IsNullOrEmpty(Account.Mutation))
      {
        return false;
      }

      if (string.IsNullOrEmpty(Account.TallyName))
      {
        return false;
      }

      return true;
    }

    internal bool CanProposal()
    {
      if (string.IsNullOrEmpty(Account.TallyName))
      {
        return false;
      }

      return true;
    }

    internal void Proposal()
    {
      TalliesRulesViewModel missed = new TalliesRulesViewModel((Window)View, MainVM)
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

      missed.ShowTalliesRules();
		}
}
}
