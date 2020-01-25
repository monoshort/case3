using System;
using System.Diagnostics.CodeAnalysis;
using FrontendService.Constants;

namespace FrontendService.Models
{
    [ExcludeFromCodeCoverage]
    public class BestelRegel
    {
        public long Id { get; set; }
        public string Leverancierscode { get; set; }
        public long ArtikelNummer { get; set; }
        public string Naam { get; set; }
        public int Aantal { get; set; }
        public string AfbeeldingUrl { get; set; }
        public decimal StukPrijs { get; set; }
        public decimal RegelPrijs =>
            Math.Round(Aantal * StukPrijs, SystemVariables.DefaultAmountOfDecimals);
        public decimal StukPrijsInclusiefBtw =>
            Math.Round(StukPrijs * SystemVariables.BtwMultiplier, SystemVariables.DefaultAmountOfDecimals);
        public decimal RegelPrijsInclusiefBtw =>
            Math.Round(Aantal * StukPrijsInclusiefBtw, SystemVariables.DefaultAmountOfDecimals);
    }
}
