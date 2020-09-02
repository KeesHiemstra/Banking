using System;
using System.IO;
using System.Reflection;

namespace CHi.Log
{
	public static class Log
  {

    #region [ Fields ]

    private static string LoggingFile;
    private static bool IsLogged = false;

    #endregion

    /// <summary>
    /// Write single line 'message' to the log file followed to the date/time.
    /// </summary>
    /// <param name="message"></param>
    public static void Write(string message)
    {
      if (string.IsNullOrEmpty(LoggingFile))
      {
				if (!IsLogged)
				{
          string location = Assembly.GetExecutingAssembly().Location;
          string name = Assembly.GetExecutingAssembly().GetName().Name;
          string lf = SetLoggingFile($".\\{name}.log");
          string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
#if DEBUG
          version += " (debug)";
#else
          version += " (release)";
#endif
          if (File.Exists(lf))
          {
            using StreamWriter stream = new StreamWriter(LoggingFile, true);
            stream.WriteLine();
          }
          Write($"{location} is started");
          Write($"{name} version {version}");
          Write($"Logging '{name}' started using '{lf}'");
        }
      }
      IsLogged = true;
			if (!string.IsNullOrEmpty(message))
			{
        string Message = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} {message}";
        using (StreamWriter stream = new StreamWriter(LoggingFile, true))
        {
          stream.WriteLine(Message);
        }
      }
    }

    /// <summary>
    /// Write multi lines messages to the log file, but only first line followed to the date/time.
    /// </summary>
    /// <param name="messages"></param>
    public static void Write(params string[] messages)
    {
			if (messages.Length == 0) 
      {
        Write("");
        return;	
      }
      Write(messages[0]);

			using StreamWriter stream = new StreamWriter(LoggingFile, true);
			for (int LineNo = 1; LineNo < messages.Length; LineNo++)
			{
				stream.WriteLine($"                    {messages[LineNo]}");
			}
		}

    /// <summary>
    /// Set the LoggingFile for the log file (other than the default).
    /// </summary>
    /// <param name="loggingFile"></param>
    /// <returns></returns>
    public static string SetLoggingFile(string loggingFile)
    {
      LoggingFile = loggingFile;
      return loggingFile;
    }

  }
}
