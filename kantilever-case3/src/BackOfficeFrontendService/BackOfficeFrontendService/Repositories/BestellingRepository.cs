using System.Collections.Generic;
using System.Linq;
using BackOfficeFrontendService.DAL;
using BackOfficeFrontendService.Models;
using BackOfficeFrontendService.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace BackOfficeFrontendService.Repositories
{
    /// <summary>
    /// Repository to persist bestellingen
    /// </summary>
    public class BestellingRepository : IBestellingRepository
    {
        /// <summary>
        /// Database context
        /// </summary>
        private readonly BackOfficeContext _context;

        /// <summary>
        /// Initialize a repository with a given context
        /// </summary>
        public BestellingRepository(BackOfficeContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get an inpack opdracht by its bestelnummer
        /// </summary>
        public Bestelling GetInpakOpdrachtMetId(long bestellingId)
        {
            return _context.Bestellingen
                .Include(b => b.Klant.Factuuradres)
                .Include(b => b.AfleverAdres)
                .Include(b => b.BestelRegels)
                .ThenInclude(b => b.Voorraad)
                .FirstOrDefault(b => b.Id == bestellingId);
        }

        /// <summary>
        /// See if the database is empty
        /// </summary>
        public bool IsEmpty()
        {
            return !_context.Bestellingen.Any();
        }

        /// <summary>
        /// Get the next inpak opdracht
        /// </summary>
        public Bestelling GetVolgendeInpakOpdracht()
        {
            return _context.Bestellingen
                .Where(b => !b.KlaarGemeld && b.Goedgekeurd)
                .Include(b => b.BestelRegels)
                .ThenInclude(br => br.Voorraad)
                .AsEnumerable()
                .FirstOrDefault(b => b.VoorraadBeschikbaar);
        }

        /// <summary>
        /// Add a bestelling to the database
        /// </summary>
        public void Add(Bestelling bestelling)
        {
            _context.Bestellingen.Add(bestelling);
            _context.SaveChanges();
        }

        /// <summary>
        /// Update an existing bestelling in the database
        /// </summary>
        public void Update(Bestelling bestelling)
        {
            _context.Entry(bestelling).CurrentValues.SetValues(bestelling);
            _context.SaveChanges();
        }

        /// <summary>
        /// Get all niet gekeurde bestellingen
        /// </summary>
        public IEnumerable<Bestelling> GetNietGekeurdeBestellingen()
        {
            return _context.Bestellingen.Where(b => !b.Goedgekeurd && !b.Afgekeurd)
                .Include(b => b.Klant.Factuuradres)
                .Include(b => b.BestelRegels);
        }

        /// <summary>
        /// Get a Bestelling by its bestellingnummer
        /// </summary>
        public Bestelling GetBestellingByBestellingNummer(string bestellingNummer)
        {
            return _context.Bestellingen.Where(bestelling => bestelling.BestellingNummer == bestellingNummer)
                .Include(best => best.BestelRegels)
                .Include(best => best.Klant)
                .Include(best => best.Klant.Factuuradres)
                .FirstOrDefault();
        }

        /// <summary>
        /// Get all bestellingen with wanbetalers
        /// </summary>
        public IEnumerable<Bestelling> GetWanbetaalBestellingen()
        {
            return _context.Bestellingen.Where(e => e.IsKlantWanbetaler);
        }
    }
}
