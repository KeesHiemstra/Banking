﻿using CHi.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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
      if (!ServiceExtensions.IsStarted("MSSQLServer", true))
      {
        Application.Current.Shutdown();
      }
    }
  }
}
