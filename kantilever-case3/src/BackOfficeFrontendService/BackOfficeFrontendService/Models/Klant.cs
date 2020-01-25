using System.Diagnostics.CodeAnalysis;

namespace BackOfficeFrontendService.Models
{
    [ExcludeFromCodeCoverage]
    public class Klant
    {
        public long Id { get; set; }
        public string Naam { get; set; }
        public Adres Factuuradres { get; set; }
        public string Telefoonnummer { get; set; }
    }
}