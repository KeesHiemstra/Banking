﻿using Banking.Models;
using Banking.ViewModels;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Banking.Views
{
	/// <summary>
	/// Interaction logic for BalanceWindow.xaml
	/// </summary>
	public partial class BalanceWindow : Window
	{
		public MainViewModel MainMV { get; set; }
		public BalanceViewModel BalanceMV { get; set; } 

		public BalanceWindow(MainViewModel mainModelView )
		{
			InitializeComponent();

			MainMV = mainModelView;
			BalanceMV = new BalanceViewModel(MainMV, this);
			Top = MainMV.View.Top + 20;
			Left = MainMV.View.Left + 20;

			DataContext = MainMV;
			BalanceNameComboBox.ItemsSource = MainMV.Balances;
			BalanceNameComboBox.SelectedIndex = BalanceMV.SelectBalance(0);
		}

		private void BalanceNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			int selectedBalance = (int)((ComboBox)sender).SelectedIndex;
			if (selectedBalance >= 0 && selectedBalance < BalanceMV.MainVM.Balances.Count)
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
