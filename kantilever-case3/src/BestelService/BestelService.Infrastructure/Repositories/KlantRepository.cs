using System.Linq;
using BestelService.Core.Models;
using BestelService.Core.Repositories;
using BestelService.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;

namespace BestelService.Infrastructure.Repositories
{
    public class KlantRepository : IKlantRepository
    {
        private readonly BestelContext _context;

        public KlantRepository(BestelContext context)
        {
            _context = context;
        }

        public bool IsEmpty()
        {
            return !_context.Klanten.Any();
        }

        public Klant GetById(long id)
        {
            return _context.Klanten
                .Include(k => k.Bestellingen)
                .Include(k => k.Factuuradres)
                .SingleOrDefault(k => k.Id == id);
        }

        public void Add(params Klant[] klant)
        {
            _context.Klanten.AddRange(klant);
            _context.SaveChanges();
        }
    }
}
