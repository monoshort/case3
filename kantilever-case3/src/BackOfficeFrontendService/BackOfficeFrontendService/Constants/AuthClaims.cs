using System.Diagnostics.CodeAnalysis;

namespace BackOfficeFrontendService.Constants
{
    [ExcludeFromCodeCoverage]
    internal static class AuthClaims
    {
        internal const string KanBestellingInpakkan = "KanBestellingInpakken";
        internal const string KanBestellingKeuren = "KanBestellingKeuren";
        internal const string KanBetalingRegistreren = "KanBetalingRegistreren";
        internal const string KanArtikelenBijbestellen = "KanArtikelenBijbestellen";
        internal const string KanWanBetalersBekijken = "KanWanBetalersBekijken";

        internal const string True = "true";
    }
}
