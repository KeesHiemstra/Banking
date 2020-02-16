using CHi.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Develop_MissedTalliesView.ViewModels
{
  public class MissedTalliesViewModel : INotifyPropertyChanged
  {

    #region [ Fields ]

    Window Parent;
    MissedTalliesWindow MissedTalliesView;

    //ToDo: Get is from Banking
    string MissedTalliesJsonFile = "%OneDrive%\\Data\\Banking\\MissedTallies.json".TranslatePath();
    private string tallyName;
    private string fullTallyName;
    private string sql;
    private bool isRecordUpdated = false;

    #endregion

    #region [ Properties ]

    public Dictionary<string, string> MissedTallies { get; set; } = new Dictionary<string, string>();
    public List<string> Tallies { get; set; }
    public string TallyName
    {
      get => tallyName;
      set
      {
        if (tallyName != value)
        {
          tallyName = value;
          NotifyPropertyChanged("TallyName");
        }
      }
    }
    public string FullTallyName
    {
      get => fullTallyName;
      set
      {
        if (fullTallyName != value)
        {
          fullTallyName = value;
          NotifyPropertyChanged("FullTallyName");
        }      }
    }
    public string Sql 
    { 
      get => sql;
      set
      {
        if (sql != value)
        {
          sql = value;
          NotifyPropertyChanged("Sql");
        }      }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    private void NotifyPropertyChanged(string propertyName = "")
    {
      if (PropertyChanged != null)
      {
        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }
    }

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

    }

    #endregion

    public void ShowMissedTallies()
    {

      MissedTalliesView = new MissedTalliesWindow(this)
      {
        Left = Parent.Left + 20,
        Top = Parent.Top + 20
      };
      MissedTalliesView.MissedTalliesListBox.ItemsSource = MissedTallies;
      MissedTalliesView.MissedTalliesListBox.SelectedItem = MissedTallies.FirstOrDefault();
      MissedTalliesView.ShowDialog();

    }

    public void SelectItem(KeyValuePair<string, string> keyValue)
    {

      string Pattern = @"\s+\d{3}$";
      Regex check = new Regex(Pattern);

      TallyName = check.Replace(keyValue.Key, "");
      FullTallyName = keyValue.Key;
      Sql = keyValue.Value;

      isRecordUpdated = false;
      MissedTalliesView.SaveButton.IsEnabled = isRecordUpdated;
      MissedTalliesView.UndoButton.IsEnabled = isRecordUpdated;

    }

    internal void RecordIsUpdated()
    {

      isRecordUpdated = true;
      MissedTalliesView.SaveButton.IsEnabled = isRecordUpdated;
      MissedTalliesView.UndoButton.IsEnabled = isRecordUpdated;

    }

  }
}
