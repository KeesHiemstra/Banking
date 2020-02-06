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
  /// Interaction logic for AccountWindow.xaml
  /// </summary>
  public partial class AccountWindow : Window
  {
    public AccountWindow(AccountModelView account)
    {
      InitializeComponent();

      CancelButton.Focus();
      DataContext = account;
    }

    private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
    {
      e.CanExecute = true;
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
