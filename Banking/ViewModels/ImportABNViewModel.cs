using Banking.Exceptions;
using Banking.Models;
using CHi.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace Banking.ViewModels
{
	public class ImportABNViewModel
	{
		private List<Import> Cache = new List<Import>();

		public ImportABNViewModel(string fileName, OptionViewModel options)
		{
			Log.Write("Starting ABN import");
			ProcessImportABNModelView(fileName, options);

		}

		private void ProcessImportABNModelView(string fileName, OptionViewModel options)
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

					Log.Write($"{Cache.Count} records are imported from ABN file");
					MessageBox.Show($"{Cache.Count} records are imported.",
						"Import ABN",
						MessageBoxButton.OK,
						MessageBoxImage.Information);
				}
				catch (Exception ex)
				{
					Log.Write($"Import ABN file has failed after record {Cache.Count}", ex.Message, ex.InnerException.ToString());
					throw new ImportException("Import ABN file has failed");
				}
			}

		}

		private bool ImportFile(string fileName)
		{

			bool result = true;
			if (!File.Exists(fileName))
			{
				result = false;
				Log.Write($"Error in {fileName}");
				throw new ImportFileException(fileName);
			}

			int Count = 0;
			try
			{
				string Line = string.Empty;

				using (StreamReader sr = new StreamReader(fileName))
				{
					while (sr.Peek() >= 0)
					{
						Line = sr.ReadLine();
						Count++;
						if (Count > 0)
						{
							ProcessLine(Line);
						}
					}
				}
			}
			catch (IndexOutOfRangeException ex)
			{
				Log.Write($"Record {Count} caused index error\n{ex.Message}");
				MessageBox.Show($"Record {Count} caused index error\n{ex.Message}");
			}
			catch (Exception ex)
			{
				result = false;
				Log.Write($"Error: {ex.Message}");
				MessageBox.Show(ex.Message);
				throw new ImportStreamException(ex.Message);
			}

			return result;

		}

		private void ProcessLine(string line)
		{

			bool DivideBy100 = (decimal.Parse("1.25") == 125);

			DateTime Date;
			DateTime importDate = DateTime.Now;
			string Name;
			string CounterAccount;
			decimal Amount;
			string Mutation;

			if (string.IsNullOrEmpty(line))
			{
				return;
			}

			string[] Record = line.Split('\t');

			if (Record.Count() != 8)
			{
				Log.Write($"The number of field doesn't match (8), there are {Record.Count()} fields");
				throw new ImportFileHeaderException("ABN tab file", 8, Record.Count());
			}

			//Account [0]
			if (Record[0] == "574446974")
			{
				Record[0] = "NL54ABNA0574446974";
			}

			//Currency [1] (not used)

			//Date [2]
			try
			{
				Date = new DateTime(Int32.Parse(Record[2].Substring(0, 4)),
					Int32.Parse(Record[2].Substring(4, 2)),
					Int32.Parse(Record[2].Substring(6, 2))
					);
			}
			catch (Exception ex)
			{
				Log.Write($"Error record(2) {Record[2]}: {ex.Message}");
				throw new ImportDateException(Record[2]);
			}

			//Start balance [3] (not used)

			//End balance [4] (not used)

			//Interest date [5] (not used)

			//Amount [6]
			Amount = decimal.Parse(Record[6].Replace(',', '.'));
			if (DivideBy100) { Amount /= 100; } //Correction because CultureInfo

			//Text [7]
			Name = string.Empty;
			CounterAccount = string.Empty;
			Mutation = string.Empty;

			Import record = new Import
			{
				ImportDate = importDate,
				Date = Date,
				Name = Name,
				Account = Record[0],
				CounterAccount = CounterAccount,
				Amount = Amount,
				Mutation = Mutation,
				Text = Record[7],
				RawText = Record[7]
			};

			ProcessRecord(record);

			Cache.Add(record);

			if (record.Name.Length > 45)
			{
				Log.Write($"Error in record {Cache.Count}. Field 'Name' is too long ({record.Name.Length})");
			}
			else if (record.Account.Length > 18)
			{
				Log.Write($"Error in record {Cache.Count}. Field 'Account' is too long ({record.Account.Length})");
			}
			else if (record.CounterAccount.Length > 30)
			{
				Log.Write($"Error in record {Cache.Count}. Field 'CounterAccount' is too long ({record.CounterAccount.Length})");
			}
			else if (record.Mutation.Length > 25)
			{
				Log.Write($"Error in record {Cache.Count}. Field 'Mutation' is too long ({record.Mutation.Length})");
			}
			else if (record.Text.Length > 350)
			{
				Log.Write($"Error in record {Cache.Count}. Field 'Text' is too long ({record.Text.Length})");
			}
			else if (record.RawText.Length > 350)
			{
				Log.Write($"Error in record {Cache.Count}. Field 'RawText' is too long ({record.RawText.Length})");
			}

		}

		private void ProcessRecord(Import record)
		{

			string Search;
			int Count;
			int Start;

			if (record.Text.StartsWith("ABN AMRO Bank N.V."))
			{
				record.Mutation = "Diverse";
			}
			else if (record.Text.StartsWith("BEA   "))
			{
				record.Mutation = "Betaalautomaat";
				record.Text = record.Text.Replace("BEA   ", "");
			}
			else if (record.Text.StartsWith("GEA   "))
			{
				record.Mutation = "Geldautomaat";
				record.Text = record.Text.Replace("GEA   ", "");
			}
			else if (record.Text.StartsWith("SEPA"))
			{
				if (record.Text.StartsWith("SEPA Overboeking"))
				{
					record.Mutation = "Online bankieren";

					Search = "SEPA Overboeking";
					Count = Search.Length + 1;
					while (record.Text[Count] == ' ') { Count++; }
					Search = record.Text.Substring(0, Count);
					record.Text = record.Text.Replace(Search, "");
				}
				else if (record.Text.StartsWith("SEPA iDEAL"))
				{
					record.Mutation = "Online bankieren";

					Search = "SEPA iDEAL";
					Count = Search.Length + 1;
					while (record.Text[Count] == ' ') { Count++; }
					Search = record.Text.Substring(0, Count);
					record.Text = record.Text.Replace(Search, "");
				}
				else if (record.Text.StartsWith("SEPA Incasso algemeen doorlopend"))
				{
					record.Mutation = "Incasso";

					Search = "SEPA Incasso algemeen doorlopend";
					Count = Search.Length + 1;
					while (record.Text[Count] == ' ') { Count++; }
					Search = record.Text.Substring(0, Count);
					record.Text = record.Text.Replace(Search, "");

					if (record.Text.StartsWith("Incassant:"))
					{
						Search = "Incassant: ";
						Count = Search.Length + 1;
						while (record.Text[Count] != ' ') { Count++; }
						Search = record.Text.Substring(0, Count);
						record.Text = record.Text.Replace(Search, "") + "  " + Search;
					}
				}

				if (record.Text.Contains("IBAN: "))
				{
					Start = record.Text.IndexOf("IBAN: ");
					Count = Start + 7;

					while (Count < record.Text.Length - 1
						&& !(record.Text[Count] == ' '
						&& record.Text[Count + 1] == ' '))
					{
						Count++;
					}

					while (Count < record.Text.Length - 1
						&& record.Text[Count] == ' '
						&& record.Text[Count + 1] == ' ')
					{
						Count++;
					}

					Search = record.Text.Substring(Start, Count - Start);
					record.CounterAccount = Search.Replace("IBAN: ", "").Trim();

					record.Text = record.Text.Replace(Search, "");
				}

				if (record.Text.Contains("Naam: "))
				{
					Start = record.Text.IndexOf("Naam: ");
					Count = Start + 7;

					while (Count < record.Text.Length - 1
						&& !(record.Text[Count] == ' '
						&& record.Text[Count + 1] == ' '))
					{
						Count++;
					}

					while (Count < record.Text.Length - 1
						&& record.Text[Count] == ' '
						&& record.Text[Count + 1] == ' ')
					{
						Count++;
					}

					if (record.Text.Substring(Start, Count - Start).Contains("Omschrijving: "))
					{
						Count = record.Text.IndexOf("Omschrijving: ");
					}

					Search = record.Text.Substring(Start, Count - Start);
					record.Name = Search.Replace("Naam: ", "").Trim();
					if (record.Name.Length > 27 && record.Name[26] == ' ')
					{
						record.Name = record.Name.Remove(26, 1);
					}
					record.Text = record.Text.Replace(Search, "").Trim();

					if (record.Text.Contains("Machtiging: "))
					{
						Start = record.Text.IndexOf("Machtiging: ");
						Count = Start + 13;

						while (Count < record.Text.Length - 1
							&& !(record.Text[Count] == ' '
							&& record.Text[Count + 1] == ' '))
						{
							Count++;
						}

						while (Count < record.Text.Length - 1
							&& record.Text[Count] == ' '
							&& record.Text[Count + 1] == ' ')
						{
							Count++;
						}

						Search = record.Text.Substring(Start, Count - Start);
						record.Text = record.Text.Replace(Search, "").Trim() + "  " + Search;
					}


					if (record.Text.StartsWith("BIC:"))
					{
						Search = "BIC: ";
						Count = Search.Length + 1;
						while (Count < record.Text.Length && record.Text[Count] != ' ') { Count++; }
						Search = record.Text.Substring(0, Count);
						record.Text = (record.Text.Replace(Search, "") + "  " + Search).TrimStart();
					}

					if (record.Text.StartsWith("Omschrijving: "))
					{
						record.Text = record.Text.Replace("Omschrijving: ", "").Trim();
					}

					while (record.Text.Contains("  "))
					{
						record.Text = record.Text.Replace("  ", " ");
					}
				}
			}
			else if (record.Text.StartsWith("/TRTP"))
			{
				record.Text = record.Text.Replace("/TRTP", "");

				if (record.Text.Contains("/IBAN/"))
				{
					Start = record.Text.IndexOf("/IBAN/");
					Count = Start + 7;
					while (record.Text[Count] != '/' && Count < record.Text.Length) { Count++; }
					Search = record.Text.Substring(Start, Count - Start);

					record.CounterAccount = Search.Replace("/IBAN/", "").Replace("/", "");
					record.Text = record.Text.Replace("/IBAN/" + record.CounterAccount, "");
				}

				if (record.Text.Contains("/NAME/"))
				{
					Start = record.Text.IndexOf("/NAME/");
					Count = Start + 7;
					while (record.Text[Count] != '/' && Count < record.Text.Length) { Count++; }
					Search = record.Text.Substring(Start, Count - Start);

					if (Search == "/NAME/DRES Vastg Won CV HOF (452")
					{
						Count = record.Text.LastIndexOf("/MARF/");
						if (Count == -1)
						{
							Count = record.Text.LastIndexOf("/REMI/");
						}
						Search = record.Text.Substring(Start, Count - Start);
					}

					record.Name = Search.Replace("/NAME/", "");
					record.Text = record.Text.Replace("/NAME/" + record.Name, "");
				}

				if (record.Text.StartsWith("/SEPA OVERBOEKING"))
				{
					record.Mutation = "Online bankieren";
					record.Text = record.Text.Replace("/SEPA OVERBOEKING", "");
				}
				if (record.Text.StartsWith("/iDEAL"))
				{
					record.Mutation = "Online bankieren";
					record.Text = record.Text.Replace("/iDEAL", "");
				}
				else if (record.Text.StartsWith("/SEPA Incasso algemeen doorlopend"))
				{
					record.Mutation = "Incasso";
					record.Text = record.Text.Replace("/SEPA Incasso algemeen doorlopend", "");
				}

				if (record.Text.Contains("/CSID/"))
				{
					Start = record.Text.IndexOf("/CSID/");
					Count = Start + 7;
					while (record.Text[Count] != '/' && Count < record.Text.Length) { Count++; }
					Search = record.Text.Substring(Start, Count - Start);

					record.Text = record.Text.Replace(Search, "");
					record.Text = record.Text + Search.Replace("/CSID/", " Machtiging: ");
				}

				if (record.Text.Contains("/MARF/"))
				{
					Start = record.Text.IndexOf("/MARF/");
					Count = record.Text.LastIndexOf("/REMI/");
					if (Count == -1)
					{
						Count = Start + 7;
						while (record.Text[Count] != '/' && Count < record.Text.Length) { Count++; }
					}
					Search = record.Text.Substring(Start, Count - Start);

					record.Text = record.Text.Replace(Search, "");
					record.Text = record.Text + Search.Replace("/MARF/", " Referrentie: ");
				}

				if (record.Text.Contains("/BIC/"))
				{
					Start = record.Text.IndexOf("/BIC/");
					Count = Start + 6;
					while (record.Text[Count] != '/' && Count < record.Text.Length) { Count++; }
					Search = record.Text.Substring(Start, Count - Start);

					record.Text = record.Text.Replace(Search, "");
					record.Text = record.Text + Search.Replace("/BIC/", " BIC: ");
				}

				if (record.Text.StartsWith("/REMI/"))
				{
					record.Text = record.Text.Replace("/REMI/", "");
				}

				while (record.Text.Contains("  "))
				{
					record.Text = record.Text.Replace("  ", " ");
				}

				record.Text = record.Text.Replace("/EREF/", " Kenmerk: ").Trim();
			}

			while (record.Text.Contains("  "))
			{
				record.Text = record.Text.Replace("  ", " ");
			}

		}//ProcessRecord
	}
}
