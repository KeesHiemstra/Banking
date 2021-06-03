using Banking.Models;
using Banking.Views;

namespace Banking.ViewModels
{
	public class ImportDetailViewModel
	{
		public Import Detail { get; set; }

		public void ShowAccount(ImportWindow parent, Import detail)
		{
			Detail = detail;

			ImportDetailWindow view = new ImportDetailWindow(this)
			{
				Top = parent.Top + 20,
				Left = parent.Left + 20
			};

			bool? Result = view.ShowDialog();
			if ((bool)Result)
			{
				//Save the change
				
			}
		}

		internal bool CanSave()
		{
			return !string.IsNullOrEmpty(Detail.Mutation);
		}
	}
}
