using System.Collections.Generic;
using FrontendService.Models;

namespace FrontendService.Repositories.Abstractions
{
    /// <summary>
    /// Repository to manage persisted artikel data
    /// </summary>
    public interface IArtikelRepository
    {
        /// <summary>
        /// Check if klant repository is empty
        /// </summary>
        bool IsEmpty();

        /// <summary>
        /// Return all artikels in the database
        /// </summary>
        IEnumerable<Artikel> GetAll();

        /// <summary>
        /// Add artikel(en) to the database
        /// </summary>
        void Add(params Artikel[] artikelen);

        /// <summary>
        /// Get an Artikel by its artikel id
        /// </summary>
        Artikel GetById(long artikelId);

        /// <summary>
        /// Get an Artikel by its ArtikelNummer
        /// </summary>
        Artikel GetByArtikelnummer(long artikelnummer);

        /// <summary>
        /// Update an existing article
        /// </summary>
        void Update(Artikel artikel);
    }
}
