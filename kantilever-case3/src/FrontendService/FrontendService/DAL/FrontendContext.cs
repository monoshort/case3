using System.Diagnostics.CodeAnalysis;
using FrontendService.Models;
using Microsoft.EntityFrameworkCore;

namespace FrontendService.DAL
{
    [ExcludeFromCodeCoverage]
    public class FrontendContext : DbContext
    {
        public FrontendContext(DbContextOptions<FrontendContext> options) : base(options) { }

        public DbSet<Artikel> Artikelen { get; set; }
        public DbSet<Klant> Klanten{ get; set; }
        public DbSet<Bestelling> Bestellingen { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Klant>()
                .OwnsOne(e => e.Factuuradres);

            modelBuilder.Entity<Bestelling>()
                .OwnsOne(e => e.AfleverAdres);
        }
    }
}
