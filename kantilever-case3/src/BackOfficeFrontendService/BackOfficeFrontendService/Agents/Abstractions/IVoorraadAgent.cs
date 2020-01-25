using System.Threading.Tasks;
using BackOfficeFrontendService.Commands;
using BackOfficeFrontendService.Models;

namespace BackOfficeFrontendService.Agents.Abstractions
{
    public interface IVoorraadAgent
    {
        /// <summary>
        /// Send out a request for all voorraad of the artikelen in de catalogus
        /// </summary>
        Task<VoorraadMagazijn[]> GetAllVoorraadAsync();

        /// <summary>
        /// Send out a command to decrease the amount of voorraad of a particular voorraad
        /// </summary>
        Task HaalVoorraadUitMagazijnAsync(HaalVoorraadUitMagazijnCommand command);

        /// <summary>
        /// Publish a voorraadbesteld event to the rest of the system
        /// </summary>
        Task ThrowVoorraadBesteldEventAsync(long artikelNummer, long aantal);
    }
}
