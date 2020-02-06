using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Models
{
  [Table("Import")]
  public class Import
  {
    [Key]
    public int Id { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    public System.DateTime ImportDate { get; set; }

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

    public string RawText { get; set; }
  }
}
