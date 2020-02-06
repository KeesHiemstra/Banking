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
  /// Interaction logic for ImportWindow.xaml
  /// </summary>
  public partial class ImportWindow : Window
  {
    private ImportModelView ModelView;

    public ImportWindow(ImportModelView modelView)
    {
      InitializeComponent();

      ModelView = modelView;
    }

    private void ImportDataGrid_KeyUp(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter)
      {
        ModelView.OpenImport((Import)((DataGrid)sender).CurrentItem);
      }
    }

    private void ImportDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
      ModelView.OpenImport((Import)((DataGrid)sender).CurrentItem);
    }


  }
}
