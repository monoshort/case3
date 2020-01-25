using BackOfficeFrontendService.Constants;
using BackOfficeFrontendService.Events;
using BackOfficeFrontendService.Models;
using BackOfficeFrontendService.Repositories.Abstractions;
using Minor.Miffy.MicroServices.Events;

namespace BackOfficeFrontendService.EventListeners
{
    public class VoorraadEventListeners
    {
        private readonly IVoorraadRepository _voorraadRepository;

        public VoorraadEventListeners(IVoorraadRepository voorraadRepository)
        {
            _voorraadRepository = voorraadRepository;
        }

        [EventListener]
        [Topic(TopicNames.VoorraadBesteldEvent)]
        public void HandleVoorraadBesteld(VoorraadBesteldEvent evt)
        {
            VoorraadMagazijn voorraadMagazijn = _voorraadRepository.GetByArtikelNummer(evt.Artikelnummer);
            voorraadMagazijn.VoorraadBesteld = true;
            _voorraadRepository.Update(voorraadMagazijn);
        }

        [EventListener]
        [Topic(TopicNames.VoorraadVerlaagdEvent)]
        public void HandleVoorraadVerlaagd(VoorraadVerlaagdEvent evt)
        {
            VoorraadMagazijn voorraadMagazijn = _voorraadRepository.GetByArtikelNummer(evt.Artikelnummer);
            voorraadMagazijn.Voorraad = evt.NieuweVoorraad;
            _voorraadRepository.Update(voorraadMagazijn);
        }

        [EventListener]
        [Topic(TopicNames.VoorraadVerhoogdEvent)]
        public void HandleVoorraadVerhoogd(VoorraadVerhoogdEvent evt)
        {
            VoorraadMagazijn voorraadMagazijn = _voorraadRepository.GetByArtikelNummer(evt.Artikelnummer);
            voorraadMagazijn.Voorraad = evt.NieuweVoorraad;
            voorraadMagazijn.VoorraadBesteld = false;
            _voorraadRepository.Update(voorraadMagazijn);
        }
    }
}
