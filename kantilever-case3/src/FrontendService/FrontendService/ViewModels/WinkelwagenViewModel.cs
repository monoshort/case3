using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace FrontendService.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class WinkelwagenViewModel
    {
        [JsonProperty("artikelen")]
        public WinkelwagenRijViewModel[] Artikelen { get; set; }
    }
}
