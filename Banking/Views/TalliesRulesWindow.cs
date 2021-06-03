using Banking.ViewModels;

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Banking.Views
{
	/// <summary>
	/// Interaction logic for MissedTalliesWindow.xaml
	/// </summary>
	public partial class TalliesRulesWindow : Window
	{

		private readonly TalliesRulesViewModel RulesVM;

		public TalliesRulesWindow(TalliesRulesViewModel rulesVM)
		{
			InitializeComponent();
			RulesVM = rulesVM;
			DataContext = rulesVM;
		}

		private void TalliesRulesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			RulesVM.SelectItem((KeyValuePair<string, string>)((ListBox)sender).SelectedItem);
		}

		private void FullTallyNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			RulesVM.RecordIsUpdated();
		}

		private void SQLTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			RulesVM.RecordIsUpdated();
		}

		#region Close command
		private void CloseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void CloseCommand_Execute(object sender, ExecutedRoutedEventArgs e)
		{
			RulesVM.CloseWindow();
		}

		#endregion

		#region Add command

		private void AddCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			//Can it always?
			e.CanExecute = true;
		}

		private void AddCommand_Execute(object sender, ExecutedRoutedEventArgs e)
		{
			RulesVM.AddTallyItem();
		}

		#endregion

		#region Save command

		private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			//Can it always?
			if (RulesVM is null) { return; }
			e.CanExecute = RulesVM.CanSaveTallyItem();
		}

		private void SaveCommand_Execute(object sender, ExecutedRoutedEventArgs e)
		{
			RulesVM.SaveTallyItem();
		}

		#endregion

	}
}
