using System.Linq;
using System.Threading.Tasks;
using BackOfficeFrontendService.Agents.Abstractions;
using BackOfficeFrontendService.Commands;
using BackOfficeFrontendService.Constants;
using BackOfficeFrontendService.Events;
using BackOfficeFrontendService.Models;
using BackOfficeFrontendService.Repositories.Abstractions;
using Minor.Miffy.MicroServices.Events;

namespace BackOfficeFrontendService.EventListeners
{
    public class BestellingEventListeners
    {
        private readonly IBestellingRepository _bestellingRepository;
        private readonly IKlantRepository _klantRepository;
        private readonly IVoorraadAgent _voorraadAgent;
        private readonly IVoorraadRepository _voorraadRepository;

        public BestellingEventListeners(IBestellingRepository bestellingRepository, IKlantRepository klantRepository, IVoorraadAgent voorraadAgent, IVoorraadRepository voorraadRepository)
        {
            _bestellingRepository = bestellingRepository;
            _klantRepository = klantRepository;
            _voorraadAgent = voorraadAgent;
            _voorraadRepository = voorraadRepository;
        }

        [EventListener]
        [Topic(TopicNames.NieuweBestellingAangemaakt)]
        public void HandleBestellingAangemaakt(NieuweBestellingAangemaaktEvent @event)
        {
            Klant klant = _klantRepository.FindById(@event.Bestelling.Klant.Id);

            @event.Bestelling.Klant = klant;

            foreach (var bestelRegel in @event.Bestelling.BestelRegels)
            {
                bestelRegel.Voorraad = _voorraadRepository.GetByArtikelNummer(bestelRegel.ArtikelNummer);
            }

            _bestellingRepository.Add(@event.Bestelling);
        }

        [EventListener]
        [Topic(TopicNames.BestellingGoedgekeurd)]
        public void HandleBestellingGoedgekeurd(BestellingGoedgekeurdEvent @event)
        {
            Bestelling bestelling = _bestellingRepository.GetInpakOpdrachtMetId(@event.BestellingId);
            bestelling.Goedgekeurd = true;
            _bestellingRepository.Update(bestelling);
        }

        [EventListener]
        [Topic(TopicNames.BestellingAfgekeurd)]
        public void HandleBestellingAfgekeurd(BestellingAfgekeurdEvent @event)
        {
            Bestelling bestelling = _bestellingRepository.GetInpakOpdrachtMetId(@event.BestellingId);
            bestelling.Afgekeurd = true;
            _bestellingRepository.Update(bestelling);
        }

        [EventListener]
        [Topic(TopicNames.BestellingKlaarGemeld)]
        public void HandleBestellingKlaargemeld(BestellingKlaarGemeldEvent @event)
        {
            Bestelling bestelling = _bestellingRepository.GetInpakOpdrachtMetId(@event.BestellingId);
            bestelling.KlaarGemeld = true;
            _bestellingRepository.Update(bestelling);

            Parallel.ForEach(bestelling.BestelRegels, bestelRegel =>
            {
                HaalVoorraadUitMagazijnCommand command = new HaalVoorraadUitMagazijnCommand
                {
                    Artikelnummer = bestelRegel.ArtikelNummer,
                    Aantal = bestelRegel.Voorraad.Voorraad - bestelRegel.Aantal
                };

                _voorraadAgent.HaalVoorraadUitMagazijnAsync(command);
            });
        }

        [EventListener]
        [Topic(TopicNames.BestelRegelIngepakt)]
        public void HandleBestelRegelIngepakt(BestelRegelIngepaktEvent @event)
        {
            Bestelling bestelling = _bestellingRepository.GetInpakOpdrachtMetId(@event.BestellingId);
            BestelRegel regel = bestelling.BestelRegels.Single(rl => rl.Id == @event.BestelRegelId);
            regel.Ingepakt = true;
            _bestellingRepository.Update(bestelling);
        }

        [EventListener]
        [Topic(TopicNames.BestellingFactuurGeprint)]
        public void HandleBestellingFactuurGeprint(BestellingFactuurGeprintEvent @event)
        {
            Bestelling bestelling = _bestellingRepository.GetInpakOpdrachtMetId(@event.BestellingId);
            bestelling.FactuurGeprint = true;
            _bestellingRepository.Update(bestelling);
        }

        [EventListener]
        [Topic(TopicNames.BestellingAdresLabelGeprint)]
        public void HandleBestellingAdresLabelGeprint(BestellingAdresLabelGeprintEvent @event)
        {
            Bestelling bestelling = _bestellingRepository.GetInpakOpdrachtMetId(@event.BestellingId);
            bestelling.AdresLabelGeprint = true;
            _bestellingRepository.Update(bestelling);
        }

        [EventListener]
        [Topic(TopicNames.BestellingKanKlaarGemeldWorden)]
        public void HandleKanKlaarGemeldWorden(BestellingKanKlaarGemeldWordenEvent @event)
        {
            Bestelling bestelling = _bestellingRepository.GetInpakOpdrachtMetId(@event.BestellingId);
            bestelling.KanKlaarGemeldWorden = true;
            _bestellingRepository.Update(bestelling);
        }

        [EventListener]
        [Topic(TopicNames.BetalingGeregistreerd)]
        public void HandleBetalingGeregistreerd(BetalingGeregistreerdEvent @event)
        {
            Bestelling bestelling = _bestellingRepository.GetInpakOpdrachtMetId(@event.BestellingId);
            bestelling.OpenstaandBedrag = @event.OpenstaandBedrag;
            _bestellingRepository.Update(bestelling);
        }

        [EventListener]
        [Topic(TopicNames.KlantIsWanbetalerGeworden)]
        public void HandleKlantIsWanbetalerGeworden(KlantIsWanbetalerGewordenEvent @event)
        {
            Bestelling bestelling = _bestellingRepository.GetInpakOpdrachtMetId(@event.BestellingId);
            bestelling.IsKlantWanbetaler = true;
            _bestellingRepository.Update(bestelling);
        }
    }
}
