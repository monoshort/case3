using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FrontendService.Constants;
using Minor.Miffy.MicroServices.Events;

namespace FrontendService.Events
{
    [ExcludeFromCodeCoverage]
    public class ArtikelAanCatalogusToegevoegdEvent : DomainEvent
    {
        public ArtikelAanCatalogusToegevoegdEvent() : base(TopicNames.ArtikelAanCatalogusToegevoegd)
        {
            Categorieen = new List<string>();
        }

        public long Artikelnummer { get; set; }
        public string Naam { get; set; }
        public string Beschrijving { get; set; }
        public decimal Prijs { get; set; }
        public string AfbeeldingUrl { get; set; }
        public DateTime LeverbaarVanaf { get; set; }
        public DateTime? LeverbaarTot { get; set; }
        public string Leveranciercode { get; set; }
        public string Leverancier { get; set; }
        public IList<string> Categorieen { get; set; }
    }
}
