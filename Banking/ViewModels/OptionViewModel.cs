using Banking.Views;
using System.IO;
using Newtonsoft.Json;
using System.ComponentModel;
using CHi.Extensions;

namespace Banking.ViewModels
{
	public class OptionViewModel : INotifyPropertyChanged
	{

		#region [ Fields ]

		private string dbConnection = @"Database=Banking_Dev;Data Source=(Local);Trusted_Connection=True;MultipleActiveResultSets=true";
		private string importBankPath = @"C:\";
		private string importOVPath = @"C:\";
		private string backupPath = @"C:\";
		private string defaultCardNumber = string.Empty;

		#endregion

		#region [ Properties ]

		public string DbConnection
		{
			get => dbConnection;
			set
			{
				if (value != dbConnection)
				{
					dbConnection = value;
					NotifyPropertyChanged();
				}
			}
		}

		public string ImportBankPath
		{
			get => importBankPath;
			set
			{
				if (value != importBankPath)
				{
					importBankPath = value.SavePath();
					NotifyPropertyChanged();
				}
			}
		}

		public string ImportOVPath
		{
			get => importOVPath;
			set
			{
				if (value != importOVPath)
				{
					importOVPath = value.SavePath();
					NotifyPropertyChanged();
				}
			}
		}

		public string BackupPath
		{
			get => backupPath;
			set
			{
				if (value != backupPath)
				{
					backupPath = value.SavePath();
					NotifyPropertyChanged();
				}
			}
		}

		public string DefaultCardNumber
		{
			get => defaultCardNumber;
			set
			{
				if (value != defaultCardNumber)
				{
					defaultCardNumber = value;
					NotifyPropertyChanged();
				}
			}
		}

		#endregion

		#region [ Fields ]

		[JsonIgnore]
		public string DbName
		{
			get
			{
				string result = string.Empty;
				string[] parts = DbConnection.Split(';');
				for (int i = 0; i < parts.Length; i++)
				{
					if (parts[i].ToLower().StartsWith("database"))
					{
						string[] option = parts[i].Split('=');
						result = option[1];
					}
				}
				return result;
			}
		}
		[JsonIgnore]
		public string JsonPath => 
			System.Reflection.Assembly.GetEntryAssembly().Location.Replace(".exe", ".json");

		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion

		#region [ Methods ]

		public void ShowOptions(MainWindow main)
		{
			OptionWindow view = new OptionWindow(this)
			{
				Top = main.Top + 30,
				Left = main.Left + 50
			};

			bool? Result = view.ShowDialog();
			if ((bool)Result)
			{
				string json = JsonConvert.SerializeObject(this, Formatting.Indented);
				using StreamWriter stream = new StreamWriter(JsonPath);
				stream.Write(json);
			}
		}

		#endregion

	}
}
