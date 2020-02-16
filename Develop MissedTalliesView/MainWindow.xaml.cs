using Develop_MissedTalliesView.ViewModels;
using System.Windows;

namespace Develop_MissedTalliesView
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
    }

    private void MissedTalliesButton_Click(object sender, RoutedEventArgs e)
    {
      new MissedTalliesViewModel((Window)this).ShowMissedTallies();
    }
  }
}
