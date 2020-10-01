using Banking.Exceptions;
using Banking.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace Banking.ViewModels
{
	public class ImportOVCardViewModel
	{
		private readonly string defaultCardNumber;
		private readonly List<OVCard> Cache = new List<OVCard>();

		public ImportOVCardViewModel(string fileName, OptionViewModel options)
		{
			int CountRecords = 0;
			defaultCardNumber = options.DefaultCardNumber;

			if (ImportFile(fileName))
			{
				try
				{
					using (BankingDbContext db = new BankingDbContext(options.DbConnection))
					{
						//Save the OV-Card data
						foreach (var record in Cache)
						{
							var check = db.OVCards
								.Where(x => (x.Date == record.Date && x.CheckOut == record.CheckOut && x.Departure == record.Departure))
								.FirstOrDefault();
							if (check is null)
							{
								CountRecords++;
								db.OVCards.Add(record);
								db.SaveChanges();
							}
						}
					}

					string message = "The record was already imported";
					if (CountRecords > 0)
					{
						message = $"{CountRecords} records imported";
					}
					MessageBox.Show(message, "Import OV-Card file", MessageBoxButton.OK, MessageBoxImage.Information);
				}
				catch (Exception ex)
				{
					MessageBox.Show($"{ex.Message}", "Import OV-Card file has failed", MessageBoxButton.OK, MessageBoxImage.Error);
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

				using StreamReader sr = new StreamReader(fileName);
				while (sr.Peek() >= 0)
				{
					//Line = sr.ReadLine().Replace("\",\"", "|").Replace("\"", "");
					Line = sr.ReadLine();
					Count++;
					//Skip the header first line
					if (Count > 1)
					{
						ProcessLine(Line);
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
			DateTime CheckDate;
			decimal Amount;
			bool IsDefaultCardNumber;

			if (string.IsNullOrEmpty(line))
			{
				return;
			}

			string[] Record = line.Split(';');

			switch (Record.Count())
			{
				case 10:
					IsDefaultCardNumber = true;
					break;
				case 12:
					IsDefaultCardNumber = false;
					break;
				default:
					throw new ImportFileHeaderException("OV-Card csv file", 12, Record.Count());
			}

			//Remove the quotes
			for (int i = 0; i < Record.Count(); i++)
			{
				Record[i] = Record[i].Substring(1, Record[i].Length - 2);
			}

			OVCard record = new OVCard();

			//Date [0]
			try
			{
				Date = new DateTime(Int32.Parse(Record[0].Substring(6, 4)),
					Int32.Parse(Record[0].Substring(3, 2)),
					Int32.Parse(Record[0].Substring(0, 2))
					);
				record.Date = Date;
			}
			catch
			{
				throw new ImportDateException(Record[0]);
			}

			//CheckIn [1]
			if (!string.IsNullOrEmpty(Record[1]))
			{
				try
				{
					CheckDate = new DateTime(Int32.Parse(Record[0].Substring(6, 4)),
						Int32.Parse(Record[0].Substring(3, 2)),
						Int32.Parse(Record[0].Substring(0, 2)),
						Int32.Parse(Record[1].Substring(0, 2)),
						Int32.Parse(Record[1].Substring(3, 2)),
						0
						);
					record.CheckIn = CheckDate;
				}
				catch
				{
					throw new ImportDateException($"{Record[0]} {Record[1]}");
				}
			}

			//Departure [2]
			record.Departure = Record[2];

			//CheckOut [3]
			if (!string.IsNullOrEmpty(Record[3]))
			{
				try
				{
					CheckDate = new DateTime(Int32.Parse(Record[0].Substring(6, 4)),
						Int32.Parse(Record[0].Substring(3, 2)),
						Int32.Parse(Record[0].Substring(0, 2)),
						Int32.Parse(Record[3].Substring(0, 2)),
						Int32.Parse(Record[3].Substring(3, 2)),
						0
						);
					record.CheckOut = CheckDate;
				}
				catch
				{
					throw new ImportDateException($"{Record[0]} {Record[3]}");
				}
			}

			//Destination [4]
			record.Destination = Record[4];

			//Amount [5]
			Amount = decimal.Parse(Record[5].Replace(',', '.'));
			if (DivideBy100) { Amount /= 100; } //Correction because CultureInfo
			record.Amount = Amount;

			//Transaction [6]
			record.Transaction = Record[6];

			//Class [7] SKIP

			//Product [8] SKIP

			//Remarks [9] SKIP

			//Name [10] Conditional SKIP

			//CardNumber [11] Conditional
			if (IsDefaultCardNumber)
			{
				record.CardNumber = defaultCardNumber;
			}
			else
			{
				record.CardNumber = Record[11];
			}

			Cache.Add(record);
		}
	}

}
