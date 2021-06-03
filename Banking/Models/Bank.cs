using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banking.Models
{
	[Table("Bank")]
	public class Bank
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string Account { get; set; }

		[Required]
		[DataType(DataType.Date)]
		public System.DateTime Date { get; set; }

		[Required]
		public string Mutation { get; set; }

		[Required]
		public decimal Amount { get; set; }

		public string Name { get; set; }

		public string CounterAccount { get; set; }

		public string Text { get; set; }

		public string Month { get; set; }

		public string Origin { get; set; }

		public string TallyName { get; set; }

		public string TallyDescription { get; set; }

		public string RawText { get; set; }
	}
}
