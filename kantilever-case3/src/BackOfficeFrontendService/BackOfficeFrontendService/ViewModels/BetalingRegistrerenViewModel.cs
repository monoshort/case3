using System.Diagnostics.CodeAnalysis;

namespace BackOfficeFrontendService.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class BetalingRegistrerenViewModel
    {
        public string BestellingNummer { get; set; }
        public decimal OpenstaandBedrag { get; set; }
        public decimal BetaaldBedrag { get; set; }
        public decimal Verschil { get; set; }
    }
}
