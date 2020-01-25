using System;
using BestelService.Constants;
using BestelService.Core.Repositories;
using BestelService.Events;
using BestelService.Seeding.Abstractions;
using Microsoft.Extensions.Logging;
using Minor.Miffy;
using RabbitMQ.Client;

namespace BestelService.Seeding
{
    public class DatabaseCacher : IDatabaseCacher
    {
        private readonly IEventReplayer _eventReplayer;
        private readonly IKlantRepository _klantRepository;
        private readonly ILogger<DatabaseCacher> _logger;

        public DatabaseCacher(IEventReplayer eventReplayer, IKlantRepository klantRepository, ILoggerFactory loggerFactory)
        {
            _eventReplayer = eventReplayer;
            _klantRepository = klantRepository;
            _logger = loggerFactory.CreateLogger<DatabaseCacher>();
        }

        public void EnsureKlanten(IBusContext<IConnection> context)
        {
            if (!_klantRepository.IsEmpty())
            {
                _logger.LogInformation("Klanten found in database, no need to populate database cache");
                return;
            }

            _logger.LogInformation("No klanten found in database, populating database cache");

            _eventReplayer.ReplayEvents(context, TopicNames.NieuweKlantAangemaakt, typeof(NieuweKlantAangemaaktEvent), DateTime.Now);
        }
    }
}
