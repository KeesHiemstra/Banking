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
	/// Interaction logic for OVCardList.xaml
	/// </summary>
	public partial class OVCardList : Window
	{
		OVCardModelView CardMV { get; set; }

		public OVCardList(OptionModelView options)
		{
			InitializeComponent();

			CardMV = new OVCardModelView(options, this);
			OVCardDataGrid.ItemsSource = CardMV.Cards;
		}

	}
}
