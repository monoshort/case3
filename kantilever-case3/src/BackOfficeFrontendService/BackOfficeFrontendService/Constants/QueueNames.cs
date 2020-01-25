using System.Diagnostics.CodeAnalysis;

namespace BackOfficeFrontendService.Constants
{
    [ExcludeFromCodeCoverage]
    internal static class QueueNames
    {
        #region Bestelling
        internal const string PrintFactuur = "Kantilever.BestelService.PrintFactuur";
        internal const string PrintAdresLabel = "Kantilever.BestelService.PrintAdresLabel";
        internal const string KeurBestellingGoed = "Kantilever.BestelService.KeurBestellingGoed";
        internal const string KeurBestellingAf = "Kantilever.BestelService.KeurBestellingAf";
        internal const string MeldBestellingKlaar = "Kantilever.BestelService.MeldBestelingKlaar";
        internal const string PakBestelRegelIn = "Kantilever.BestelService.PakBestelRegelIn";
        internal const string RegistreerBetaling = "Kantilever.BestelService.RegistreerBetaling";
        internal const string ControleerOfErWanbetalingenZijn = "Kantilever.BestelService.ControleerOfErWanbetalingenZijn";
        #endregion
    }
}
