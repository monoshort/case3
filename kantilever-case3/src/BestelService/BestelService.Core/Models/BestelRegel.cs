namespace BestelService.Core.Models
{
    public class BestelRegel
    {
        public long Id { get; set; }
        public string Leverancierscode { get; set; }
        public long ArtikelNummer { get; set; }
        public string Naam { get; set; }
        public int Aantal { get; set; }
        public bool Klaar { get; set; }
        public string AfbeeldingUrl { get; set; }
        public decimal StukPrijs { get; set; }
        public decimal StukPrijsInclusiefBtw { get; set; }
        public decimal RegelPrijs { get; set; }
        public decimal RegelPrijsInclusiefBtw { get; set; }
        public bool Ingepakt { get; set; }

        public void PakIn()
        {
            Ingepakt = true;
        }
    }
}
