﻿using Banking.Models;
using Banking.Views;

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace Banking.ViewModels
{
	public class TalliesRulesViewModel : INotifyPropertyChanged
	{

		#region [ Fields ]

		private readonly Window Parent;
		private TalliesRulesWindow RulesView;
		private readonly MainViewModel MainVM;

		private string fullTallyName;
		private string orginalFullTallyName;
		private string sql;
		private string tallyName;
		private bool isUpdatedRule = false;
		private bool isNewRule = false;

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
		public bool IsUpdatedRule
		{
			get => isUpdatedRule;
			set
			{
				if (isUpdatedRule != value)
				{
					isUpdatedRule = value;
					NotifyPropertyChanged("IsUpdatedRule");
				}
			}
		}
		public bool IsNewRule
		{
			get => isNewRule;
			set
			{
				if (isNewRule != value)
				{
					isNewRule = value;
					NotifyPropertyChanged("IsNewRule");
				}
			}
		}
		public Bank SelectedAccount { get; set; }

		#region NotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged(string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
			orginalFullTallyName = FullTallyName;
			Sql = keyValue.Value;

			IsUpdatedRule = false;
		}

		internal void RecordIsUpdated()
		{
			IsUpdatedRule = true;
		}

		internal void CloseWindow()
		{
			RulesView.DialogResult = false;
		}

		internal void AddTallyItem()
		{
			IsNewRule = true;
			IsUpdatedRule = true;

			//Unique key (FullTallyName) => TallyName + number
			string lastKey = MainVM.TalliesRules
				.Where(x => x.Key.StartsWith(SelectedAccount.TallyName))
				.LastOrDefault().Key;
			if (string.IsNullOrEmpty(lastKey)) { lastKey = "000"; }
			string lastTallyNumber = lastKey.Substring(lastKey.Length - 3, 3);
			if (int.TryParse(lastTallyNumber, out int newNumber))
			{
				newNumber += 10;
			}
			FullTallyName = $"{SelectedAccount.TallyName} {newNumber:000}";

			//SQL
			Sql = "\r\nUPDATE Bank"; 
			Sql = $"{Sql}\r\nSET [Origin] = '{MainVM.AccountNames[SelectedAccount.Account]}',";
			Sql = $"{Sql}\r\n\t[TallyName] = '{FullTallyName}'";
			Sql = $"{Sql}\r\nWHERE [TallyName] IS NULL";
			Sql = $"{Sql}\r\n\tAND [Account] = '{SelectedAccount.Account}'";
			Sql = $"{Sql}\r\n\tAND [Mutation] = '{SelectedAccount.Mutation}'";
			Sql = $"{Sql}\r\n\tAND [Name] = '{SelectedAccount.Name}'";
			Sql = $"{Sql}\r\n\tAND [CounterAccount] = '{SelectedAccount.CounterAccount}'";
			Sql = $"{Sql}\r\n\tAND [Text] LIKE '%{SelectedAccount.Text}%'";
		}

		internal bool CanSaveTallyItem()
		{
			return IsUpdatedRule || IsNewRule;
		}

		internal void SaveTallyItem()
		{
			if (IsNewRule)
			{
				MainVM.TalliesRules.Add(FullTallyName, Sql);
				RulesView.TalliesRulesListBox.ItemsSource = MainVM.TalliesRules
					.Where(x => x.Key.StartsWith(SelectedAccount.TallyName));
				RulesView.TalliesRulesListBox.SelectedItem = MainVM.TalliesRules.LastOrDefault();

				IsNewRule = false;
			}
			else
			{
				if (FullTallyName != orginalFullTallyName)
				{
					MainVM.TalliesRules.Remove(orginalFullTallyName);
					MainVM.TalliesRules.Add(FullTallyName, Sql);
				}
				else
				{
					MainVM.TalliesRules[orginalFullTallyName] = Sql;
				}
			}

			IsUpdatedRule = false;

			MainVM.TalliesRulesChanged = true;
		}

	}
}
