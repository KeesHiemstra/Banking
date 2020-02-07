using CHi.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Develop_MissedTalliesView.ViewModels
{
  public class MissedTalliesViewModel
  {

    #region [ Fields ]

    Window Parent;
    //ToDo: Get is from Banking
    string MissedTalliesJsonFile = "%OneDrive%\\Data\\Banking\\MissedTallies.json".TranslatePath();

    #endregion

    #region [ Properties ]

    public Dictionary<string, string> MissedTallies { get; set; } = new Dictionary<string, string>();

    #endregion

    #region [ Constructor ] 
    public MissedTalliesViewModel(Window view)
    {

      Parent = view;
      if (File.Exists(MissedTalliesJsonFile))
      {
        using (StreamReader stream = File.OpenText(MissedTalliesJsonFile))
        {
          string json = stream.ReadToEnd();
          MissedTallies = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        }
      }
      view.DataContext = this;

    }

    #endregion

    public void ShowMissedTallies()
    {

      MissedTalliesWindow missedTalliesView = new MissedTalliesWindow()
      {
        Left = Parent.Left + 20,
        Top = Parent.Top + 20
      };
      missedTalliesView.MissedTalliesListBox.ItemsSource = MissedTallies;
      missedTalliesView.ShowDialog();

    }

  }
}
