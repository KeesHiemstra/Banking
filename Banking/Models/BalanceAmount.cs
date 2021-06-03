using Newtonsoft.Json;

using System;
using System.ComponentModel;

namespace Banking.Models
{
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

}
