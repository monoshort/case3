using System.Threading.Tasks;
using BackOfficeFrontendService.Models;

namespace BackOfficeFrontendService.Agents.Abstractions
{
    public interface ICatalogusAgent
    {
        /// <summary>
        /// Send out a request for all artikelen in the catalogus
        /// </summary>
        Task<Artikel[]> GetAlleArtikelenAsync();
    }
}
