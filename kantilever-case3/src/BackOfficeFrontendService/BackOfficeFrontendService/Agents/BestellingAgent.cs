using System.Collections.Generic;
using System.Threading.Tasks;
using BackOfficeFrontendService.Agents.Abstractions;
using BackOfficeFrontendService.Commands;
using BackOfficeFrontendService.Constants;
using BackOfficeFrontendService.Exceptions;
using BackOfficeFrontendService.Models;
using Minor.Miffy;
using Minor.Miffy.MicroServices.Commands;

namespace BackOfficeFrontendService.Agents
{
    public class BestellingAgent : IBestellingAgent
    {
        private readonly ICommandPublisher _commandPublisher;

        public BestellingAgent(ICommandPublisher publisher)
        {
            _commandPublisher = publisher;
        }

        /// <inheritdoc/>
        public async Task KeurBestellingGoedAsync(long bestellingId)
        {
            KeurBestellingGoedCommand command = new KeurBestellingGoedCommand
            {
                BestellingId = bestellingId
            };

            try
            {
                await _commandPublisher.PublishAsync<KeurBestellingGoedCommand>(command);
            }
            catch (DestinationQueueException e)
            {
                if (e.InnerException.Message == "Sequence contains no elements")
                {
                    throw new FunctionalException(FunctionalExceptionMessages.BestellingNotFound);
                }
            }
        }

        /// <inheritdoc/>
        public async Task KeurBestellingAfAsync(long bestellingId)
        {
            KeurBestellingAfCommand command = new KeurBestellingAfCommand
            {
                BestellingId = bestellingId
            };

            try
            {
                await _commandPublisher.PublishAsync<KeurBestellingAfCommand>(command);
            }
            catch (DestinationQueueException e)
            {
                if (e.InnerException.Message == "Sequence contains no elements")
                {
                    throw new FunctionalException(FunctionalExceptionMessages.BestellingNotFound);
                }
            }
        }

        /// <inheritdoc/>
        public async Task PakBestelregelInAsync(long bestellingId, long bestelregelId)
        {
            PakBestelRegelInCommand command = new PakBestelRegelInCommand
            {
                BestellingId = bestellingId,
                BestelRegelId = bestelregelId
            };

            await _commandPublisher.PublishAsync<PakBestelRegelInCommand>(command);
        }

        /// <inheritdoc/>
        public async Task MeldBestellingKlaarAsync(long bestellingId)
        {
            MeldBestellingKlaarCommand command = new MeldBestellingKlaarCommand
            {
                BestellingId = bestellingId
            };

            await _commandPublisher.PublishAsync<MeldBestellingKlaarCommand>(command);
        }

        /// <inheritdoc/>
        public async Task BestellingPrintFactuurAsync(long bestellingId)
        {
            PrintFactuurCommand @event = new PrintFactuurCommand
            {
                BestellingId = bestellingId
            };

            await _commandPublisher.PublishAsync<PrintFactuurCommand>(@event);
        }

        /// <inheritdoc/>
        public async Task BestellingPrintAdresLabelAsync(long bestellingId)
        {
            PrintAdresLabelCommand @event = new PrintAdresLabelCommand
            {
                BestellingId = bestellingId
            };

            await _commandPublisher.PublishAsync<PrintAdresLabelCommand>(@event);
        }

        /// <inheritdoc/>
        public async Task RegistreerBetalingAsync(string bestellingNummer, decimal betaaldBedrag)
        {
            RegistreerBetalingCommand command = new RegistreerBetalingCommand
            {
                BestellingNummer = bestellingNummer,
                BetaaldBedrag = betaaldBedrag
            };
            await _commandPublisher.PublishAsync<RegistreerBetalingCommand>(command);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Bestelling>> ControleerOfErWanbetalersZijnAsync()
        {
            return await _commandPublisher.PublishAsync<IEnumerable<Bestelling>>(
                new ControleerOfErWanbetalingenZijnCommand());
        }
    }
}
