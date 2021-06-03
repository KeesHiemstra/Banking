using Banking.ViewModels;

using System.Windows;

namespace Banking.Views
{
	/// <summary>
	/// Interaction logic for OptionWindow.xaml
	/// </summary>
	public partial class OptionWindow : Window
	{
		public OptionWindow(OptionViewModel options)
		{
			InitializeComponent();

			DbConnectionTextBox.Focus();
			DataContext = options;
		}

		private void OKButton_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
		}

		private void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
		}
	}
}
