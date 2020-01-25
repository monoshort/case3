using FrontendService.Models;

namespace FrontendService.Repositories.Abstractions
{
    public interface IKlantRepository
    {
        bool IsEmpty();
        void Add(Klant klant);
        Klant GetById(long id);
        Klant GetByUsername(string username);
    }
}
