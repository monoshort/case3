using System.Collections.Generic;
using FrontendService.Models;

namespace FrontendService.Repositories.Abstractions
{
    public interface IBestellingRepository
    {
        void Add(Bestelling bestelling);
        bool IsEmpty();
        Bestelling GetById(long id);
        IEnumerable<Bestelling> GetByKlantUsername(string username);
        void Update(Bestelling bestelling);
    }
}
