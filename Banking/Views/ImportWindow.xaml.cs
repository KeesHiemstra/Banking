using Banking.Models;
using Banking.ViewModels;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Banking.Views
{
	/// <summary>
	/// Interaction logic for ImportWindow.xaml
	/// </summary>
	public partial class ImportWindow : Window
	{
		private ImportViewModel ModelView;

		public ImportWindow(ImportViewModel modelView)
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
