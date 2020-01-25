using System.Linq;
using BackOfficeFrontendService.DAL;
using BackOfficeFrontendService.Models;
using BackOfficeFrontendService.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace BackOfficeFrontendService.Repositories
{
    public class KlantRepository : IKlantRepository
    {
        /// <summary>
        /// Backoffice context
        /// </summary>
        private readonly BackOfficeContext _context;

        /// <summary>
        /// Instantiate repository with context
        /// </summary>
        public KlantRepository(BackOfficeContext backOfficeContext)
        {
            _context = backOfficeContext;
        }

        /// <summary>
        /// Persist a new klant
        /// </summary>
        public void Add(Klant klant)
        {
            _context.Klanten.Add(klant);
            _context.SaveChanges();
        }

        /// <summary>
        /// See if the database is empty
        /// </summary>
        public bool IsEmpty()
        {
            return !_context.Klanten.Any();
        }

        /// <summary>
        /// Find a klant by its id
        /// </summary>
        public Klant FindById(long id)
        {
            return _context.Klanten.Include(k => k.Factuuradres).SingleOrDefault(k => k.Id == id);
        }
    }
}
