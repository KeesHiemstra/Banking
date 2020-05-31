using Banking.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Banking.Views
{
  /// <summary>
  /// Interaction logic for MissedTalliesWindow.xaml
  /// </summary>
  public partial class TalliesRulesWindow : Window
  {

    TalliesRulesViewModel RulesVM;

    public TalliesRulesWindow(TalliesRulesViewModel viewModel)
    {

      InitializeComponent();
      RulesVM = viewModel;
      DataContext = viewModel;

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
