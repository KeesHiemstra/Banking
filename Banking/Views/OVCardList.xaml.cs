using Banking.ViewModels;

using System.Windows;

namespace Banking.Views
{
  /// <summary>
  /// Interaction logic for OVCardList.xaml
  /// </summary>
  public partial class OVCardList : Window
	{
		OVCardViewModel CardMV { get; set; }

		public OVCardList(OptionViewModel options)
		{
			InitializeComponent();

			CardMV = new OVCardViewModel(options, this);
			OVCardDataGrid.ItemsSource = CardMV.Cards;
		}

	}
}
