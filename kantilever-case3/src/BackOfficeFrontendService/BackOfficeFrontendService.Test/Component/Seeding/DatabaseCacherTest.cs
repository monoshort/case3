using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BackOfficeFrontendService.Agents;
using BackOfficeFrontendService.Agents.Abstractions;
using BackOfficeFrontendService.Constants;
using BackOfficeFrontendService.DAL;
using BackOfficeFrontendService.Events;
using BackOfficeFrontendService.Models;
using BackOfficeFrontendService.Repositories;
using BackOfficeFrontendService.Repositories.Abstractions;
using BackOfficeFrontendService.Seeding;
using BackOfficeFrontendService.Seeding.Abstractions;
using Flurl.Http.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minor.Miffy;
using Minor.Miffy.MicroServices.Events;
using Minor.Miffy.TestBus;
using RabbitMQ.Client;

namespace BackOfficeFrontendService.Test.Component.Seeding
{
    [TestClass]
    public class DatabaseCacherTest
    {
        private const int MessageIntervalTime = 50;

        private HttpTest _httpTest;

        private SqliteConnection _connection;
        private DbContextOptions<BackOfficeContext> _options;

        [TestInitialize]
        public void TestInitialize()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            _options = new DbContextOptionsBuilder<BackOfficeContext>()
                .UseSqlite(_connection)
                .Options;

            using BackOfficeContext context = new BackOfficeContext(_options);
            context.Database.EnsureCreated();

            Environment.SetEnvironmentVariable(EnvNames.VoorraadServiceUrl, "http://voorraad.nl");
            Environment.SetEnvironmentVariable(EnvNames.CatalogusServiceUrl, "http://catalogus.nl");
            Environment.SetEnvironmentVariable(EnvNames.AuditLoggerUrl, "http://auditlogger");

            _httpTest = new HttpTest();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _httpTest.Dispose();
            _connection.Close();
        }

        private IServiceProvider _serviceProvider;

        /// <summary>
        /// Get a cacher instance with all dependencies
        /// </summary>
        public IDatabaseCacher GetPopulatedInstance(IBusContext<IConnection> busContext,
            BackOfficeContext context)
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(busContext);
            serviceCollection.AddSingleton(context);
            serviceCollection.AddSingleton<IDatabaseCacher, DatabaseCacher>();
            serviceCollection.AddSingleton<IVoorraadAgent, VoorraadAgent>();
            serviceCollection.AddSingleton<ICatalogusAgent, CatalogusAgent>();
            serviceCollection.AddSingleton<IVoorraadRepository, VoorraadRepository>();
            serviceCollection.AddSingleton<IKlantRepository, KlantRepository>();
            serviceCollection.AddSingleton<IBestellingRepository, BestellingRepository>();
            serviceCollection.AddSingleton<IHttpAgent, HttpAgent>();
            serviceCollection.AddSingleton<IAuditAgent, AuditAgent>();
            serviceCollection.AddSingleton<IEventPublisher, EventPublisher>();
            serviceCollection.AddSingleton<IEventReplayer, EventReplayer>();
            serviceCollection.AddSingleton<ILoggerFactory, NullLoggerFactory>();
            serviceCollection.AddSingleton(serviceCollection);

            _serviceProvider = serviceCollection.BuildServiceProvider();

            return _serviceProvider.GetRequiredService<IDatabaseCacher>();
        }

        [TestMethod]
        public void EnsureVoorraad_ProperlyFillsDatabase()
        {
            // Arrange
            using BackOfficeContext context = new BackOfficeContext(_options);
            IDatabaseCacher databaseCacher = GetPopulatedInstance(new TestBusContext(), context);

            VoorraadMagazijn[] data = {
                new VoorraadMagazijn {ArtikelNummer = 123, Voorraad = 20},
                new VoorraadMagazijn {ArtikelNummer = 5635, Voorraad = 520},
                new VoorraadMagazijn {ArtikelNummer = 1, Voorraad = 5},
            };

            Artikel[] artikelData =
            {
                new Artikel {Artikelnummer = 123, Leverancier = "20 lev"},
                new Artikel {Artikelnummer = 5635, Leverancier = "20 lev"},
                new Artikel {Artikelnummer = 1, Leverancier = "1 lev"}
            };

            _httpTest.RespondWithJson(data);
            _httpTest.RespondWithJson(artikelData);

            // Act
            databaseCacher.EnsureVoorraad();

            // Assert
            using BackOfficeContext resultContext = new BackOfficeContext(_options);
            VoorraadMagazijn[] result = resultContext.VoorraadMagazijn.ToArray();
            Assert.IsNotNull(result.SingleOrDefault(v => v.ArtikelNummer == 123 && v.Voorraad == 20));
            Assert.IsNotNull(result.SingleOrDefault(v => v.ArtikelNummer == 5635 && v.Voorraad == 520));
            Assert.IsNotNull(result.SingleOrDefault(v => v.ArtikelNummer == 1 && v.Voorraad == 5));
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(4)]
        [DataRow(12)]
        [DataRow(20)]
        public void EnsureKlanten_SavesAllKlanten(int amount)
        {
            // Arrange
            using BackOfficeContext context = new BackOfficeContext(_options);
            TestBusContext busContext = new TestBusContext();

            IDatabaseCacher databaseCacher = GetPopulatedInstance(new TestBusContext(), context);

            NieuweKlantAangemaaktEvent[] events = Enumerable.Range(0, amount).Select(b => new NieuweKlantAangemaaktEvent
            {
                Klant = new Klant { Factuuradres = new Adres(), Naam = "Piet ten Berge" }
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
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(4)]
        [DataRow(12)]
        [DataRow(20)]
        public void EnsureBestellingen_SavesAllBestellingen(int amount)
        {
            // Arrange
            using BackOfficeContext context = new BackOfficeContext(_options);
            TestBusContext busContext = new TestBusContext();

            IDatabaseCacher databaseCacher = GetPopulatedInstance(new TestBusContext(), context);

            Klant klant = new Klant { Id = 1, Factuuradres = new Adres() };
            TestHelpers.InjectData(_options, klant);

            NieuweBestellingAangemaaktEvent[] events = Enumerable.Repeat(new NieuweBestellingAangemaaktEvent
            {
                Bestelling = new Bestelling { Klant = klant }
            }, amount).ToArray();

            _httpTest.RespondWith($"{events.Length}");

            // To ensure the next calls don't error
            _httpTest.RespondWith("0");
            _httpTest.RespondWith("0");
            _httpTest.RespondWith("0");
            _httpTest.RespondWith("0");
            _httpTest.RespondWith("0");
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
