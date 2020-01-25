using System;
using System.Linq;
using System.Threading.Tasks;
using BackOfficeFrontendService.Agents.Abstractions;
using BackOfficeFrontendService.Constants;
using BackOfficeFrontendService.Events;
using BackOfficeFrontendService.Models;
using BackOfficeFrontendService.Repositories.Abstractions;
using BackOfficeFrontendService.Seeding.Abstractions;
using Microsoft.Extensions.Logging;
using Minor.Miffy;
using RabbitMQ.Client;

namespace BackOfficeFrontendService.Seeding
{
    public class DatabaseCacher : IDatabaseCacher
    {
        private readonly IVoorraadAgent _voorraadAgent;
        private readonly ICatalogusAgent _catalogusAgent;
        private readonly IVoorraadRepository _voorraadRepository;
        private readonly IBestellingRepository _bestellingRepository;
        private readonly IKlantRepository _klantRepository;
        private readonly IEventReplayer _eventReplayer;
        private readonly ILogger<DatabaseCacher> _logger;

        public DatabaseCacher(IVoorraadAgent voorraadAgent,
            ICatalogusAgent catalogusAgent,
            IVoorraadRepository voorraadRepository,
            IBestellingRepository bestellingRepository,
            IKlantRepository klantRepository,
            IEventReplayer eventReplayer,
            ILoggerFactory loggerFactory)
        {
            _voorraadAgent = voorraadAgent;
            _catalogusAgent = catalogusAgent;
            _voorraadRepository = voorraadRepository;
            _bestellingRepository = bestellingRepository;
            _klantRepository = klantRepository;
            _eventReplayer = eventReplayer;
            _logger = loggerFactory.CreateLogger<DatabaseCacher>();
        }

        public void EnsureBestellingen(IBusContext<IConnection> context)
        {
            if (!_bestellingRepository.IsEmpty())
            {
                _logger.LogInformation("Voorraad database contains data, skipping cache");
                return;
            }

            _eventReplayer.ReplayEvents(context, TopicNames.NieuweBestellingAangemaakt, typeof(NieuweBestellingAangemaaktEvent), DateTime.Now);
            _eventReplayer.ReplayEvents(context, TopicNames.BestellingGoedgekeurd, typeof(BestellingGoedgekeurdEvent), DateTime.Now);
            _eventReplayer.ReplayEvents(context, TopicNames.BestellingAfgekeurd, typeof(BestellingAfgekeurdEvent), DateTime.Now);
            _eventReplayer.ReplayEvents(context, TopicNames.BestellingFactuurGeprint, typeof(BestellingFactuurGeprintEvent), DateTime.Now);
            _eventReplayer.ReplayEvents(context, TopicNames.BestellingAdresLabelGeprint, typeof(BestellingAdresLabelGeprintEvent), DateTime.Now);
            _eventReplayer.ReplayEvents(context, TopicNames.BestelRegelIngepakt, typeof(BestelRegelIngepaktEvent), DateTime.Now);
            _eventReplayer.ReplayEvents(context, TopicNames.BestellingKanKlaarGemeldWorden, typeof(BestellingKanKlaarGemeldWordenEvent), DateTime.Now);
            _eventReplayer.ReplayEvents(context, TopicNames.BestellingKlaarGemeld, typeof(BestellingKlaarGemeldEvent), DateTime.Now);
        }

        public void EnsureKlanten(IBusContext<IConnection> context)
        {
            if (!_klantRepository.IsEmpty())
            {
                _logger.LogInformation("Voorraad database contains data, skipping cache");
                return;
            }

            _eventReplayer.ReplayEvents(context, TopicNames.NieuweKlantAangemaakt, typeof(NieuweKlantAangemaaktEvent), DateTime.Now);
        }

        public void EnsureVoorraad()
        {
            if (!_voorraadRepository.IsEmpty())
            {
                _logger.LogInformation("Voorraad database contains data, skipping cache");
                return;
            }

            _logger.LogDebug("No voorraad found, fetching voorraad and catalogus");
            Task<VoorraadMagazijn[]> voorraadTask = _voorraadAgent.GetAllVoorraadAsync();
            Task<Artikel[]> artikelenTask = _catalogusAgent.GetAlleArtikelenAsync();

            Task.WaitAll(voorraadTask, artikelenTask);

            VoorraadMagazijn[] voorraad = voorraadTask.Result;
            Artikel[] artikelen = artikelenTask.Result;

            _logger.LogDebug($"Combining {artikelen.Length} artikelen and {voorraad.Length} voorraad");

            VoorraadMagazijn[] combinedResult = voorraad.Select(e =>
            {
                Artikel artikel = artikelen.Single(a => a.Artikelnummer == e.ArtikelNummer);
                e.Leverancier = artikel.Leverancier;
                e.Leveranciercode = artikel.Leveranciercode;
                return e;
            }).ToArray();

            _logger.LogInformation($"Found {combinedResult.Length} voorraad items, importing...");
            _voorraadRepository.Add(combinedResult);
        }
    }
}
