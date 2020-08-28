using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Banking.Models
{
	[DebuggerDisplay("{Date}")]
	[Table("OVCard")]
	public class OVCard
	{
		[Key]
		public int Id { get; set; }

		public int? TravelId { get; set; }
		public virtual OVTravel Travel { get; set; }

		[Required]
		public string CardNumber { get; set; }

		[Required]
		[DataType(DataType.Date)]
		public DateTime Date { get; set; }

		[DataType(DataType.DateTime)]
		public DateTime? CheckIn { get; set; }

		[Required]
		public string Departure { get; set; }

		[DataType(DataType.DateTime)]
		public DateTime? CheckOut { get; set; }

		[Required]
		public string Destination { get; set; }

		[Required]
		public decimal Amount { get; set; }

		[Required]
		public string Transaction { get; set; }
	}
}
