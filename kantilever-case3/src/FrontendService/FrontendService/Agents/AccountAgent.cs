using System.Threading.Tasks;
using FrontendService.Agents.Abstractions;
using FrontendService.Commands;
using Minor.Miffy.MicroServices.Commands;

namespace FrontendService.Agents
{
    public class AccountAgent : IAccountAgent
    {
        private readonly ICommandPublisher _commandPublisher;

        public AccountAgent(ICommandPublisher commandPublisher)
        {
            _commandPublisher = commandPublisher;
        }

        /// <inheritdoc/>
        public async Task MaakAccountAanAsync(string username, string password)
        {
            MaakAccountAanCommand command = new MaakAccountAanCommand
            {
                Username = username,
                Password = password
            };

            await _commandPublisher.PublishAsync<MaakAccountAanCommand>(command);
        }

        /// <inheritdoc/>
        public async Task VerwijderAccountAsync(string username)
        {
            VerwijderAccountCommand command = new VerwijderAccountCommand
            {
                Username = username
            };

            await _commandPublisher.PublishAsync<VerwijderAccountCommand>(command);
        }
    }
}
