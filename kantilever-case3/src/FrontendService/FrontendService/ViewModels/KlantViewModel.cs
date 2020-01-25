using System.Diagnostics.CodeAnalysis;
using FrontendService.Models;

namespace FrontendService.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class KlantViewModel
    {
        public long Id { get; set; }
        public string Naam { get; set; }
        public Adres Factuuradres { get; set; }
        public string Telefoonnummer { get; set; }
    }
}
