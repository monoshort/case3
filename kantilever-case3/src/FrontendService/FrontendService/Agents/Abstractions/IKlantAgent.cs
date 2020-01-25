using System.Threading.Tasks;
using FrontendService.Models;

namespace FrontendService.Agents.Abstractions
{
    public interface IKlantAgent
    {
        Task<Klant> MaakKlantAanAsync(Klant klant);
    }
}