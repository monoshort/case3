using System.Collections.Generic;
using System.Linq;
using FrontendService.DAL;
using FrontendService.Models;
using FrontendService.Repositories.Abstractions;

namespace FrontendService.Repositories
{
    public class BestellingRepository : IBestellingRepository
    {
        private readonly FrontendContext _context;

        public BestellingRepository(FrontendContext context)
        {
            _context = context;
        }

        public void Add(Bestelling bestelling)
        {
            _context.Bestellingen.Add(bestelling);
            _context.SaveChanges();
        }

        /// <inheritdoc/>
        public bool IsEmpty()
        {
            return !_context.Bestellingen.Any();
        }

        public Bestelling GetById(long id)
        {
            return _context.Bestellingen.Find(id);
        }

        public IEnumerable<Bestelling> GetByKlantUsername(string username)
        {
            return _context.Bestellingen.Where(e => e.Klant.Username == username);
        }

        public void Update(Bestelling bestelling)
        {
            Bestelling fromDb = _context.Bestellingen.Find(bestelling.Id);
            _context.Entry(fromDb).CurrentValues.SetValues(bestelling);
            _context.SaveChanges();
        }
    }
}
