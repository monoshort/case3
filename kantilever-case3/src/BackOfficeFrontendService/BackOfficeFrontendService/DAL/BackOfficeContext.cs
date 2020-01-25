using System.Diagnostics.CodeAnalysis;
using BackOfficeFrontendService.Models;
using Microsoft.EntityFrameworkCore;

namespace BackOfficeFrontendService.DAL
{
    [ExcludeFromCodeCoverage]
    public class BackOfficeContext : DbContext
    {
        public BackOfficeContext(DbContextOptions<BackOfficeContext> contextOptions) : base(contextOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BestelRegel>()
                .HasOne(e => e.Voorraad)
                .WithMany(e => e.BestelRegels);

            modelBuilder.Entity<VoorraadMagazijn>()
                .HasKey(e => e.ArtikelNummer);

            modelBuilder.Entity<Bestelling>()
                .OwnsOne(e => e.AfleverAdres);

            modelBuilder.Entity<Klant>()
                .OwnsOne(e => e.Factuuradres);
        }

        public DbSet<VoorraadMagazijn> VoorraadMagazijn { get; set; }
        public DbSet<Bestelling> Bestellingen { get; set; }
        public DbSet<Klant> Klanten { get; set; }
    }
}
