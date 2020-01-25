using System.Collections.Generic;
using BestelService.Commands;
using BestelService.Constants;
using BestelService.Core.Models;
using BestelService.Core.Repositories;
using BestelService.Services.Services.Abstractions;
using Minor.Miffy.MicroServices.Commands;

namespace BestelService.Listeners
{
    public class BestellingCommandListener
    {
        private readonly IBestellingService _bestellingService;
        private readonly IKlantRepository _klantRepository;

        public BestellingCommandListener(IBestellingService bestellingService, IKlantRepository klantRepository)
        {
            _bestellingService = bestellingService;
            _klantRepository = klantRepository;
        }

        [CommandListener(QueueNames.MaakNieuweBestellingAan)]
        public MaakNieuweBestellingAanCommand HandleNieuweBestelling(MaakNieuweBestellingAanCommand command)
        {
            Klant klant = _klantRepository.GetById(command.Bestelling.Klant.Id);

            command.Bestelling.Klant = klant;

            _bestellingService.MaakBestellingAan(command.Bestelling);
            return command;
        }

        [CommandListener(QueueNames.KeurBestellingGoed)]
        public KeurBestellingGoedCommand HandleKeurBestellingGoed(KeurBestellingGoedCommand command)
        {
            _bestellingService.KeurBestellingGoed(command.BestellingId);
            return command;
        }

        [CommandListener(QueueNames.KeurBestellingAf)]
        public KeurBestellingAfCommand HandleKeurBestellingAf(KeurBestellingAfCommand command)
        {
            _bestellingService.KeurBestellingAf(command.BestellingId);
            return command;
        }

        [CommandListener(QueueNames.MeldBestellingKlaar)]
        public MeldBestellingKlaarCommand HandleMeldBestellingKlaar(MeldBestellingKlaarCommand command)
        {
            _bestellingService.MeldBestellingKlaar(command.BestellingId);
            return command;
        }

        [CommandListener(QueueNames.PakBestelRegelIn)]
        public PakBestelRegelInCommand HandlePakBestelRegelIn(PakBestelRegelInCommand command)
        {
            _bestellingService.PakBestelRegelIn(command.BestellingId, command.BestelRegelId);
            return command;
        }

        [CommandListener(QueueNames.PrintFactuur)]
        public PrintFactuurCommand HandlePrintFactuur(PrintFactuurCommand command)
        {
            _bestellingService.PrintFactuur(command.BestellingId);
            return command;
        }

        [CommandListener(QueueNames.PrintAdresLabel)]
        public PrintAdresLabelCommand HandlePrintAdresLabel(PrintAdresLabelCommand command)
        {
            _bestellingService.PrintAdresLabel(command.BestellingId);
            return command;
        }

        [CommandListener(QueueNames.RegistreerBetaling)]
        public RegistreerBetalingCommand HandleRegistreerBetaling(RegistreerBetalingCommand command)
        {
            _bestellingService.RegistreerBetaling(command.BestellingNummer, command.BetaaldBedrag);
            return command;
        }

        [CommandListener(QueueNames.ControleerOfErWanbetalingenZijn)]
        public IEnumerable<Bestelling> HandleWanbetalingenZijnCommand(ControleerOfErWanbetalingenZijnCommand command)
        {
            return _bestellingService.ControleerOpWanbetalingen();
        }
    }
}
