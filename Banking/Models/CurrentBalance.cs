using Newtonsoft.Json;

using System;

namespace Banking.Models
{
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
