using System.Collections.Generic;
using System.Linq;

namespace BackOfficeFrontendService.Models
{
    public class VoorraadMagazijn
    {
        public ICollection<BestelRegel> BestelRegels { get; set; } = new List<BestelRegel>();
        public long ArtikelNummer { get; set; }
        public int Voorraad { get; set; }
        public string Leverancier { get; set; }
        public string Leveranciercode { get; set; }
        public bool VoorraadBesteld { get; set; }

        private long BesteldAantal()
        {
            return BestelRegels.Where(e => !e.Bestelling.KlaarGemeld)
                .Sum(e => e.Aantal);
        }

        public bool IsBijbestellenNodig => BesteldAantal() > Voorraad;
        public long BijTeBestellen => BesteldAantal() - Voorraad;
    }
}
