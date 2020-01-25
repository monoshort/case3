using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FrontendService.Constants;

namespace FrontendService.Models
{
    [ExcludeFromCodeCoverage]
    public class Bestelling
    {
        public long Id { get; set; }
        public Klant Klant { get; set; }
        public long KlantId { get; set; }
        public string BestellingNummer { get; set; }
        public ICollection<BestelRegel> BestelRegels { get; set; } = new List<BestelRegel>();
        public bool Goedgekeurd { get; set; }
        public bool Afgekeurd { get; set; }
        public bool KlaarGemeld { get; set; }
        public bool Ingepakt { get; set; }
        public Adres AfleverAdres { get; set; }
        public DateTime BestelDatum { get; set; }
        public decimal SubtotaalInclusiefBtw => BestelRegels.Sum(e => e.RegelPrijsInclusiefBtw);
        public decimal SubtotaalInclusiefBtwMetVerzendKosten =>
            Math.Round(SubtotaalInclusiefBtw + SystemVariables.VerzendKostenInclusiefBtw, SystemVariables.DefaultAmountOfDecimals);
        public decimal Subtotaal => BestelRegels.Sum(e => e.RegelPrijs);
        public decimal SubtotaalMetVerzendKosten =>
            Math.Round(Subtotaal + SystemVariables.VerzendKostenExclusiefBtw, SystemVariables.DefaultAmountOfDecimals);

        // Is equal to the the subtotaal because no payments happen on the frontend
        public decimal OpenstaandBedrag => SubtotaalInclusiefBtwMetVerzendKosten;
    }
}
