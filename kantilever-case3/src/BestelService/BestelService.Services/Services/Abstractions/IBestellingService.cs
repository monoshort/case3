using System.Collections.Generic;
using BestelService.Core.Models;

namespace BestelService.Services.Services.Abstractions
{
    public interface IBestellingService
    {
        /// <summary>
        /// Maak a bestelling aan
        /// </summary>
        void MaakBestellingAan(Bestelling bestelling);

        /// <summary>
        /// Keur a bestelling goed
        /// </summary>
        void KeurBestellingGoed(long bestellingId);

        /// <summary>
        /// Keur a bestelling af
        /// </summary>
        void KeurBestellingAf(long bestellingId);

        /// <summary>
        /// Meld a bestelling klaar
        /// </summary>
        void MeldBestellingKlaar(long bestellingId);

        /// <summary>
        /// Pak a bestelregel in of bestelling
        /// </summary>
        void PakBestelRegelIn(long bestellingId, long bestelRegelId);

        /// <summary>
        /// Print the factuur of a bestelling
        /// </summary>
        void PrintFactuur(long bestellingId);

        /// <summary>
        /// Print the adres label of a bestelling
        /// </summary>
        void PrintAdresLabel(long bestellingId);

        /// <summary>
        /// Registreer a betaling on a bestelling
        /// </summary>
        void RegistreerBetaling(string bestellingNummer, decimal betaaldBedrag);

        /// <summary>
        /// Controleer op wanbetalers and return new wanbetalers
        /// </summary>
        IEnumerable<Bestelling> ControleerOpWanbetalingen();
    }
}
