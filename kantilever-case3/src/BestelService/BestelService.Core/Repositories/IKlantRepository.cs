using BestelService.Core.Models;

namespace BestelService.Core.Repositories
{
    public interface IKlantRepository
    {
        bool IsEmpty();
        Klant GetById(long id);
        void Add(params Klant[] klant);
    }
}
