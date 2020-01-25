using System.Diagnostics.CodeAnalysis;

namespace FrontendService.Constants
{
    [ExcludeFromCodeCoverage]
    internal static class SystemVariables
    {
        internal const decimal BtwMultiplier = 1.21M;
        internal const int DefaultAmountOfDecimals = 2;
        internal const decimal VerzendKostenInclusiefBtw = 6.00M;
        internal const decimal VerzendKostenExclusiefBtw = 6.00M / 1.21M;
    }
}
