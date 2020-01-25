using KlantService.DAL;
using KlantService.Models;

namespace KlantService.Repositories
{
    public class KlantRepository : IKlantRepository
    {
        private readonly KlantContext _context;

        public KlantRepository(KlantContext context)
        {
            _context = context;
        }

        public void Add(Klant klant)
        {
            _context.Klanten.Add(klant);
            _context.SaveChanges();
        }
    }
}
