using System;

namespace Banking.Models
{
	class ABNImport
	{
		public DateTime Date { get; set; }
		public string Name { get; set; }
		public string Account { get; set; }
		public string CounterAccount { get; set; }
		public decimal Amount { get; set; }
		public string Mutation { get; set; }
		public string Text { get; set; }
	}
}
