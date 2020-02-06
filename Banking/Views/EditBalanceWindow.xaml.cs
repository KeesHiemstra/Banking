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
	/// Interaction logic for EditBalanceWindow.xaml
	/// </summary>
	public partial class EditBalanceWindow : Window
	{
		public BalanceModelView BalanceMV { get; set; }

		public EditBalanceWindow(BalanceModelView balanceModelView)
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
