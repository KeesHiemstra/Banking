using Banking.Models;
using Banking.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Banking.Views
{
	/// <summary>
	/// Interaction logic for BalanceWindow.xaml
	/// </summary>
	public partial class BalanceWindow : Window
	{
		public MainModelView MainMV { get; set; }
		public BalanceModelView BalanceMV { get; set; } 

		public BalanceWindow(MainModelView mainModelView )
		{

			InitializeComponent();

			MainMV = mainModelView;
			BalanceMV = new BalanceModelView(MainMV, this);
			Top = MainMV.View.Top + 20;
			Left = MainMV.View.Left + 20;

			DataContext = MainMV;
			BalanceNameComboBox.ItemsSource = MainMV.Balances;
			BalanceNameComboBox.SelectedIndex = BalanceMV.SelectBalance(0);

		}

		private void BalanceNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

			int selectedBalance = (int)((ComboBox)sender).SelectedIndex;
			if (selectedBalance >= 0 && selectedBalance < BalanceMV.MainMV.Balances.Count)
			{
				BalanceMV.SelectBalance(selectedBalance);
			}

		}

		private void NewAccountButton_Click(object sender, RoutedEventArgs e)
		{
			BalanceMV.NewAccount();
		}

		private void NewBalanceButton_Click(object sender, RoutedEventArgs e)
		{
			BalanceMV.NewBalance();
		}

		private void BalanceAmountDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			BalanceMV.EditBalance((BalanceAmount)((DataGrid)sender).CurrentItem);
		}
	}
}
