using Banking.Models;
using Banking.Views;
using System;

namespace Banking.ViewModels
{
  public class DetailViewModel
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

    internal bool CanSave()
    {
      return !string.IsNullOrEmpty(Detail.Mutation);
    }
  }
}
