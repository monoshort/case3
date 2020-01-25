using System.Diagnostics.CodeAnalysis;
using FrontendService.Models;

namespace FrontendService.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class BestellingViewModel
    {
        public WinkelwagenViewModel Winkelwagen { get; set; }
        public KlantViewModel Klant { get; set; }
        public Adres AfleverAdres { get; set; }
    }
}
