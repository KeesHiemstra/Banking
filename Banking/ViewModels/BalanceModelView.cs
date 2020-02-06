using Banking.Models;
using Banking.Views;
using System;

namespace Banking.ModelViews
{
	public class BalanceModelView
	{
		private EditBalanceWindow EditBalanceView { get; set; }
		private BalanceAmount CurrentAmount;

		public MainModelView MainMV { get; set; }
		public BalanceWindow BalanceView { get; set; }

		public bool AddAccount { get; set; }
		public bool AddBalance { get; set; }
		public int SelectedBalance { get; set; }
		public string EditAccount { get; set; }
		public DateTime EditDate { get; set; }
		public decimal? EditAmount { get; set; }

		public BalanceModelView(MainModelView mainMV, BalanceWindow balanceWindow)
		{

			MainMV = mainMV;
			BalanceView = balanceWindow;

		}

		public int SelectBalance(int selectBalance)
		{

			SelectedBalance = selectBalance;

			if (MainMV.Balances.Count > 0)
			{
				BalanceView.BalanceAmountDataGrid.ItemsSource = MainMV.Balances[selectBalance].Amounts;
			}

			return selectBalance;

		}

		public void SaveWindow()
		{

			if (AddAccount)
			{
				MainMV.Balances.Add(new Models.Balance { Name = EditAccount });
				MainMV.ToSaveBalances = true;
				SelectedBalance = MainMV.Balances.Count - 1;
				SelectBalance(SelectedBalance);
			}

			if (MainMV.Balances[SelectedBalance].Name != EditAccount)
			{
				MainMV.Balances[SelectedBalance].Name = EditAccount;
				MainMV.ToSaveBalances = true;
			}

			if (!string.IsNullOrEmpty(EditAmount.ToString()) )
			{
				if (AddBalance)
				{
					MainMV.Balances[SelectedBalance].Amounts.Add(new Models.BalanceAmount
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
				MainMV.ToSaveBalances = true;
			}

			if (MainMV.ToSaveBalances)
			{
				MainMV.GetCurrentBalances();
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
				EditAccount = MainMV.Balances[SelectedBalance].Name;
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
		}

	}

}
