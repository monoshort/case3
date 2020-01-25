using System.Collections.Generic;
using BackOfficeFrontendService.Models;

namespace BackOfficeFrontendService.Repositories.Abstractions
{
    /// <summary>
    /// Repository to persist bestellingen
    /// </summary>
    public interface IBestellingRepository
    {
        /// <summary>
        /// See if the database is empty or not
        /// </summary>
        bool IsEmpty();

        /// <summary>
        /// Get the next inpak opdracht
        /// </summary>
        Bestelling GetVolgendeInpakOpdracht();

        /// <summary>
        /// Get an inpack opdracht by its bestellingId
        /// </summary>
        Bestelling GetInpakOpdrachtMetId(long bestellingId);

        /// <summary>
        /// Add a bestelling to the database
        /// </summary>
        void Add(Bestelling bestelling);

        /// <summary>
        /// Update an existing bestelling in the database
        /// </summary>
        void Update(Bestelling bestelling);

        /// <summary>
        /// Get all niet gekeurde bestellingen
        /// </summary>
        IEnumerable<Bestelling> GetNietGekeurdeBestellingen();

        /// <summary>
        /// Get bestelling by its bestellingnummer
        /// </summary>
        Bestelling GetBestellingByBestellingNummer(string bestellingNummer);

        /// <summary>
        /// Get wanbetaal bestellingen
        /// </summary>
        /// <returns></returns>
        IEnumerable<Bestelling> GetWanbetaalBestellingen();
    }
}
