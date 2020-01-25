using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrontendService.Agents.Abstractions;
using FrontendService.Constants;
using FrontendService.Events;
using FrontendService.Models;
using FrontendService.Repositories.Abstractions;
using FrontendService.Seeding.Abstractions;
using Microsoft.Extensions.Logging;
using Minor.Miffy;
using RabbitMQ.Client;

namespace FrontendService.Seeding
{
    public class DatabaseCacher : IDatabaseCacher
    {
        /// <summary>
        /// Agent to communicate with the catalogus
        /// </summary>
        private readonly ICatalogusAgent _catalogusAgent;

        /// <summary>
        /// Agent to communicate with the voorraad
        /// </summary>
        private readonly IVoorraadAgent _voorraadAgent;

        /// <summary>
        /// Repository that persists artikelen
        /// </summary>
        private readonly IArtikelRepository _artikelRepository;

        /// <summary>
        /// Repository that persists klanten
        /// </summary>
        private readonly IKlantRepository _klantRepository;

        /// <summary>
        /// Repository that persists bestellingen
        /// </summary>
        private readonly IBestellingRepository _bestellingRepository;

        /// <summary>
        /// Replayer to replay events
        /// </summary>
        private readonly IEventReplayer _eventReplayer;

        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger<DatabaseCacher> _logger;

        /// <summary>
        /// Initialize a database cacher with its required dependencies
        /// </summary>
        public DatabaseCacher(IArtikelRepository artikelRepository,
            ICatalogusAgent catalogusAgent,
            IVoorraadAgent voorraadAgent,
            IKlantRepository klantRepository,
            IBestellingRepository bestellingRepository,
            IEventReplayer eventReplayer,
            ILoggerFactory loggerFactory)
        {
            _catalogusAgent = catalogusAgent;
            _voorraadAgent = voorraadAgent;
            _artikelRepository = artikelRepository;
            _bestellingRepository = bestellingRepository;
            _klantRepository = klantRepository;
            _eventReplayer = eventReplayer;
            _logger = loggerFactory.CreateLogger<DatabaseCacher>();
        }

        /// <summary>
        /// Ensure klantdata
        /// </summary>
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

        /// <summary>
        /// Ensure bestellingdata+
        /// </summary>
        public void EnsureBestellingen(IBusContext<IConnection> context)
        {
            if (!_bestellingRepository.IsEmpty())
            {
                _logger.LogInformation("Bestellingen found in database, no need to populate database cache");
                return;
            }

            _logger.LogInformation("No bestellingen found in database, populating database cache");

            _eventReplayer.ReplayEvents(context, TopicNames.NieuweBestellingAangemaakt, typeof(NieuweBestellingAangemaaktEvent), DateTime.Now);
            _eventReplayer.ReplayEvents(context, TopicNames.BestellingGoedgekeurd, typeof(BestellingGoedgekeurdEvent), DateTime.Now);
            _eventReplayer.ReplayEvents(context, TopicNames.BestellingAfgekeurd, typeof(BestellingAfgekeurdEvent), DateTime.Now);
        }

        /// <summary>
        /// Ensure artikeldata
        /// </summary>
        public void EnsureArtikelen()
        {
            if (!_artikelRepository.IsEmpty())
            {
                _logger.LogInformation("Artikelen found in database, no need to populate database cache");
                return;
            }

            _logger.LogInformation("No artikelen found in database, populating database cache");

            Task<IEnumerable<Artikel>> artikelenTask = _catalogusAgent.GetAlleArtikelenAsync();
            Task<IEnumerable<VoorraadMagazijn>> voorraadTask = _voorraadAgent.GetAllVoorraadAsync();

            Task.WaitAll(artikelenTask, voorraadTask);

            _logger.LogInformation(
                $"Fetched {artikelenTask.Result.Count()} artikelen and {voorraadTask.Result.Count()} voorraad from remote");

            Artikel[] artikelenMetVoorraad =
                UpdateArtikelenWithVooraad(artikelenTask.Result, voorraadTask.Result).ToArray();

            _logger.LogDebug("Adding fetched data to database");
            _artikelRepository.Add(artikelenMetVoorraad);
        }

        /// <summary>
        /// Glue artikelen and voorraad together
        /// </summary>
        private IEnumerable<Artikel> UpdateArtikelenWithVooraad(IEnumerable<Artikel> artikelen, IEnumerable<VoorraadMagazijn> voorraaden)
        {
            _logger.LogTrace("Sorting voorraad");
            IEnumerable<VoorraadMagazijn> sortedVoorraad = voorraaden.OrderBy(e => e.ArtikelNummer);

            _logger.LogDebug("Joining voorraaden and artikelen together");
            return artikelen
                .OrderBy(e => e.Artikelnummer)
                .Select(artikel =>
                    {
                        artikel.Voorraad = sortedVoorraad.SingleOrDefault(voorraad => artikel.Artikelnummer == voorraad.ArtikelNummer)?.Voorraad ?? 0;
                        return artikel;
                    });
        }
    }
}
