using System;
using System.Diagnostics.CodeAnalysis;

namespace FrontendService.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class ArtikelDetailViewModel
    {
        public long Id { get; set; }
        public long Artikelnummer { get; set; }
        public string Naam { get; set; }
        public string Beschrijving { get; set; }
        public decimal Prijs { get; set; }
        public decimal PrijsInclBtw { get; set; }
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
