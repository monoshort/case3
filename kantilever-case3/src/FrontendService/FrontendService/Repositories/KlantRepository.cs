using System.Linq;
using FrontendService.DAL;
using FrontendService.Models;
using FrontendService.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace FrontendService.Repositories
{
    public class KlantRepository : IKlantRepository
    {
        /// <summary>
        /// Context to communicate with the database
        /// </summary>
        private readonly FrontendContext _context;

        /// <summary>
        /// Instantiate a Klant repository with a frontend context
        /// </summary>
        public KlantRepository(FrontendContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public bool IsEmpty()
        {
            return !_context.Klanten.Any();
        }

        /// <summary>
        /// Add a klant entity to the database
        /// </summary>
        public void Add(Klant klant)
        {
            _context.Klanten.Add(klant);
            _context.SaveChanges();
        }

        /// <summary>
        /// Get a klant by its id
        /// </summary>
        public Klant GetById(long id)
        {
            return _context.Klanten.Include(kl => kl.Factuuradres).SingleOrDefault(kl => kl.Id == id);
        }

        /// <summary>
        /// Get a klant by its username
        /// </summary>
        public Klant GetByUsername(string username)
        {
            return _context.Klanten.Include(kl => kl.Factuuradres).SingleOrDefault(kl => kl.Username == username);
        }
    }
}
