using FrontendService.Models;

namespace FrontendService.Agents.Abstractions
{
    public interface IBestellingAgent
    {
        /// <summary>
        /// Publish a bestelling
        /// </summary>
        void Bestel(Bestelling bestelling);
    }
}
