using System.Collections.Generic;
using BackOfficeFrontendService.Models;

namespace BackOfficeFrontendService.Repositories.Abstractions
{
    public interface IVoorraadRepository
    {
        bool IsEmpty();
        void Add(params VoorraadMagazijn[] voorraad);
        void Update(VoorraadMagazijn voorraadMagazijn);
        VoorraadMagazijn GetByArtikelNummer(long bestelRegelArtikelNummer);
        IEnumerable<VoorraadMagazijn> GetArtikelenNietOpVoorraad();
    }
}
