using System.Diagnostics.CodeAnalysis;

namespace BackOfficeFrontendService.Constants
{
    /// <summary>
    /// Public constants since they need to be used in the view
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class AuthPolicies
    {
        public const string KanBestellingInpakkenPolicy = "KanBestellingInpakkenPolicy";
        public const string KanBestellingKeurenPolicy = "KanBestellingKeurenPolicy";
        public const string KanBetalingRegistrerenPolicy = "KanBetalingRegistrerenPolicy";
        public const string KanArtikelenBijbestellenPolicy = "KanArtikelenBijbestellen";
        public const string KanWanBetalersBekijken = "KanWanBetalersBekijken";
    }
}
