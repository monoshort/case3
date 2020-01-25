using System.Diagnostics.CodeAnalysis;

namespace FrontendService.Models
{
    [ExcludeFromCodeCoverage]
    public class VoorraadMagazijn
    {
        public long ArtikelNummer { get; set; }
        public int Voorraad { get; set; }
    }
}
