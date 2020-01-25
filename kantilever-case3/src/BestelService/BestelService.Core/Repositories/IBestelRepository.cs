using System.Collections.Generic;
using BestelService.Core.Models;

namespace BestelService.Core.Repositories
{
    public interface IBestelRepository
    {
        /// <summary>
        /// Add a new bestelling
        /// </summary>
        void Add(Bestelling bestelling);

        /// <summary>
        /// Update a bestelling
        /// </summary>
        void Update(Bestelling bestelling);

        /// <summary>
        /// Get a bestelling by its id
        /// </summary>
        Bestelling GetById(long id);

        /// <summary>
        /// Get a bestelling by its bestellingnummer
        /// </summary>
        Bestelling GetByBestellingNummer(string bestellingNummer);

        /// <summary>
        /// Get all bestellingen
        /// </summary>
        IEnumerable<Bestelling> GetAll();

        /// <summary>
        /// Get most recent unassessed bestelling
        /// </summary>
        Bestelling GetMostRecentUnassessedBestelling();
    }
}
