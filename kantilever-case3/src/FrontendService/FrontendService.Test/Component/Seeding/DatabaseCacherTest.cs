using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http.Testing;
using FrontendService.Agents;
using FrontendService.Agents.Abstractions;
using FrontendService.Constants;
using FrontendService.DAL;
using FrontendService.Events;
using FrontendService.Models;
using FrontendService.Repositories;
using FrontendService.Repositories.Abstractions;
using FrontendService.Seeding;
using FrontendService.Seeding.Abstractions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minor.Miffy.MicroServices.Events;
using Minor.Miffy.TestBus;

namespace FrontendService.Test.Component.Seeding
{
    [TestClass]
    public class DatabaseCacherTest
    {
        private const int MessageIntervalTime = 50;

        private SqliteConnection _connection;
        private DbContextOptions<FrontendContext> _options;

        private HttpTest _httpTest;

        [TestInitialize]
        public void TestInitialize()
        {
            _httpTest = new HttpTest();

            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            _options = new DbContextOptionsBuilder<FrontendContext>()
                .UseSqlite(_connection)
                .Options;

            using FrontendContext context = new FrontendContext(_options);
            context.Database.EnsureCreated();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _httpTest.Dispose();
            _connection.Dispose();
        }

        private IServiceProvider _serviceProvider;

        /// <summary>
        /// Since populating a cacher with dependencies is quite repetitive, this function makes it
        /// easy to just generate one
        /// </summary>
        private IDatabaseCacher GeneratePopulatedController(FrontendContext frontendContext)
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton(frontendContext);
            services.AddSingleton<IHttpAgent, HttpAgent>();
            services.AddSingleton<ICatalogusAgent, CatalogusAgent>();
            services.AddSingleton<IVoorraadAgent, VoorraadAgent>();
            services.AddSingleton<IKlantRepository, KlantRepository>();
            services.AddSingleton<IBestellingRepository, BestellingRepository>();
            services.AddSingleton<IArtikelRepository, ArtikelRepository>();
            services.AddSingleton<ILoggerFactory, NullLoggerFactory>();
            services.AddSingleton<IDatabaseCacher, DatabaseCacher>();
            services.AddSingleton<IAuditAgent, AuditAgent>();
            services.AddSingleton<IEventReplayer, EventReplayer>();

            services.AddSingleton(services);

            _serviceProvider = services.BuildServiceProvider();

            return _serviceProvider.GetRequiredService<IDatabaseCacher>();
        }

