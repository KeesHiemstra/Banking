using Banking.ViewModels;

using System.Windows;

namespace Banking.Views
{
  /// <summary>
  /// Interaction logic for EditBalanceWindow.xaml
  /// </summary>
  public partial class EditBalanceWindow : Window
	{
		public BalanceViewModel BalanceMV { get; set; }

		public EditBalanceWindow(BalanceViewModel balanceModelView)
		{
			InitializeComponent();

			BalanceMV = balanceModelView;
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			BalanceMV.SaveWindow();
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			BalanceMV.CancelWindow();
		}
	}
}
