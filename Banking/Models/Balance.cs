using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Models
{
	public class Balance : INotifyPropertyChanged
	{
		private string name;
		 
		public string Name
		{
			get => name;
			set
			{
				if (name != value)
				{
					name = value;
					NotifyPropertyChanged("Name");
				}
			}
		}

		[JsonIgnore]
		public decimal Difference { get; private set; }

		public ObservableCollection<BalanceAmount> Amounts { get; set; } =
			new ObservableCollection<BalanceAmount>();

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

	}

	public class BalanceAmount : INotifyPropertyChanged
	{
		private DateTime date;
		private decimal amount;

		public DateTime Date
		{
			get => date;
			set
			{
				if (date != value)
				{
					date = value;
					NotifyPropertyChanged("Date");
				}
			}
		}

		public decimal Amount
		{
			get => amount;
			set
			{
				if (amount != value)
				{
					amount = value;
					NotifyPropertyChanged("Amount");
				}
			}
		}

		[JsonIgnore]
		public decimal? Difference { get; set; }

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

	}

	public class CurrentBalance
	{

		[JsonIgnore]
		public string Name { get; set; }
		[JsonIgnore]
		public DateTime Date { get; set; }
		[JsonIgnore]
		public decimal Amount { get; set; }
		[JsonIgnore]
		public decimal? Diffence { get; set; }

	}

}
