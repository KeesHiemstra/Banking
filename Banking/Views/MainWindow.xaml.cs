using Banking.Models;
using Banking.ModelViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Banking
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public static MainModelView MainMV;

    public MainWindow()
    {

      InitializeComponent();

      Title = $"Banking ({System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()})";

      MainMV = new MainModelView(this);
      DataContext = MainMV;

		}

    #region Exit command
    private void ExitCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
      e.CanExecute = true;
    }

    private void ExitCommand_Execute(object sender, ExecutedRoutedEventArgs e)
    {
      Application.Current.Shutdown();
    }
    #endregion

    #region Options command
    private void OptionsCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
      e.CanExecute = true;
    }

    private void OptionsCommand_Execute(object sender, ExecutedRoutedEventArgs e)
    {
      MainMV.Options.ShowOptions(this);
    }
		#endregion

		#region Backup command
		private void BackupCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void BackupCommand_Execute(object sender, ExecutedRoutedEventArgs e)
		{
			MainMV.Backup();
		}
		#endregion

		#region Bank list
		private void BankCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
      e.CanExecute = true;
    }

    private void BankCommand_Execute(object sender, ExecutedRoutedEventArgs e)
    {
      MainMV.ShowBankList();
    }
    #endregion

    #region Import list
    private void ImportCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
      e.CanExecute = true;
    }

    private void ImportCommand_Execute(object sender, ExecutedRoutedEventArgs e)
    {
      MainMV.ShowImportList();
    }
		#endregion

		#region Overview
		private void OverviewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void OverviewCommand_Execute(object sender, ExecutedRoutedEventArgs e)
		{
			MainMV.ShowOverview();
		}
		#endregion

		#region Import ABN file
		private void ImportABNCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void ImportABNCommand_Execute(object sender, ExecutedRoutedEventArgs e)
		{
			MainMV.ImportABNFileAsync();
		}
		#endregion

		#region Import ING file
		private void ImportINGCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void ImportINGCommand_Execute(object sender, ExecutedRoutedEventArgs e)
		{
			MainMV.ImportINGFileAsync();
		}
		#endregion

		#region Process import table
		private void ProcessImportTableCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = MainMV.ImportCount > 0;
		}

		private void ProcessImportTableCommand_Execute(object sender, ExecutedRoutedEventArgs e)
		{
			MainMV.ProcessImportTableAsync();
		}
		#endregion

		#region Clear import table
		private void ClearImportTableCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void ClearImportTableCommand_Execute(object sender, ExecutedRoutedEventArgs e)
		{
			MainMV.ClearImportTable();
		}
		#endregion

		#region Balance overview
		private void BalanceCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void BalanceCommand_Execute(object sender, ExecutedRoutedEventArgs e)
		{
			MainMV.ShowBalances();
		}
		#endregion

		#region Import OV-Card file
		private void ImportOVCardCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void ImportOVCardCommand_Execute(object sender, ExecutedRoutedEventArgs e)
		{
			MainMV.ImportOVCardFile();
		}
		#endregion

		#region OV-Card list
		private void OVCardListCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void OVCardListCommand_Execute(object sender, ExecutedRoutedEventArgs e)
		{
			MainMV.ShowOVCardList();
		}
    #endregion

    #region History
    private void HistoryCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void HistoryCommand_Execute(object sender, ExecutedRoutedEventArgs e)
		{
			MainMV.ShowHistory();
		}
    #endregion

		#region Last backup
		private void LastBackupCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
		}

		private void LastBackupCommand_Execute(object sender, ExecutedRoutedEventArgs e)
		{
			MainMV.ShowLastBackup();
		}
		#endregion

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			MainMV.CloseWindow();
		}

	}
}
