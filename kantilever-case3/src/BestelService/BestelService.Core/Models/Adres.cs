using System.Diagnostics.CodeAnalysis;

namespace BestelService.Core.Models
{
    [ExcludeFromCodeCoverage]
    public class Adres
    {
        public string StraatnaamHuisnummer { get; set; }
        public string Woonplaats { get; set; }
        public string Postcode { get; set; }
    }
}
