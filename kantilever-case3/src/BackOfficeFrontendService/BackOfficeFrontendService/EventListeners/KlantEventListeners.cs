using BackOfficeFrontendService.Constants;
using BackOfficeFrontendService.Events;
using BackOfficeFrontendService.Repositories.Abstractions;
using Minor.Miffy.MicroServices.Events;

namespace BackOfficeFrontendService.EventListeners
{
    public class KlantEventListeners
    {
        /// <summary>
        /// Klant repository
        /// </summary>
        private readonly IKlantRepository _klantRepository;

        /// <summary>
        /// Instantiate event listener with klant repository
        /// </summary>
        public KlantEventListeners(IKlantRepository klantRepository)
        {
            _klantRepository = klantRepository;
        }

        [EventListener]
        [Topic(TopicNames.NieuweKlantAangemaakt)]
        public void HandleNieuweKlant(NieuweKlantAangemaaktEvent aangemaaktEvent)
        {
            _klantRepository.Add(aangemaaktEvent.Klant);
        }
    }
}
