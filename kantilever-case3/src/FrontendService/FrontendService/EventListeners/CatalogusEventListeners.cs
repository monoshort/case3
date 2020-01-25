using FrontendService.Constants;
using FrontendService.Events;
using FrontendService.Models;
using FrontendService.Repositories.Abstractions;
using Minor.Miffy.MicroServices.Events;

namespace FrontendService.Listeners
{   
    public class CatalogusEventListeners
    {
        private readonly IArtikelRepository _artikelRepository;

        public CatalogusEventListeners(IArtikelRepository artikelRepository)
        {
            _artikelRepository = artikelRepository;
        }

        [EventListener]
        [Topic(TopicNames.ArtikelAanCatalogusToegevoegd)]
        public void HandleArtikelToegevoegd(ArtikelAanCatalogusToegevoegdEvent @event)
        {
            var artikel = new Artikel
            {
                AfbeeldingUrl = @event.AfbeeldingUrl,
                Artikelnummer = @event.Artikelnummer,
                Beschrijving = @event.Beschrijving,
                Leverancier = @event.Leverancier,
                Leveranciercode = @event.Leveranciercode,
                LeverbaarTot = @event.LeverbaarTot,
                LeverbaarVanaf = @event.LeverbaarVanaf,
                Naam = @event.Naam,
                Prijs = @event.Prijs,
            };
            _artikelRepository.Add(artikel);
        }
    }
}