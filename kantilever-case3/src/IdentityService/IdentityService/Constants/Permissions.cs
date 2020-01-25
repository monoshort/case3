using System.Diagnostics.CodeAnalysis;

namespace IdentityService.Constants
{
    [ExcludeFromCodeCoverage]
    internal static class Permissions
    {
        internal const string KanBestellingInpakken = "KanBestellingInpakken";
        internal const string KanBestellingKeuren = "KanBestellingKeuren";
        internal const string KanBetalingInvoeren = "KanBetalingInvoeren";
        internal const string KanBestellen = "KanBestellen";
        internal const string KanArtikelenZien = "KanArtikelenZien";
        internal const string KanArtikelenBijbestellen = "KanArtikelenBijbestellen";
        internal const string KanWanBetalersBekijken = "KanWanBetalersBekijken";
        internal const string KanBetalingRegistreren = "KanBetalingRegistreren";
    }
}
