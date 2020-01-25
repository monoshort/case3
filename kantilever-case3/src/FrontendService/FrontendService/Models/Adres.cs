using System.Diagnostics.CodeAnalysis;

namespace FrontendService.Models
{
    [ExcludeFromCodeCoverage]
    public class Adres
    {
        public string StraatnaamHuisnummer { get; set; }
        public string Postcode { get; set; }
        public string Woonplaats { get; set; }
    }
}
