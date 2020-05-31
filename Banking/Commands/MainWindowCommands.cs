using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Banking.Commands
{
	public static class MainWindowCommands
	{
		public static readonly RoutedUICommand Options = new RoutedUICommand
			(
				"_Options",
				"Options",
				typeof(MainWindowCommands),
				new InputGestureCollection() { }
			);

		public static readonly RoutedUICommand Backup = new RoutedUICommand
			(
				"_Backup",
				"Backup",
				typeof(MainWindowCommands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.B, ModifierKeys.Alt)
				}
			);

		public static readonly RoutedUICommand Exit = new RoutedUICommand
			(
				"E_xit",
				"Exit",
				typeof(MainWindowCommands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.F4, ModifierKeys.Alt)
				}
			);

		public static readonly RoutedUICommand Bank = new RoutedUICommand
			(
				"_Rekening lijst",
				"Bank",
				typeof(MainWindowCommands),
				new InputGestureCollection() { }
			);

		public static readonly RoutedUICommand Import = new RoutedUICommand
			(
				"_Import lijst",
				"Import",
				typeof(MainWindowCommands),
				new InputGestureCollection() { }
			);

		public static readonly RoutedUICommand Balance = new RoutedUICommand
			(
				"_Saldo overzicht",
				"Balance",
				typeof(MainWindowCommands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.S, ModifierKeys.Alt)
				}
			);

		public static readonly RoutedUICommand Overview = new RoutedUICommand
			(
				"_Overzicht",
				"Overview",
				typeof(MainWindowCommands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.O, ModifierKeys.Control)
				}
			);

		public static readonly RoutedUICommand ImportABN = new RoutedUICommand
		(
			"Import _ABN file",
			"ImportABN",
			typeof(MainWindowCommands),
			new InputGestureCollection() { }
		);

		public static readonly RoutedUICommand ImportING = new RoutedUICommand
		(
			"Import _ING file",
			"ImportING",
			typeof(MainWindowCommands),
			new InputGestureCollection() { }
		);

		public static readonly RoutedUICommand ProcessImportTable = new RoutedUICommand
		(
			"_Process import table",
			"ProcessImportTable",
			typeof(MainWindowCommands),
			new InputGestureCollection() { }
		);

		public static readonly RoutedUICommand ClearImportTable = new RoutedUICommand
		(
			"_Clear Import table",
			"ClearImportTable",
			typeof(MainWindowCommands),
			new InputGestureCollection() { }
		);

		public static readonly RoutedUICommand TalliesRules = new RoutedUICommand
		(
			"_Tally's regels lijst",
			"MissedTallies",
			typeof(MainWindowCommands),
			new InputGestureCollection() { }
		);

		public static readonly RoutedUICommand ImportOVCard = new RoutedUICommand
		(
			"_Import OV-Card file",
			"ImportOVCard",
			typeof(MainWindowCommands),
			new InputGestureCollection() { }
		);

		public static readonly RoutedUICommand OVCardList = new RoutedUICommand
		(
			"_OV card lijst",
			"ShowOVCardList",
			typeof(MainWindowCommands),
			new InputGestureCollection() { }
		);

		public static readonly RoutedUICommand History = new RoutedUICommand
		(
			"_History",
			"ShowHistory",
			typeof(MainWindowCommands),
			new InputGestureCollection() { }
		);

		public static readonly RoutedUICommand About = new RoutedUICommand
		(
			"_About",
			"ShowAbout",
			typeof(MainWindowCommands),
			new InputGestureCollection() { }
		);

		public static readonly RoutedUICommand LastBackup = new RoutedUICommand
		(
			"_Last backup",
			"ShowLastBackup",
			typeof(MainWindowCommands),
			new InputGestureCollection() { }
		);

	}
}
