using Banking.Models;
using Banking.Views;
using System.Data.Entity;
using System.Linq;

namespace Banking.ViewModels
{
  public class ImportViewModel
  {
    private ImportWindow View { get; set; }

    private readonly OptionViewModel Options;

    public ImportViewModel(OptionViewModel options, MainWindow parent)
    {
      Options = options;

      OpenImportTable();

      View = new ImportWindow(this)
      {
        Top = parent.Top + 20,
        Left = parent.Left + 20
      };

      View.ShowDialog();
    }

    private async void OpenImportTable()
    {
      using (BankingDbContext db = new BankingDbContext(Options.DbConnection))
      {
        var Imports = await (from a in db.Imports
                             orderby a.Date descending
                             select a).ToListAsync();
        View.ImportDataGrid.ItemsSource = Imports;
      }
    }

    public void OpenImport(Import import)
    {
      DetailViewModel detailMV = new DetailViewModel();
      detailMV.ShowAccount(View, import);
    }


  }
}
