using System.Collections.Generic;
using System.Linq;
using BackOfficeFrontendService.DAL;
using BackOfficeFrontendService.Models;
using BackOfficeFrontendService.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace BackOfficeFrontendService.Repositories
{
    public class VoorraadRepository : IVoorraadRepository
    {
        private readonly BackOfficeContext _context;

        public VoorraadRepository(BackOfficeContext context)
        {
            _context = context;
        }

        public bool IsEmpty()
        {
            return !_context.VoorraadMagazijn.Any();
        }

        public void Add(params VoorraadMagazijn[] voorraad)
        {
            _context.VoorraadMagazijn.AddRange(voorraad);
            _context.SaveChanges();
        }

        public void Update(VoorraadMagazijn voorraadMagazijn)
        {
            _context.Entry(voorraadMagazijn).CurrentValues.SetValues(voorraadMagazijn);
            _context.SaveChanges();
        }

        public VoorraadMagazijn GetByArtikelNummer(long bestelRegelArtikelNummer)
        {
            return _context.VoorraadMagazijn.Find(bestelRegelArtikelNummer);
        }

        public IEnumerable<VoorraadMagazijn> GetArtikelenNietOpVoorraad()
        {
            return _context.VoorraadMagazijn
                .Where(e => !e.VoorraadBesteld)
                .Include(e => e.BestelRegels)
                .ThenInclude(e => e.Bestelling)
                .AsEnumerable()
                .Where(e => e.IsBijbestellenNodig);
        }
    }
}
