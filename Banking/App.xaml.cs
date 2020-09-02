using CHi.Extensions;
using CHi.Log;

using System.Threading;
using System.Windows;

namespace Banking
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    public App()
    {
      Log.Write();
      if (!ServiceExtensions.IsStarted("MSSQLServer", true))
      {
        Log.Write("MSSQLServer is not started"); 
        Application.Current.Shutdown();
      }
    }
  }
}
