﻿using Banking.ViewModels;

using System.Windows;
using System.Windows.Input;

namespace Banking.Views
{
	/// <summary>
	/// Interaction logic for AccountWindow.xaml
	/// </summary>
	public partial class BankAccountWindow : Window
	{
		private readonly BankAccountViewModel AccountVM;
		private readonly MainViewModel VM;

		public BankAccountWindow(MainViewModel mainVM, BankAccountViewModel account)
		{
			VM = mainVM;
			AccountVM = account;

			InitializeComponent();

			_ = CancelButton.Focus();
			DataContext = AccountVM;

			if (!AccountVM.CanProposal())
			{
				ProposalButton.Visibility = Visibility.Visible;
			}
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			OriginComboBox.ItemsSource = VM.Origins;
			OriginComboBox.IsEditable = !(AccountVM.Account.Mutation == "Betaalautomaat" ||
				AccountVM.Account.Mutation == "Geldautomaat");
		}

		private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = AccountVM.CanSave();
		}

		private void SaveCommand_Execute(object sender, ExecutedRoutedEventArgs e)
		{
			DialogResult = true;
		}

		private void ProposalCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = AccountVM.CanProposal();
		}

		private void ProposalCommand_Execute(object sender, ExecutedRoutedEventArgs e)
		{
			AccountVM.Proposal();
		}

		private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void CancelCommand_Execute(object sender, ExecutedRoutedEventArgs e)
		{
			DialogResult = false;
		}

	}
}
