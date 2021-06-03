using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banking.Models
{
	[Table("OVTravel")]
	public class OVTravel
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string Direction { get; set; }

		public string Address { get; set; }

		public string Purpose { get; set; }

		public virtual List<OVCard> OVCards { get; set; }
	}
}
