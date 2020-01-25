using System.Diagnostics.CodeAnalysis;

namespace FrontendService.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class WinkelwagenRijViewModel
    {
        public ArtikelViewModel Artikel { get; set; }
        public int Aantal { get; set; }
    }
}
