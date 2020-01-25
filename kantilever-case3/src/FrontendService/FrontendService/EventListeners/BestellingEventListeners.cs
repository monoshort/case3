using FrontendService.Constants;
using FrontendService.Events;
using FrontendService.Models;
using FrontendService.Repositories.Abstractions;
using Minor.Miffy.MicroServices.Events;

namespace FrontendService.EventListeners
{
    public class BestellingEventListeners
    {
        private readonly IBestellingRepository _bestellingRepository;
        private readonly IKlantRepository _klantRepository;

        public BestellingEventListeners(IBestellingRepository bestellingRepository, IKlantRepository klantRepository)
        {
            _bestellingRepository = bestellingRepository;
            _klantRepository = klantRepository;
        }

        [EventListener]
        [Topic(TopicNames.NieuweBestellingAangemaakt)]
        public void HandleNieuweBestellingAangemaaktEvent(NieuweBestellingAangemaaktEvent @event)
        {
            Klant klant = _klantRepository.GetById(@event.Bestelling.Klant.Id);

            @event.Bestelling.Klant = klant;

            _bestellingRepository.Add(@event.Bestelling);
        }

        [EventListener]
        [Topic(TopicNames.BestellingGoedgekeurd)]
        public void HandleBestellingGoedgekeurdEvent(BestellingGoedgekeurdEvent @event)
        {
            Bestelling bestelling = _bestellingRepository.GetById(@event.BestellingId);
            bestelling.Goedgekeurd = true;
            _bestellingRepository.Update(bestelling);
        }

        [EventListener]
        [Topic(TopicNames.BestellingAfgekeurd)]
        public void HandleBestellingAfgekeurdEvent(BestellingAfgekeurdEvent @event)
        {
            Bestelling bestelling = _bestellingRepository.GetById(@event.BestellingId);
            bestelling.Afgekeurd = true;
            _bestellingRepository.Update(bestelling);
        }

        [EventListener]
        [Topic(TopicNames.BestellingKlaarGemeld)]
        public void HandleBestellingKlaargemeldEvent(BestellingKlaarGemeldEvent @event)
        {
            Bestelling bestelling = _bestellingRepository.GetById(@event.BestellingId);
            bestelling.KlaarGemeld = true;
            _bestellingRepository.Update(bestelling);
        }
    }
}
