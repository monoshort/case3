using System.Diagnostics.CodeAnalysis;

namespace BestelService.Services.Constants
{
    [ExcludeFromCodeCoverage]
    internal static class TopicNames
    {
        #region Bestelling
        internal const string NieuweBestellingAangemaakt = "Kantilever.BestelService.NieuweBestellingAangemaakt";
        internal const string BestellingGoedgekeurd = "Kantilever.BestelService.BestellingGoedgekeurd";
        internal const string BestellingAfgekeurd = "Kantilever.BestelService.BestellingAfgekeurd";
        internal const string BestellingKlaarGemeld = "Kantilever.BestelService.BestellingKlaargemeld";
        internal const string BestelRegelIngepakt = "Kantilever.BestelService.BestelRegelIngepakt";
        internal const string BetalingGeregistreerd = "Kantilever.BestelService.BetalingGeregistreerd";
        internal const string FactuurGeprint = "Kantilever.BestelService.BestellingFactuurGeprint";
        internal const string AdresLabelGeprint = "Kantilever.BestelService.BestellingAdresLabelGeprint";
        internal const string BestellingKanKlaarGemeldWorden = "Kantilever.BestelService.BestellingKanKlaarGemeldWorden";
        internal const string KlantIsWanbetalerGeworden = "Kantilever.BestelService.KlantIsWanbetalerGeworden";
        #endregion
    }
}
