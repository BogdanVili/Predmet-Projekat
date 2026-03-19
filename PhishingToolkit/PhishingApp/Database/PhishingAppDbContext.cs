using PhishingApp.Model;
using System.Data.Entity;

namespace PhishingApp.Database
{
    public class PhishingAppDbContext : DbContext
    {
        public DbSet<Victim> Victims { get; set; }

        public DbSet<Website> Websites { get; set; }

        public PhishingAppDbContext() : base("PhishingAppDbContext")
        {

        }
    }
}
