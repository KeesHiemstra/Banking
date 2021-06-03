using Newtonsoft.Json;

using System.Collections.ObjectModel;
using System.ComponentModel;

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
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		#endregion

	}

}
