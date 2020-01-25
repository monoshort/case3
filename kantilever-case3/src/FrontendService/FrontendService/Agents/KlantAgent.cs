using System.Threading.Tasks;
using FrontendService.Agents.Abstractions;
using FrontendService.Commands;
using FrontendService.Models;
using Minor.Miffy.MicroServices.Commands;

namespace FrontendService.Agents
{
    public class KlantAgent : IKlantAgent
    {
        private readonly ICommandPublisher _commandPublisher;

        public KlantAgent(ICommandPublisher commandPublisher)
        {
            _commandPublisher = commandPublisher;
        }
        
        public async Task<Klant> MaakKlantAanAsync(Klant klant)
        {
            MaakNieuweKlantAanCommand command = new MaakNieuweKlantAanCommand
            {
                Klant = klant
            };

            MaakNieuweKlantAanCommand returnedCommand =
                await _commandPublisher.PublishAsync<MaakNieuweKlantAanCommand>(command);

            return returnedCommand.Klant;
        }
    }
}