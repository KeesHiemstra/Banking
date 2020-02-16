using Develop_MissedTalliesView.ViewModels;
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

namespace Develop_MissedTalliesView
{
  /// <summary>
  /// Interaction logic for MissedTalliesWindow.xaml
  /// </summary>
  public partial class MissedTalliesWindow : Window
  {

    MissedTalliesViewModel MissedTalliesViewModel;

    public MissedTalliesWindow(MissedTalliesViewModel viewModel)
    {

      InitializeComponent();
      MissedTalliesViewModel = viewModel;
      DataContext = viewModel;

    }

    private void MissedTalliesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      MissedTalliesViewModel.SelectItem((KeyValuePair<string, string>)((ListBox)sender).SelectedItem);
    }

    private void FullTallyNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
      MissedTalliesViewModel.RecordIsUpdated();
    }

    private void SQLTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
      MissedTalliesViewModel.RecordIsUpdated();
    }
  }
}
