using KlantService.Commands;
using KlantService.Constants;
using KlantService.Events;
using KlantService.Repositories;
using Minor.Miffy.MicroServices.Commands;
using Minor.Miffy.MicroServices.Events;

namespace KlantService.Listeners
{
    public class KlantListeners
    {
        /// <summary>
        /// Repository to persist klanten
        /// </summary>
        private readonly IKlantRepository _klantRepository;

        /// <summary>
        /// Publisher to publish events
        /// </summary>
        private readonly IEventPublisher _eventPublisher;

        /// <summary>
        /// Instantiate a listener with a klanten repository
        /// </summary>
        public KlantListeners(IKlantRepository repository, IEventPublisher eventPublisher)
        {
            _klantRepository = repository;
            _eventPublisher = eventPublisher;
        }

        /// <summary>
        /// Handle nieuwe klant commands
        /// </summary>
        [CommandListener(QueueNames.MaakNieuweKlantAan)]
        public MaakNieuweKlantAanCommand HandleNieuwKlantCommand(MaakNieuweKlantAanCommand command)
        {
            _klantRepository.Add(command.Klant);

            NieuweKlantAangemaaktEvent aangemaaktEvent = new NieuweKlantAangemaaktEvent
            {
                Klant = command.Klant
            };

            _eventPublisher.PublishAsync(aangemaaktEvent);
            return command;
        }
    }
}
