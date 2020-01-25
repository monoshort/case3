using System.Diagnostics.CodeAnalysis;

namespace KlantService.Models
{
    [ExcludeFromCodeCoverage]
    public class Klant
    {
        public long Id { get; set; }
        public string Naam { get; set; }
        public Adres Factuuradres { get; set; }
        public string Telefoonnummer { get; set; }
        public string Username { get; set; }
    }
}
