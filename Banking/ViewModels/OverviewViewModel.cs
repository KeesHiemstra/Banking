using Banking.Views;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;

namespace Banking.ViewModels
{
	public class OverviewViewModel : INotifyPropertyChanged
	{
		private MainViewModel MainVM;
		private OverviewWindow View;

		private List<string> Tallies { get; set; } = new List<string>();
		private List<(string Month, string Tally, decimal SumAmount)> Pivot { get; set; } = 
			new List<(string Month, string Tally, decimal SumAmount)>();
		private DataView PivotView { get; set; }
		
		public List<string> Overviews { get; } = new List<string>
			{
				"Gezamelijk af",
				"Persoonlijk af",
				"Onverwacht",
				"Inkomen",
				"Alles"
			};
		public DataSet Data { get; set; } = new DataSet("Data");
		
		#region INotifyPropertyChanged
		public event PropertyChangedEventHandler PropertyChanged;
		private void NotifyPropertyChanged(string propertyName = "")
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
		#endregion

		#region ctor
		//public OverviewModelView() { }

		public OverviewViewModel(OverviewWindow view, MainViewModel mainViewModel)
		{

			MainVM = mainViewModel;
			if (MainVM.HasMissedTallies) { return; }

			View = view;

		}
		#endregion

		public void FilterAccountList(string TallyName, string Month)
		{

			_ = new BankViewModel(MainVM.Options, View, TallyName, Month);

		}

		public void SelectOverview(OverviewItem item)
		{
			Tallies.Clear();
			
			switch (item)
			{
				case OverviewItem.CommonlyExpenses:
					Tallies = GetTallies(false, "Gezamenlijk");
					Pivot = GetPivot(false, "Gezamenlijk");
					break;
				case OverviewItem.PersonalExpenses:
					Tallies = GetTallies(false, "Persoonlijk");
					Pivot = GetPivot(false, "Persoonlijk");
					break;
				case OverviewItem.VariableExpenses:
					Tallies = MainVM.Accounts
						.Where(x => x.Origin is null)
						.Select(x => x.TallyName)
						.OrderBy(x => x)
						.Distinct()
						.ToList();
					Tallies = GetTallies(false, "Onverwacht");
					Pivot = GetPivot(false, "Onverwacht");
					break;
				case OverviewItem.Incomes:
					Tallies = GetTallies(true, "Inkomen gezamenlijk");
					Pivot = GetPivot(true, "Inkomen gezamenlijk");
					break;
				case OverviewItem.All:
					Tallies = MainVM.Accounts
						.Select(x => x.TallyName)
						.OrderBy(x => x)
						.Distinct()
						.ToList();
					Pivot = GetPivot(true, "Alles");
					break;
				default:
					break;
			}

			CreatePivotTable();
			FillPivotTable();

			View.PivotDataGrid.ItemsSource = null;
			PivotView = new DataView(Data.Tables["Pivot"]);
			View.PivotDataGrid.ItemsSource = PivotView;

		}

		public void ExportPivot()
		{

			string export = string.Empty;
			foreach (var item in Data.Tables[0].Columns)
			{
				export += $"{item.ToString()}\t";
			}
			export += "\n";

			foreach (DataRow row in Data.Tables[0].Rows)
			{
				foreach (var item in row.ItemArray)
				{
					export += $"{item.ToString()}\t";
				}
				export += "\n";
			}

			Clipboard.SetText(export);

		}

		/// <summary>
		/// (re)Collect the tallies according the requests.
		/// </summary>
		/// <param name="isPositiveAmounts"></param>
		/// <param name="originName"></param>
		/// <returns></returns>
		private List<string> GetTallies(bool isPositiveAmounts, string originName)
		{
			return MainVM.Accounts
				.Where(x => originName == "Alles" ? (x.Amount != 0) :
					(isPositiveAmounts ? (x.Amount > 0) :
						(x.Amount < 0 && (originName == "Onverwacht" ? x.Origin is null : x.Origin == originName))))
						.Select(x => x.TallyName)
				.OrderBy(x => x)
				.Distinct()
				.ToList();
		}

		/// <summary>
		/// (re)Collect the pivot according the requests.
		/// </summary>
		/// <param name="isPositiveAmounts"></param>
		/// <param name="originName"></param>
		/// <returns></returns>
		private List<(string Month, string Tally, decimal SumAmount)> GetPivot(bool isPositiveAmounts, string originName)
		{

			List<(string Month, string Tally, decimal SumAmount)> result = MainVM.Accounts
				.Where(x => originName == "Alles" ? (x.Amount != 0) :
					(isPositiveAmounts ? (x.Amount > 0) :
						(x.Amount < 0 && (originName == "Onverwacht" ? x.Origin is null : x.Origin == originName))))
						.OrderByDescending(x => (x.Date, x.TallyName))
				.GroupBy(x => (x.Month, x.TallyName),
				(key, pivot) =>
				{
					return (Month: key.Month,
					TallyName: key.TallyName,
					SumAmount: pivot.Sum(x => x.Amount)
					);
				}).ToList();

			return result;

		}

		/// <summary>
		/// (re)Create the PivotTable.
		/// </summary>
		private void CreatePivotTable()
		{

			if (Data.Tables.Count > 0) { Data.Tables.Clear(); };

			DataTable pivot = new DataTable("Pivot");
			DataColumn column = pivot.Columns.Add("Month", typeof(string));
			pivot.Columns[0].AllowDBNull = false;

			int count = 0;
			//Add the header
			foreach (string col in Tallies)
			{
				pivot.Columns.Add(col, typeof(string));
				count++;
				pivot.Columns[count].AllowDBNull = true;
			};

			Data.Tables.Add(pivot);

		}

		/// <summary>
		/// Fill the Pivot to PivotTable.
		/// </summary>
		private void FillPivotTable()
		{

			DataRow row = null;
			string month = string.Empty;
			for (int i = 0; i < Pivot.Count - 1; i++)
			{
				if (month != Pivot[i].Month)
				{
					if (!string.IsNullOrEmpty(month) || row != null)
					{
						Data.Tables[0].Rows.Add(row);
					}
					row = Data.Tables[0].NewRow();
					month = Pivot[i].Month;
					row["Month"] = month;
				}

				row[Pivot[i].Tally] = Pivot[i].SumAmount.ToString("0.00");
			}

			Data.Tables[0].Rows.Add(row);

		}

		public enum OverviewItem
		{
			CommonlyExpenses,
			PersonalExpenses,
			VariableExpenses,
			Incomes,
			All
		}
		
	}
}
