using Banking.Exceptions;
using Banking.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace Banking.ModelViews
{
	public class ImportINGModelView
	{

		private List<Import> Cache = new List<Import>();

		public ImportINGModelView(string fileName, OptionModelView options)
		{
			if (ImportFile(fileName))
			{
				try
				{
					using (BankingDbContext db = new BankingDbContext(options.DbConnection))
					{
						foreach (Import record in Cache)
						{
							db.Imports.Add(record);
							db.SaveChanges();
						}
					}

					MessageBox.Show($"{Cache.Count} records are imported.",
						"Import ING",
						MessageBoxButton.OK,
						MessageBoxImage.Information);
				}
				catch
				{
					throw new ImportException("Import ING file has failed");
				}
			}
		}

		private bool ImportFile(string fileName)
		{

			bool result = true;
			if (!File.Exists(fileName))
			{
				result = false;
				throw new ImportFileException(fileName);
			}

			try
			{
				string Line = string.Empty;
				int Count = 0;

				using (StreamReader sr = new StreamReader(fileName))
				{
					while (sr.Peek() >= 0)
					{
						Line = sr.ReadLine().Replace("\",\"", "|")
							.Replace("\"", "");
						Count++;
						//Skip the header first line
						if (Count > 1)
						{
							ProcessLine(Line);
						}
					}
				}
			}
			catch (Exception ex)
			{
				result = false;
				throw new ImportStreamException(ex.Message);
			}

			return result;

		}

		private void ProcessLine(string line)
		{

			bool DivideBy100 = (decimal.Parse("1.25") == 125);

			DateTime Date;
			DateTime importDate = DateTime.Now;
			decimal Amount;

			if (string.IsNullOrEmpty(line))
			{
				return;
			}

			//Split line on pipeline character (comma was disrupting as decimal separator)
			string[] Record = line.Split('|');

			if (Record.Count() != 9)
			{
				throw new ImportFileHeaderException("ING csv file", 9, Record.Count());
			}

			//Date [0]
			try
			{
				Date = new DateTime(Int32.Parse(Record[0].Substring(0, 4)),
					Int32.Parse(Record[0].Substring(4, 2)),
					Int32.Parse(Record[0].Substring(6, 2))
					);
			}
			catch
			{
				throw new ImportDateException(Record[0]);
			}

			//Name [1]

			//Account [2]

			//CounterAccount [3]

			//Code [4]

			//Direction [5]

			//Amount [6]
			Amount = decimal.Parse(Record[6].Replace(',', '.'));
			if (Record[5] == "Af") { Amount = -Amount; }
			if (DivideBy100) { Amount /= 100; } //Correction because CultureInfo

			//Mutation [7]
			//Text [8]

			Cache.Add(new Import
			{
				ImportDate = importDate,
				Date = Date,
				Name = Record[1],
				Account = Record[2],
				CounterAccount = Record[3],
				Amount = Amount,
				Mutation = Record[7],
				Text = Record[8],
				RawText = Record[8]
			});
		}

	}
}
