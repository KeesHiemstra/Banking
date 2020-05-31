using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Banking.Commands
{
  public static class TalliesRulesWindowCommands
  {
    public static readonly RoutedUICommand Close = new RoutedUICommand
      (
        "_Close",
        "Close",
        typeof(TalliesRulesWindowCommands),
        new InputGestureCollection() { }
      );

    public static readonly RoutedUICommand Add = new RoutedUICommand
      (
        "_Add",
        "Add",
        typeof(TalliesRulesWindowCommands),
        new InputGestureCollection() { }
      );

    public static readonly RoutedUICommand Save = new RoutedUICommand
      (
        "_Save",
        "Save",
        typeof(TalliesRulesWindowCommands),
        new InputGestureCollection() { }
      );
  }
}
