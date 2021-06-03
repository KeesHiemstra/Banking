using Banking.Models;
using Banking.Views;

using System;
using System.Linq;

namespace Banking.ViewModels
{
	public class BalanceViewModel
	{
		#region [ Fields ]

		private EditBalanceWindow EditBalanceView { get; set; }
		private BalanceAmount CurrentAmount;

		#endregion

		#region [ Properties ]

		public MainViewModel MainVM { get; set; }
		public BalanceWindow BalanceView { get; set; }

		public bool AddAccount { get; set; }
		public bool AddBalance { get; set; }
		public int SelectedBalance { get; set; }
		public string EditAccount { get; set; }
		public DateTime EditDate { get; set; }
		public decimal? EditAmount { get; set; }

		#endregion

		#region [ Constructions ]

		public BalanceViewModel(MainViewModel mainVM, BalanceWindow balanceWindow)
		{
			MainVM = mainVM;
			BalanceView = balanceWindow;
		}

		#endregion

		public int SelectBalance(int selectBalance)
		{
			SelectedBalance = selectBalance;

			if (MainVM.Balances.Count > 0)
			{
				BalanceView.BalanceAmountDataGrid.ItemsSource = MainVM.Balances[selectBalance].Amounts
					.OrderByDescending(x => x.Date);
			}

			return selectBalance;
		}

		public void SaveWindow()
		{
			if (AddAccount)
			{
				MainVM.Balances.Add(new Models.Balance { Name = EditAccount });
				MainVM.ToSaveBalances = true;
				SelectedBalance = MainVM.Balances.Count - 1;
				SelectBalance(SelectedBalance);
			}

			if (MainVM.Balances[SelectedBalance].Name != EditAccount)
			{
				MainVM.Balances[SelectedBalance].Name = EditAccount;
				MainVM.ToSaveBalances = true;
			}

			if (!string.IsNullOrEmpty(EditAmount.ToString()) )
			{
				if (AddBalance)
				{
					MainVM.Balances[SelectedBalance].Amounts.Add(new Models.BalanceAmount
					{
						Date = EditDate.Date,
						Amount = EditAmount.Value
					});
				}
				else
				{
					CurrentAmount.Date = EditDate.Date;
					CurrentAmount.Amount = EditAmount.Value;
				}
				MainVM.ToSaveBalances = true;
			}

			if (MainVM.ToSaveBalances)
			{
				MainVM.GetCurrentBalances();
			}

			EditBalanceView.Close();
		}

		public void CancelWindow()
		{
			EditBalanceView.Close();
		}

		public void NewAccount()
		{
			AddAccount = true;
			AddBalance = true;
			ShowEditBalance(true, true);
		}

		public void NewBalance()
		{
			AddAccount = false;
			AddBalance = true;
			ShowEditBalance(false, true);
		}

		public void EditBalance(BalanceAmount balanceAmount)
		{
			AddAccount = false;
			AddBalance = false;
			CurrentAmount = balanceAmount;
			EditDate = balanceAmount.Date;
			EditAmount = balanceAmount.Amount;
			ShowEditBalance(false, false);
		}

		private void ShowEditBalance(bool newAccountName, bool newBalence)
		{
			if (newAccountName)
			{
				EditAccount = string.Empty;
			}
			else
			{
				EditAccount = MainVM.Balances[SelectedBalance].Name;
			}

			if (newBalence)
			{
				EditDate = DateTime.Now;
				EditAmount = null;
			}

			EditBalanceView = new EditBalanceWindow(this)
			{
				Top = BalanceView.Top + 20,
				Left = BalanceView.Left + 20,
				DataContext = this
			};
			EditBalanceView.AmountTextBox.Focus();
			EditBalanceView.ShowDialog();
			SelectBalance(SelectedBalance);
		}

	}

}
