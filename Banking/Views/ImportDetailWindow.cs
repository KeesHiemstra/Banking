using Banking.ViewModels;

using System.Windows;
using System.Windows.Input;

namespace Banking.Views
{
  /// <summary>
  /// Interaction logic for DetailWindow.xaml
  /// </summary>
  public partial class ImportDetailWindow : Window
  {
		readonly ImportDetailViewModel DetailVM;

    public ImportDetailWindow(ImportDetailViewModel detail)
    {
      InitializeComponent();

      DetailVM = detail;
      DataContext = DetailVM;
    }

    private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
      e.CanExecute = DetailVM.CanSave();
    }

    private void SaveCommand_Execute(object sender, ExecutedRoutedEventArgs e)
    {
			DialogResult = true;
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
