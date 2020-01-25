using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace BestelService.Core.Models
{
    [ExcludeFromCodeCoverage]
    public class Klant
    {
        public long Id { get; set; }
        public string Naam { get; set; }
        public Adres Factuuradres { get; set; }
        public string Telefoonnummer { get; set; }

        [JsonIgnore]
        public ICollection<Bestelling> Bestellingen { get; set; } = new List<Bestelling>();
    }
}
