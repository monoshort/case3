using System.Diagnostics.CodeAnalysis;

namespace FrontendService.Constants
{
    [ExcludeFromCodeCoverage]
    internal static class QueueNames
    {
        internal const string MaakNieuweBestellingAanCommand = "Kantilever.BestelService.MaakNieuweBestellingAan";
        internal const string MaakNieuweKlantAanCommand = "Kantilever.KlantService.MaakNieuweKlantAan";
        internal const string MaakAccountAan = "Kantilever.IdentityService.MaakAccountAan";
        internal const string VerwijderAccount = "Kantilever.IdentityService.VerwijderAccount";
    }
}
