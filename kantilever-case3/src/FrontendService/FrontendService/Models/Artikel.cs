using System;
using FrontendService.Constants;

namespace FrontendService.Models
{
    public class Artikel
    {
        public long Id { get; set; }
        public long Artikelnummer { get; set; }
        public string Naam { get; set; }
        public string Beschrijving { get; set; }
        public decimal Prijs { get; set; }
        public decimal PrijsInclBtw =>
            Math.Round(Prijs * SystemVariables.BtwMultiplier, SystemVariables.DefaultAmountOfDecimals);

        public string AfbeeldingUrl { get; set; }
        public DateTime? LeverbaarVanaf { get; set; }
        public DateTime? LeverbaarTot { get; set; }
        public string Leveranciercode { get; set; }
        public string Leverancier { get; set; }
        public string Categorie { get; set; }
        public string SubCategorie { get; set; }
        public int Voorraad { get; set; }

    }
}
