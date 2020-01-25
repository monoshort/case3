using FrontendService.Agents.Abstractions;
using FrontendService.Commands;
using FrontendService.Models;
using Minor.Miffy.MicroServices.Commands;

namespace FrontendService.Agents
{
    public class BestellingAgent : IBestellingAgent
    {
        private readonly ICommandPublisher _publisher;

        public BestellingAgent(ICommandPublisher commandPublisher)
        {
            _publisher = commandPublisher;
        }

        /// <inheritdoc/>
        public void Bestel(Bestelling bestelling)
        {
            var bestellingCommand = new MaakNieuweBestellingAanCommand
            {
                Bestelling = bestelling
            };

            _publisher.PublishAsync<MaakNieuweBestellingAanCommand>(bestellingCommand);
        }
    }
}
