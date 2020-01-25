using FrontendService.Constants;
using FrontendService.Events;
using FrontendService.Repositories.Abstractions;
using Minor.Miffy.MicroServices.Events;

namespace FrontendService.EventListeners
{
    public class KlantEventListeners
    {
        private readonly IKlantRepository _klantRepository;

        public KlantEventListeners(IKlantRepository klantRepository)
        {
            _klantRepository = klantRepository;
        }

        [EventListener]
        [Topic(TopicNames.NieuweKlantAangemaakt)]
        public void HandleNieuweKlantAangemaaktEvent(NieuweKlantAangemaaktEvent @event)
        {
            _klantRepository.Add(@event.Klant);
        }
    }
}