        [TestMethod]
        public void EnsureArtikelen_PopulatesDatabaseWithArtikelen()
        {
            // Arrange
            using FrontendContext context = new FrontendContext(_options);

            Environment.SetEnvironmentVariable(EnvNames.CatalogusServiceUrl, "http://catalogus.nl");
            Environment.SetEnvironmentVariable(EnvNames.VoorraadServiceUrl, "http://voorraad.nl");
            Environment.SetEnvironmentVariable(EnvNames.AuditLoggerUrl, "http://auditlogger");

            IDatabaseCacher databaseCacher = GeneratePopulatedController(context);

            List<Artikel> artikelen = new List<Artikel>
            {
                new Artikel
                {
                    Artikelnummer = 5,
                    Beschrijving = "Artikel 1"
                },
                new Artikel
                {
                    Artikelnummer = 10481,
                    Beschrijving = "Artikel 2"
                },
                new Artikel
                {
                    Artikelnummer = 10495820,
                    Beschrijving = "Artikel 3"
                }
            };

            _httpTest.RespondWithJson(artikelen);

            List<VoorraadMagazijn> voorraadMagazijns = new List<VoorraadMagazijn>
            {
                new VoorraadMagazijn
                {
                    ArtikelNummer = 10495820,
                    Voorraad = 12
                },
                new VoorraadMagazijn
                {
                    ArtikelNummer = 5,
                    Voorraad = 2
                },
                new VoorraadMagazijn
                {
                    ArtikelNummer = 10481,
                    Voorraad = 23
                }
            };

            _httpTest.RespondWithJson(voorraadMagazijns);

            // Act
            databaseCacher.EnsureArtikelen();

            // Assert
            using FrontendContext resultContext = new FrontendContext(_options);
            Artikel[] resultArtikelen = resultContext.Artikelen.ToArray();
            Assert.AreEqual(3, resultArtikelen.Length);

            Artikel artikel1 = resultArtikelen.FirstOrDefault(e => e.Beschrijving == "Artikel 1");
            Assert.AreEqual(2, artikel1.Voorraad);
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(2)]
        [DataRow(4)]
        [DataRow(10)]
        [DataRow(20)]
        public void EnsureKlanten_SavesAllKlanten(int amount)
        {
            // Arrange
            using FrontendContext context = new FrontendContext(_options);
            TestBusContext busContext = new TestBusContext();

            Environment.SetEnvironmentVariable(EnvNames.CatalogusServiceUrl, "http://catalogus.nl");
            Environment.SetEnvironmentVariable(EnvNames.VoorraadServiceUrl, "http://voorraad.nl");
            Environment.SetEnvironmentVariable(EnvNames.AuditLoggerUrl, "http://auditlogger");

            IDatabaseCacher databaseCacher = GeneratePopulatedController(context);

            NieuweKlantAangemaaktEvent[] events = Enumerable.Range(0, amount).Select(b => new NieuweKlantAangemaaktEvent
            {
                Klant = new Klant { Factuuradres = new Adres() }
            }).ToArray();

            _httpTest.RespondWith(amount.ToString());

            IEventReplayer eventReplayer = _serviceProvider.GetRequiredService<IEventReplayer>();

            IEventPublisher eventPublisher = new EventPublisher(busContext);

            eventReplayer.StartedReplaying += () =>
            {
                foreach (NieuweKlantAangemaaktEvent @event in events)
                {
                    eventPublisher.Publish(@event);

                    Thread.Sleep(MessageIntervalTime);
                }
            };

            // Act
            databaseCacher.EnsureKlanten(busContext);

            // Assert
            Assert.AreEqual(events.Length, context.Klanten.Count());
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(2)]
        [DataRow(4)]
        [DataRow(10)]
        [DataRow(20)]
        public void EnsureBestellingen_SavesAllBestellingen(int amount)
        {
            // Arrange
            using FrontendContext context = new FrontendContext(_options);
            TestBusContext busContext = new TestBusContext();

            Environment.SetEnvironmentVariable(EnvNames.CatalogusServiceUrl, "http://catalogus.nl");
            Environment.SetEnvironmentVariable(EnvNames.VoorraadServiceUrl, "http://voorraad.nl");
            Environment.SetEnvironmentVariable(EnvNames.AuditLoggerUrl, "http://auditlogger");

            IDatabaseCacher databaseCacher = GeneratePopulatedController(context);

            Klant klant = new Klant { Id = 1, Factuuradres = new Adres() };
            TestHelpers.InjectData(_options, klant);

            NieuweBestellingAangemaaktEvent[] events = Enumerable.Repeat(new NieuweBestellingAangemaaktEvent
            {
                Bestelling = new Bestelling { Klant = klant }
            }, amount).ToArray();

            _httpTest.RespondWith($"{events.Length}");

            // To ensure the next 2 calls don't error
            _httpTest.RespondWith("0");
            _httpTest.RespondWith("0");

            IEventReplayer eventReplayer = _serviceProvider.GetRequiredService<IEventReplayer>();

            IEventPublisher eventPublisher = new EventPublisher(busContext);

            eventReplayer.StartedReplaying += () =>
            {
                foreach (NieuweBestellingAangemaaktEvent @event in events)
                {
                    eventPublisher.Publish(@event);

                    Thread.Sleep(MessageIntervalTime);
                }
            };

            // Act
            databaseCacher.EnsureBestellingen(busContext);

            // Assert
            Assert.AreEqual(events.Length, context.Bestellingen.Count());
        }
    }
}
