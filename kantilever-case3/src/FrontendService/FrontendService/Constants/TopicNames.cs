using System.Diagnostics.CodeAnalysis;

namespace FrontendService.Constants
{
    [ExcludeFromCodeCoverage]
    internal static class TopicNames
    {
        internal const string ArtikelAanCatalogusToegevoegd = "Kantilever.CatalogusService.ArtikelAanCatalogusToegevoegd";
        internal const string VoorraadVerhoogdEvent = "Kantilever.MagazijnService.VoorraadVerhoogdEvent";
        internal const string VoorraadVerlaagdEvent = "Kantilever.MagazijnService.VoorraadVerlaagdEvent";
        internal const string NieuweKlantAangemaakt = "Kantilever.KlantService.NieuweKlantAangemaakt";
        internal const string NieuweBestellingAangemaakt = "Kantilever.BestelService.NieuweBestellingAangemaakt";
        internal const string BestellingGoedgekeurd = "Kantilever.BestelService.BestellingGoedgekeurd";
        internal const string BestellingAfgekeurd = "Kantilever.BestelService.BestellingAfgekeurd";
        internal const string BestellingKlaarGemeld = "Kantilever.BestelService.BestellingKlaargemeld";
    }
}
