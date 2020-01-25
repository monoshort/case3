using FrontendService.Constants;
using FrontendService.Events;
using FrontendService.Models;
using FrontendService.Repositories.Abstractions;
using Minor.Miffy.MicroServices.Events;

namespace FrontendService.Listeners
{
    public class MagazijnEventListeners
    {
        private readonly IArtikelRepository _artikelRepository;

        public MagazijnEventListeners(IArtikelRepository artikelRepository)
        {
            _artikelRepository = artikelRepository;
        }

        [EventListener]
        [Topic(TopicNames.VoorraadVerhoogdEvent)]
        public void HandleVoorraadVerhoogd(VoorraadVerhoogdEvent @event)
        {
            Artikel artikel = _artikelRepository.GetByArtikelnummer(@event.Artikelnummer);
            artikel.Voorraad = @event.NieuweVoorraad;
            _artikelRepository.Update(artikel);
        }

        [EventListener]
        [Topic(TopicNames.VoorraadVerlaagdEvent)]
        public void HandleVoorraadVerlaagd(VoorraadVerlaagdEvent @event)
        {
            Artikel artikel = _artikelRepository.GetByArtikelnummer(@event.Artikelnummer);
            artikel.Voorraad = @event.NieuweVoorraad;
            _artikelRepository.Update(artikel);
        }
    }
}
