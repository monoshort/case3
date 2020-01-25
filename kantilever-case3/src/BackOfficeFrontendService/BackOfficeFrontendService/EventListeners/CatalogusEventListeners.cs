using BackOfficeFrontendService.Constants;
using BackOfficeFrontendService.Events;
using BackOfficeFrontendService.Models;
using BackOfficeFrontendService.Repositories.Abstractions;
using Minor.Miffy.MicroServices.Events;

namespace BackOfficeFrontendService.EventListeners
{
    public class CatalogusEventListeners
    {
        private readonly IVoorraadRepository _voorraadRepository;

        public CatalogusEventListeners(IVoorraadRepository voorraadRepository)
        {
            _voorraadRepository = voorraadRepository;
        }

        [EventListener]
        [Topic(TopicNames.ArtikelAanCatalogusToegevoegd)]
        public void HandleArtikelAanCatalogusToegevoegd(ArtikelAanCatalogusToegevoegdEvent evt)
        {
            VoorraadMagazijn voorraadMagazijn = new VoorraadMagazijn
            {
                ArtikelNummer = evt.Artikelnummer,
                Voorraad = 0,
                Leverancier = evt.Leverancier,
                Leveranciercode = evt.Leveranciercode,
                VoorraadBesteld = false
            };

            _voorraadRepository.Add(voorraadMagazijn);
        }
    }
}
