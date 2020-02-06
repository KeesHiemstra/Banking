using Banking.Models;
using Banking.Views;

namespace Banking.ModelViews
{
  public class DetailModelView
  {
    public Import Detail { get; set; }

    public void ShowAccount(ImportWindow parent, Import detail)
    {
      Detail = detail;

      DetailWindow view = new DetailWindow(this)
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




  }
}
