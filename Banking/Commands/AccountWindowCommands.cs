using System.Windows.Input;

namespace Banking.Commands
{
	public static class AccountWindowCommands
	{
		public static readonly RoutedUICommand Save = new RoutedUICommand
			(
				"_Save",
				"Save",
				typeof(AccountWindowCommands),
				new InputGestureCollection() { }
			);

		public static readonly RoutedUICommand Proposal = new RoutedUICommand
			(
				"_Proposal",
				"Proposal",
				typeof(AccountWindowCommands),
				new InputGestureCollection() { }
			);

		public static readonly RoutedUICommand Cancel = new RoutedUICommand
			(
				"_Cancel",
				"Cancel",
				typeof(AccountWindowCommands),
				new InputGestureCollection()
				{
					new KeyGesture(Key.Escape)
				}
			);
	}
}
