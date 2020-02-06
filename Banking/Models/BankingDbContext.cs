using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Models
{
  public class BankingDbContext : DbContext
  {
    public BankingDbContext(string dbConnection) : base(dbConnection) { }

    public DbSet<Bank> Accounts { get; set; }

    public DbSet<Import> Imports { get; set; }

		public DbSet<OVCard> OVCards { get; set; }

		internal static void SeedData(BankingDbContext context)
    {
      context.Database.CreateIfNotExists();
    }
  }
}
