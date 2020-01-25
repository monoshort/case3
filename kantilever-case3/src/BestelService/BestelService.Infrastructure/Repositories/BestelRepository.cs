using System.Collections.Generic;
using System.Linq;
using BestelService.Core.Models;
using BestelService.Core.Repositories;
using BestelService.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;

namespace BestelService.Infrastructure.Repositories
{
    public class BestelRepository : IBestelRepository
    {
        private readonly BestelContext _context;

        public BestelRepository(BestelContext bestelContext)
        {
            _context = bestelContext;
        }

        /// <inheritdoc/>
        public void Add(Bestelling bestelling)
        {
            _context.Bestellingen.Add(bestelling);
            _context.SaveChanges();
        }

        /// <inheritdoc/>
        public void Update(Bestelling bestelling)
        {
            _context.Entry(bestelling).CurrentValues.SetValues(bestelling);
            _context.SaveChanges();
        }

        /// <inheritdoc/>
        public Bestelling GetById(long id)
        {
            return _context.Bestellingen
                .Include(b => b.AfleverAdres)
                .Include(b => b.BestelRegels)
                .Include(b => b.Klant.Bestellingen)
                .Single(b => b.Id == id);
        }

        /// <inheritdoc/>
        public Bestelling GetByBestellingNummer(string bestellingNummer)
        {
            return _context.Bestellingen
                .Include(b => b.BestelRegels)
                .Single(b => b.BestellingNummer == bestellingNummer);
        }

        /// <inheritdoc/>
        public Bestelling GetMostRecentUnassessedBestelling()
        {
            return _context.Bestellingen
                .Include(b => b.Klant.Bestellingen)
                .Include(b => b.BestelRegels)
                .OrderByDescending(b => b.BestelDatum)
                .FirstOrDefault(b => b.Goedgekeurd == false && b.Afgekeurd == false);
        }

        /// <inheritdoc/>
        public IEnumerable<Bestelling> GetAll()
        {
            return _context.Bestellingen
                .Include(b => b.BestelRegels)
                .Include(b => b.Klant.Bestellingen);
        }
    }
}
