using System.Collections.Generic;
using BestelService.Core.Models;
using BestelService.Core.Repositories;
using BestelService.Services.Events;
using BestelService.Services.Services.Abstractions;
using Minor.Miffy.MicroServices.Events;

namespace BestelService.Services.Services
{
    public class BestellingService : IBestellingService
    {
        private readonly IBestelRepository _bestelRepository;
        private readonly IEventPublisher _eventPublisher;

        public BestellingService(IBestelRepository bestelRepository, IEventPublisher eventPublisher)
        {
            _bestelRepository = bestelRepository;
            _eventPublisher = eventPublisher;
        }

        /// <inheritdoc />
        public void MaakBestellingAan(Bestelling bestelling)
        {
            _bestelRepository.Add(bestelling);

            bestelling.VoegBestelnummerToe();

            bestelling.ControleerOfBestellingAutomatischGoedgekeurdKanWorden();

            _bestelRepository.Update(bestelling);

            var @event = new NieuweBestellingAangemaaktEvent { Bestelling = bestelling };
            _eventPublisher.PublishAsync(@event);
        }

        /// <inheritdoc />
        public void KeurBestellingGoed(long bestellingId)
        {
            Bestelling dbBestelling = _bestelRepository.GetById(bestellingId);

            dbBestelling.KeurGoed();

            _bestelRepository.Update(dbBestelling);

            var @event = new BestellingGoedgekeurdEvent { BestellingId = dbBestelling.Id };
            _eventPublisher.PublishAsync(@event);
        }

        /// <inheritdoc />
        public void KeurBestellingAf(long bestellingId)
        {
            Bestelling dbBestelling = _bestelRepository.GetById(bestellingId);

            dbBestelling.KeurAf();

            _bestelRepository.Update(dbBestelling);

            var @event = new BestellingAfgekeurdEvent { BestellingId = dbBestelling.Id };
            _eventPublisher.PublishAsync(@event);
        }

        /// <inheritdoc />
        public void MeldBestellingKlaar(long bestellingId)
        {
            Bestelling dbBestelling = _bestelRepository.GetById(bestellingId);

            dbBestelling.MeldKlaar();

            _bestelRepository.Update(dbBestelling);

            var @event = new BestellingKlaarGemeldEvent { BestellingId = dbBestelling.Id };
            _eventPublisher.PublishAsync(@event);
        }

        /// <inheritdoc />
        public void PakBestelRegelIn(long bestellingId, long bestelRegelId)
        {
            Bestelling dbBestelling = _bestelRepository.GetById(bestellingId);

            dbBestelling.BestellingKanKlaargemeldWorden += (bestelling, args) =>
            {
                var @klaarEvent = new BestellingKanKlaarGemeldWordenEvent
                {
                    BestellingId = bestelling.Id
                };
                _eventPublisher.PublishAsync(@klaarEvent);
            };

            dbBestelling.PakIn(bestelRegelId);

            _bestelRepository.Update(dbBestelling);

            var @event = new BestelRegelIngepaktEvent { BestelRegelId = bestelRegelId, BestellingId = bestellingId };
            _eventPublisher.PublishAsync(@event);
        }

        /// <inheritdoc />
        public void PrintFactuur(long bestellingId)
        {
            Bestelling dbBestelling = _bestelRepository.GetById(bestellingId);

            dbBestelling.BestellingKanKlaargemeldWorden += (bestelling, args) =>
            {
                var @klaarEvent = new BestellingKanKlaarGemeldWordenEvent
                {
                    BestellingId = bestelling.Id
                };
                _eventPublisher.PublishAsync(@klaarEvent);
            };

            dbBestelling.PrintFactuur();

            _bestelRepository.Update(dbBestelling);

            var @event = new BestellingFactuurGeprintEvent { BestellingId = dbBestelling.Id };
            _eventPublisher.PublishAsync(@event);
        }

        /// <inheritdoc />
        public void PrintAdresLabel(long bestellingId)
        {
            Bestelling dbBestelling = _bestelRepository.GetById(bestellingId);

            dbBestelling.BestellingKanKlaargemeldWorden += (bestelling, args) =>
                {
                    var @klaarEvent = new BestellingKanKlaarGemeldWordenEvent
                    {
                        BestellingId = bestelling.Id
                    };
                    _eventPublisher.PublishAsync(@klaarEvent);
                };

            dbBestelling.PrintAdresLabel();

            _bestelRepository.Update(dbBestelling);

            var @event = new BestellingAdresLabelGeprintEvent { BestellingId = dbBestelling.Id };
            _eventPublisher.PublishAsync(@event);
        }

        /// <inheritdoc />
        public void RegistreerBetaling(string bestellingNummer, decimal betaaldBedrag)
        {
            Bestelling bestelling = _bestelRepository.GetByBestellingNummer(bestellingNummer);
            bestelling.BoekBedragAf(betaaldBedrag);

            _bestelRepository.Update(bestelling);

            var @event = new BetalingGeregistreerdEvent
            {
                BestellingId = bestelling.Id,
                OpenstaandBedrag = bestelling.OpenstaandBedrag
            };
            _eventPublisher.PublishAsync(@event);

            Bestelling ongekeurdeBestelling = _bestelRepository.GetMostRecentUnassessedBestelling();

            if (ongekeurdeBestelling == null)
            {
                return;
            }

            ongekeurdeBestelling.ControleerOfBestellingAutomatischGoedgekeurdKanWorden();

            if (!ongekeurdeBestelling.Goedgekeurd)
            {
                return;
            }

            BestellingGoedgekeurdEvent goedgekeurdEvent = new BestellingGoedgekeurdEvent
            {
                BestellingId = ongekeurdeBestelling.Id,
            };

            _eventPublisher.PublishAsync(goedgekeurdEvent);
        }

        /// <inheritdoc />
        public IEnumerable<Bestelling> ControleerOpWanbetalingen()
        {
            IEnumerable<Bestelling> bestellingen = _bestelRepository.GetAll();

            List<Bestelling> newWanbetalers = new List<Bestelling>();

            foreach (Bestelling bestelling in bestellingen)
            {
                bestelling.BestellingKlantIsWanbetalerGeworden += (subject, args) =>
                {
                    KlantIsWanbetalerGewordenEvent evt = new KlantIsWanbetalerGewordenEvent
                    {
                        BestellingId = bestelling.Id
                    };

                    _eventPublisher.PublishAsync(evt);

                    newWanbetalers.Add(subject);
                };

                bestelling.ControleerOfKlantWanbetalerIs();

                _bestelRepository.Update(bestelling);
            }

            return newWanbetalers;
        }
    }
}
