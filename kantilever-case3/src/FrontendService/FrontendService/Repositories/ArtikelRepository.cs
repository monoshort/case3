using System.Collections.Generic;
using System.Linq;
using FrontendService.DAL;
using FrontendService.Models;
using FrontendService.Repositories.Abstractions;

namespace FrontendService.Repositories
{
    /// <inheritdoc/>
    public class ArtikelRepository : IArtikelRepository
    {
        /// <summary>
        /// Context to communicate with the database
        /// </summary>
        private readonly FrontendContext _context;

        /// <summary>
        /// Instantiate an artikel repository with a artikel context
        /// </summary>
        public ArtikelRepository(FrontendContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public void Add(params Artikel[] artikelen)
        {
            _context.Artikelen.AddRange(artikelen);
            _context.SaveChanges();
        }

        /// <inheritdoc/>
        public Artikel GetById(long artikelId)
        {
            return _context.Artikelen.Find(artikelId);
        }

        /// <inheritdoc/>
        public Artikel GetByArtikelnummer(long artikelnummer)
        {
            return _context.Artikelen.SingleOrDefault(e => e.Artikelnummer == artikelnummer);
        }

        /// <inheritdoc/>
        public bool IsEmpty()
        {
            return !_context.Artikelen.Any();
        }

        /// <inheritdoc/>
        public IEnumerable<Artikel> GetAll()
        {
            return _context.Artikelen.ToList();
        }

        /// <inheritdoc/>
        public void Update(Artikel artikel)
        {
            _context.Entry(artikel).CurrentValues.SetValues(artikel);
            _context.SaveChanges();
        }
    }
}
