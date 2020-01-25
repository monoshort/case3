using BestelService.Constants;
using BestelService.Core.Repositories;
using BestelService.Events;
using Minor.Miffy.MicroServices.Events;

namespace BestelService.Listeners
{
    public class KlantEventListener
    {
        private readonly IKlantRepository _klantRepository;

        public KlantEventListener(IKlantRepository klantRepository)
        {
            _klantRepository = klantRepository;
        }

        [EventListener]
        [Topic(TopicNames.NieuweKlantAangemaakt)]
        public void HandleNieuweKlantAangemaakt(NieuweKlantAangemaaktEvent @event)
        {
            _klantRepository.Add(@event.Klant);
        }
    }
}
