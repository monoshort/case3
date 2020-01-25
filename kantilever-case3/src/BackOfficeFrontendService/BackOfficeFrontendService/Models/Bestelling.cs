using System;
using System.Collections.Generic;
using System.Linq;

namespace BackOfficeFrontendService.Models
{
    public class Bestelling
    {
        public long Id { get; set; }
        public Klant Klant { get; set; }
        public string BestellingNummer { get; set; }
        public ICollection<BestelRegel> BestelRegels { get; set; } = new List<BestelRegel>();
        public bool Goedgekeurd { get; set; }
        public bool KlaarGemeld { get; set; }
        public Adres AfleverAdres { get; set; }
        public decimal Subtotaal { get; set; }
        public decimal SubtotaalMetVerzendKosten { get; set; }
        public decimal SubtotaalInclusiefBtw { get; set; }
        public decimal SubtotaalInclusiefBtwMetVerzendKosten { get; set; }
        public DateTime BestelDatum { get; set; }
        public bool Afgekeurd { get; set; }
        public bool AdresLabelGeprint { get; set; }
        public bool FactuurGeprint { get; set; }
        public decimal OpenstaandBedrag { get; set; }
        public bool KanKlaarGemeldWorden { get; set; }
        public bool IsKlantWanbetaler { get; set; }

        public bool VoorraadBeschikbaar => BestelRegels.All(r => r.VoorraadBeschikbaar);
    }

    public class BestellingEqualityComparer : IEqualityComparer<Bestelling>
    {
        public bool Equals(Bestelling x, Bestelling y)
        {
            if (x == null || y == null)
            {
                return false;
            }

            return x.Id == y.Id;
        }

        public int GetHashCode(Bestelling obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
