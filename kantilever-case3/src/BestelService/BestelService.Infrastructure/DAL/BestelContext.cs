using BestelService.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace BestelService.Infrastructure.DAL
{
    public class BestelContext : DbContext
    {
        public BestelContext(DbContextOptions<BestelContext> options) : base(options) { }

        public DbSet<Bestelling> Bestellingen { get; set; }
        public DbSet<Klant> Klanten { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Klant>()
                .OwnsOne(e => e.Factuuradres);

            modelBuilder.Entity<Bestelling>()
                .HasOne(e => e.Klant)
                .WithMany(e => e.Bestellingen);

            modelBuilder
                .Entity<Bestelling>()
                .OwnsOne(p => p.AfleverAdres);
        }
    }
}
