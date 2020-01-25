using System.Collections.Generic;
using System.Threading.Tasks;
using BackOfficeFrontendService.Models;

namespace BackOfficeFrontendService.Agents.Abstractions
{
    public interface IBestellingAgent
    {
        /// <summary>
        /// Send out a command to indicate the bestelling adres label should be printed
        /// </summary>
        Task BestellingPrintAdresLabelAsync(long bestellingId);

        /// <summary>
        /// Send out a command to indicate the bestelling factuur should be printed
        /// </summary>
        Task BestellingPrintFactuurAsync(long bestellingId);

        /// <summary>
        /// Send out a command to indicate the bestelling should be goedgekeurd
        /// </summary>
        Task KeurBestellingGoedAsync(long bestellingId);

        /// <summary>
        /// Send out a command to indicate the bestelling should be afgekeurd
        /// </summary>
        Task KeurBestellingAfAsync(long bestellingId);

        /// <summary>
        /// Send out a command to indicate the bestelregel of the bestelling should become ingepakt
        /// </summary>
        Task PakBestelregelInAsync(long bestellingId, long bestelregelId);

        /// <summary>
        /// Send out a command to indciate the bestelling should become klaargemeld
        /// </summary>
        Task MeldBestellingKlaarAsync(long bestellingId);

        /// <summary>
        /// Register a command indicating that a betaling has been registered
        /// </summary>
        Task RegistreerBetalingAsync(string bestellingNummer, decimal betaaldBedrag);

        /// <summary>
        /// Check if there are any new wanbetalers and return any new wanbetalers
        /// </summary>
        Task<IEnumerable<Bestelling>> ControleerOfErWanbetalersZijnAsync();
    }
}
