using System.Diagnostics.CodeAnalysis;
using KlantService.Models;
using Microsoft.EntityFrameworkCore;

namespace KlantService.DAL
{
    [ExcludeFromCodeCoverage]
    public class KlantContext : DbContext
    {
        public KlantContext(DbContextOptions<KlantContext> options) : base(options) {}

        public DbSet<Klant> Klanten { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Klant>().HasIndex(k => k.Username).IsUnique();

            modelBuilder.Entity<Klant>()
                .OwnsOne(e => e.Factuuradres);
        }
    }
}
