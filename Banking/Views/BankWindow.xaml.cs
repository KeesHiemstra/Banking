using Banking.Models;
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
using System.Windows.Shapes;

namespace Banking.Views
{
  /// <summary>
  /// Interaction logic for BankWindow.xaml
  /// </summary>
  public partial class BankWindow : Window
  {
    private BankViewModel ModelView;

    public BankWindow(BankViewModel modelView)
    {
      InitializeComponent();

      ModelView = modelView;
			DataContext = ModelView;
    }

    private void BankingDataGrid_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter)
      {
        ModelView.OpenAccount((Bank)((DataGrid)sender).CurrentItem);
      }
    }

    private void BankingDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      ModelView.OpenAccount((Bank)((DataGrid)sender).CurrentItem);
    }

		private void FilterButton_Click(object sender, RoutedEventArgs e)
		{
			ModelView.OpenBankTable();
		}
	}
}
