using System.Diagnostics.CodeAnalysis;

namespace BackOfficeFrontendService.Constants
{
    [ExcludeFromCodeCoverage]
    internal static class TopicNames
    {
        #region Bestelling
        internal const string NieuweBestellingAangemaakt = "Kantilever.BestelService.NieuweBestellingAangemaakt";
        internal const string BestellingGoedgekeurd = "Kantilever.BestelService.BestellingGoedgekeurd";
        internal const string BestellingAfgekeurd = "Kantilever.BestelService.BestellingAfgekeurd";
        internal const string BestellingFactuurGeprint = "Kantilever.BestelService.BestellingFactuurGeprint";
        internal const string BestellingAdresLabelGeprint = "Kantilever.BestelService.BestellingAdresLabelGeprint";
        internal const string BestellingKlaarGemeld = "Kantilever.BestelService.BestellingKlaargemeld";
        internal const string BestelRegelIngepakt = "Kantilever.BestelService.BestelRegelIngepakt";
        internal const string BetalingGeregistreerd = "Kantilever.BestelService.BetalingGeregistreerd";
        internal const string BestellingKanKlaarGemeldWorden = "Kantilever.BestelService.BestellingKanKlaarGemeldWorden";
        internal const string KlantIsWanbetalerGeworden = "Kantilever.BestelService.KlantIsWanbetalerGeworden";
        #endregion

        #region Klant
        internal const string NieuweKlantAangemaakt = "Kantilever.KlantService.NieuweKlantAangemaakt";
        #endregion

        #region voorraad
        internal const string VoorraadVerhoogdEvent = "Kantilever.MagazijnService.VoorraadVerhoogdEvent";
        internal const string VoorraadVerlaagdEvent = "Kantilever.MagazijnService.VoorraadVerlaagdEvent";
        internal const string VoorraadBesteldEvent = "Kantilever.MagazijnService.VoorraadBesteldEvent";
        #endregion

        #region catalogus
        internal const string ArtikelAanCatalogusToegevoegd = "Kantilever.CatalogusService.ArtikelAanCatalogusToegevoegd";
        #endregion
    }
}
