using System.Data.Entity;

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
