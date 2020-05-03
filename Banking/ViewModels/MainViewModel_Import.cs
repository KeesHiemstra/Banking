using Banking.Models;
using CHi.Extensions;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Banking.ViewModels
{
	public partial class MainViewModel : INotifyPropertyChanged
	{
		private static readonly Dictionary<string, string> ImportFileFilters =
			new Dictionary<string, string>();

		private string SelectImportFilePath(string filter)
		{
			//ABN: *.tab
			//ING: *.csv
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = filter;
			openFileDialog.InitialDirectory = Options.ImportBankPath.TranslatePath();

			if (openFileDialog.ShowDialog() == true)
			{
				return openFileDialog.FileName;
			}

			return string.Empty;

		}

		#region Import ABN file
		public async Task ImportABNFileAsync()
		{
			string fileName = string.Empty;
			fileName = SelectImportFilePath(ImportFileFilters["ABN"]);
			if (String.IsNullOrEmpty(fileName))
			{
				return;
			}

			if (!File.Exists(fileName))
			{
				MessageBox.Show($"File '{fileName}' doesn't exists",
					"Error in ABN import file",
					MessageBoxButton.OK,
					MessageBoxImage.Exclamation);
				return;
			}

			try
			{
				_ = new ImportABNViewModel(fileName, Options);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"{ex}", "Error");
			}
			await GetImportSummaryAsync();
			NotifyPropertyChanged("Imports");

		}
		#endregion

		#region Import ING file
		public async Task ImportINGFileAsync()
		{
			string fileName = string.Empty;
			fileName = SelectImportFilePath(ImportFileFilters["ING"]);
			if (String.IsNullOrEmpty(fileName))
			{
				return;
			}

			if (!File.Exists(fileName))
			{
				MessageBox.Show($"File '{fileName}' doesn't exists",
					"Error in ING import file",
					MessageBoxButton.OK,
					MessageBoxImage.Exclamation);
				return;
			}

			_ = new ImportINGViewModel(fileName, Options);
			await GetImportSummaryAsync();
			NotifyPropertyChanged("Imports");

		}
		#endregion

		#region Process import table
		public async Task ProcessImportTableAsync()
		{

			ImportProcessViewModel import = new ImportProcessViewModel(Options, this);

			//Step 1: Import the data from Import table to Bank table.

			//Step 2: Update the Origin and TallyName, TallyDescription.

			GetSummaries();
			NotifyPropertyChanged();

		}
		#endregion

		#region Clear import table
		public void ClearImportTable()
		{
			using (BankingDbContext db = new BankingDbContext(Options.DbConnection))
			{
				MessageBoxResult result = MessageBox.Show("Are you sure to clear the import table?", "Clear import table", MessageBoxButton.YesNo, MessageBoxImage.Question);
				if (result != MessageBoxResult.Yes)
				{
					return;
				}

				db.Database.ExecuteSqlCommand("TRUNCATE TABLE Import");
				Thread.Sleep(1000);

				GetSummaries();
			}
		}
		#endregion

		#region Import OV Card file
		public void ImportOVCardFile()
		{
			string fileName = string.Empty;
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "OV-Card import file (*.csv)|*.csv";
			openFileDialog.InitialDirectory = Options.ImportOVPath.TranslatePath();

			if (openFileDialog.ShowDialog() == true)
			{
				fileName = openFileDialog.FileName;
			}

			if (String.IsNullOrEmpty(fileName))
			{
				return;
			}

			if (!File.Exists(fileName))
			{
				MessageBox.Show($"File '{fileName}' doesn't exists",
					"Error in OV-Card import file",
					MessageBoxButton.OK,
					MessageBoxImage.Exclamation);
				return;
			}

			_ = new ImportOVCardViewModel(fileName, Options);

		}
		#endregion

	}
}

/* SQL command
-- Restore Backup_Import table

INSERT INTO Import(I.[ImportDate], I.[Account], I.[Date], I.[Mutation], I.[Amount], I.[Name], I.[CounterAccount], I.[Text], I.[RawText])
SELECT I.[ImportDate], I.[Account], I.[Date], I.[Mutation], I.[Amount], I.[Name], I.[CounterAccount], I.[Text], I.[RawText]
FROM Backup_Import AS I 
*/
