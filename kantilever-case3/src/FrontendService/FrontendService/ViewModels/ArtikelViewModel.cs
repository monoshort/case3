using System.Diagnostics.CodeAnalysis;

namespace FrontendService.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class ArtikelViewModel
    {
        public long Id { get; set; }
        public string Naam { get; set; }
        public decimal Prijs { get; set; }
        public decimal PrijsInclBtw { get; set; }
        public int BeschikbaarAantal { get; set; }
        public string AfbeeldingUrl { get; set; }

        public string Beschrijving { get; set; }
        public string Categorie { get; set; }
    }
}
