using Banking.Models;
using Banking.Views;

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

namespace Banking.ViewModels
{
	public class TalliesRulesViewModel : INotifyPropertyChanged
	{

		#region [ Fields ]

		Window Parent;
		TalliesRulesWindow RulesView;
		MainViewModel MainVM;

		private string tallyName;
		private string fullTallyName;
		private string sql;
		private bool isRecordUpdated = false;

		#endregion

		#region [ Properties ]

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
				}
			}
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
				}
			}
		}
		public bool IsRecordUpdated
		{
			get => isRecordUpdated;
			set
			{
				if (isRecordUpdated != value)
				{
					isRecordUpdated = value;
					NotifyPropertyChanged("IsRecordUpdated");
				}
			}
		}
		public Bank SelectedAccount { get; set; }

		#region NotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged(string propertyName = "")
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		#endregion

		#endregion

		#region [ Constructor ] 
		public TalliesRulesViewModel(Window view, MainViewModel mainVM)
		{

			Parent = view;
			MainVM = mainVM;

			if (MainVM.TalliesRules.Count == 0)
			{
				MainVM.ReadTalliesRules();
			}

		}
		#endregion

		public bool? ShowTalliesRules()
		{

			RulesView = new TalliesRulesWindow(this)
			{
				Left = Parent.Left + 20,
				Top = Parent.Top + 20
			};

			if (SelectedAccount is null)
			{
				RulesView.TalliesRulesListBox.ItemsSource = MainVM.TalliesRules;
				RulesView.TalliesRulesListBox.SelectedItem = MainVM.TalliesRules.FirstOrDefault();
			}
			else
			{
				RulesView.TalliesRulesListBox.ItemsSource = MainVM
					.TalliesRules.Where(x => x.Key.StartsWith(SelectedAccount.TallyName));
				RulesView.TalliesRulesListBox.SelectedItem = MainVM.TalliesRules.LastOrDefault();
			}

			RulesView.Owner = Parent;
			return RulesView.ShowDialog();

		}

		public void SelectItem(KeyValuePair<string, string> keyValue)
		{

			string Pattern = @"\s+\d{3}$";
			Regex check = new Regex(Pattern);

			TallyName = check.Replace(keyValue.Key, "");
			FullTallyName = keyValue.Key;
			Sql = keyValue.Value;

			IsRecordUpdated = false;
			RulesView.SaveButton.IsEnabled = IsRecordUpdated;
			RulesView.UndoButton.IsEnabled = IsRecordUpdated;

		}

		internal void RecordIsUpdated()
		{
			IsRecordUpdated = true;
			RulesView.FullTallyNameTextBox.IsEnabled = true;
			RulesView.SQLTextBox.IsEnabled = true;
		}

		internal void CloseWindow()
		{
			RulesView.DialogResult = false;
		}

		internal void AddTallyItem()
		{
			//Unique key (FullTallyName) => TallyName + number
			string lastKey = MainVM.TalliesRules
				.Where(x => x.Key.StartsWith(SelectedAccount.TallyName))
				.LastOrDefault().Key;
			string lastTallyNumber = lastKey.Substring(lastKey.Length - 3, 3);
			if (int.TryParse(lastTallyNumber, out int newNumber))
			{
				newNumber += 10;
			}
			FullTallyName = $"{SelectedAccount.TallyName} {newNumber:000}";

			//SQL
			Sql = "\r\nUPDATE Bank";
			if (SelectedAccount.Account == "NL54ABNA0574446974")
			{
				Sql = $"{Sql}\r\nSET [Origin] = 'Gezamenlijk',";
			}
			else
			{
				Sql = $"{Sql}\r\nSET [Origin] = 'Persoonlijk',";
			}
			Sql = $"{Sql}\r\n\t[TallyName] = '{SelectedAccount.TallyName}'";

			Sql = $"{Sql}\r\nWHERE [TallyName] IS NULL";
			Sql = $"{Sql}\r\n\tAND [Account] = '{SelectedAccount.Account}'";
			Sql = $"{Sql}\r\n\tAND [Mutation] = '{SelectedAccount.Mutation}'";
			Sql = $"{Sql}\r\n\tAND [Name] = '{SelectedAccount.Name}'";
			Sql = $"{Sql}\r\n\tAND [CounterAccount] = '{SelectedAccount.CounterAccount}'";
			Sql = $"{Sql}\r\n\tAND [Text] LIKE '%{SelectedAccount.Text}%'";
		}

		internal bool CanSaveTallyItem()
		{
			return IsRecordUpdated;
		}

		internal void SaveTallyItem()
		{
			MainVM.TalliesRules.Add(FullTallyName, Sql);
			RulesView.TalliesRulesListBox.ItemsSource = MainVM.TalliesRules
				.Where(x => x.Key.StartsWith(SelectedAccount.TallyName));
			RulesView.TalliesRulesListBox.SelectedItem = MainVM.TalliesRules.LastOrDefault();

			MainVM.TalliesRulesChanged = true;
		}

	}
}
