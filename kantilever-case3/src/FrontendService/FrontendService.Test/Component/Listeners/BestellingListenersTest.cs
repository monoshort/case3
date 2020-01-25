using System.Linq;
using System.Threading;
using FrontendService.DAL;
using FrontendService.EventListeners;
using FrontendService.Events;
using FrontendService.Models;
using FrontendService.Repositories;
using FrontendService.Repositories.Abstractions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minor.Miffy.MicroServices.Events;
using Minor.Miffy.MicroServices.Host;
using Minor.Miffy.TestBus;

namespace FrontendService.Test.Component.Listeners
{
    [TestClass]
    public class BestellingListenersTest
    {
        private const int WaitTime = 200;

        private SqliteConnection _connection;
        private DbContextOptions<FrontendContext> _options;

        [TestInitialize]
        public void TestInitialize()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();
            _options = new DbContextOptionsBuilder<FrontendContext>()
                .UseSqlite(_connection).Options;

            using FrontendContext context = new FrontendContext(_options);
            context.Database.EnsureCreated();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _connection.Close();
        }

        [TestMethod]
        public void HandleBestellingAangemaaktEvent_MaaktBestellingAan()
        {
            using var dbContext = new FrontendContext(_options);
            TestBusContext testBusContext = new TestBusContext();

            MicroserviceHostBuilder hostBuilder = new MicroserviceHostBuilder()
                .WithBusContext(testBusContext)
                .RegisterDependencies(services =>
                {
                    services.AddSingleton(dbContext);
                    services.AddSingleton<IKlantRepository, KlantRepository>();
                    services.AddSingleton<IBestellingRepository, BestellingRepository>();

                })
                .AddEventListener<BestellingEventListeners>();

            using IMicroserviceHost host = hostBuilder.CreateHost();
            host.Start();

            IEventPublisher eventPublisher = new EventPublisher(testBusContext);

            Klant klant = new Klant()
            {
                Id = 463764235,
                Naam = "Harrie",
                Telefoonnummer = "204878954273"
            };

            TestHelpers.InjectData(_options, klant);

            Bestelling bestelling = new Bestelling()
            {
                Klant = klant,
                BestellingNummer = "56712936",
                Ingepakt = true,
                Afgekeurd = false,
                Goedgekeurd = false,
                KlaarGemeld = false
            };

            using FrontendContext tempContext = new FrontendContext(_options);
            Klant klantWithId = tempContext.Klanten.FirstOrDefault(klant => klant.Telefoonnummer == "204878954273");

            NieuweBestellingAangemaaktEvent aangemaaktEvent = new NieuweBestellingAangemaaktEvent()
            {
                Bestelling = bestelling
            };

            // Act
            eventPublisher.Publish(aangemaaktEvent);

            Thread.Sleep(WaitTime);

            // Assert
            using FrontendContext resultContext = new FrontendContext(_options);
            var bestellingFromDb = resultContext.Bestellingen.Where(bestelling => bestelling.KlantId == klantWithId.Id).First();
            Assert.AreEqual("56712936", bestellingFromDb.BestellingNummer);
        }


        [TestMethod]
        public void HandleBestellingAfgekeurdEvent_KeursBestellingAf()
        {
            // Arrange
            using var dbContext = new FrontendContext(_options);
            Klant klant = new Klant()
            {
                Naam = "Harrie",
                Telefoonnummer = "204878954273"
            };
            TestHelpers.InjectData(_options, klant);

            Klant klantFromDb = dbContext.Klanten.First();
            Bestelling bestelling = new Bestelling()
            {
                KlantId = klantFromDb.Id,
                BestellingNummer = "56712936",
                Ingepakt = false,
                Afgekeurd = false,
                Goedgekeurd = false,
                KlaarGemeld = false
            };

            TestHelpers.InjectData(_options, bestelling);
            TestBusContext testBusContext = new TestBusContext();
            MicroserviceHostBuilder hostBuilder = new MicroserviceHostBuilder()
                .WithBusContext(testBusContext)
                .RegisterDependencies(services =>
                {
                    services.AddSingleton(dbContext);
                    services.AddSingleton<IKlantRepository, KlantRepository>();
                    services.AddSingleton<IBestellingRepository, BestellingRepository>();
                })
                .AddEventListener<BestellingEventListeners>();

            using IMicroserviceHost host = hostBuilder.CreateHost();
            host.Start();
            IEventPublisher eventPublisher = new EventPublisher(testBusContext);

            var bestellingFromDB = dbContext.Bestellingen.First();
            BestellingAfgekeurdEvent afgekeurdEvent = new BestellingAfgekeurdEvent()
            {
                BestellingId = bestellingFromDB.Id
            };

            // Act
            eventPublisher.Publish(afgekeurdEvent);

            Thread.Sleep(WaitTime);

            // Assert
            using var newdbContext = new FrontendContext(_options);

            Bestelling bestellingResult = newdbContext.Bestellingen.First();
            Assert.AreEqual(true, bestellingResult.Afgekeurd);
        }

        [TestMethod]
        public void HandleBestellingKlaargemeldEvent_MeldsBestellingKlaar()
        {
            // Arrange
            using var dbContext = new FrontendContext(_options);
            Klant klant = new Klant()
            {
                Naam = "Harrie",
                Telefoonnummer = "204878954273"
            };
            TestHelpers.InjectData(_options, klant);

            Klant klantFromDb = dbContext.Klanten.First();
            Bestelling bestelling = new Bestelling()
            {
                KlantId = klantFromDb.Id,
                BestellingNummer = "56712936",
                Ingepakt = false,
                Afgekeurd = false,
                Goedgekeurd = false,
                KlaarGemeld = false
            };

            TestHelpers.InjectData(_options, bestelling);
            TestBusContext testBusContext = new TestBusContext();
            MicroserviceHostBuilder hostBuilder = new MicroserviceHostBuilder()
                .WithBusContext(testBusContext)
                .RegisterDependencies(services =>
                {
                    services.AddSingleton(dbContext);
                    services.AddSingleton<IKlantRepository, KlantRepository>();
                    services.AddSingleton<IBestellingRepository, BestellingRepository>();
                })
                .AddEventListener<BestellingEventListeners>();

            using IMicroserviceHost host = hostBuilder.CreateHost();
            host.Start();
            IEventPublisher eventPublisher = new EventPublisher(testBusContext);

            var bestellingFromDB = dbContext.Bestellingen.First();
            BestellingKlaarGemeldEvent klaargemeldEvent = new BestellingKlaarGemeldEvent()
            {
                BestellingId = bestellingFromDB.Id
            };

            // Act
            eventPublisher.Publish(klaargemeldEvent);

            Thread.Sleep(WaitTime);

            // Assert
            using var newdbContext = new FrontendContext(_options);

            Bestelling bestellingResult = newdbContext.Bestellingen.First();
            Assert.AreEqual(true, bestellingResult.KlaarGemeld);
        }

        [TestMethod]
        public void HandleBestellingGoedgekeurdEvent_KeursBestellingGoed()
        {
            // Arrange
            using var dbContext = new FrontendContext(_options);
            Klant klant = new Klant()
            {
                Naam = "Harrie",
                Telefoonnummer = "204878954273"
            };
            TestHelpers.InjectData(_options, klant);

            Klant klantFromDb = dbContext.Klanten.First();
            Bestelling bestelling = new Bestelling()
            {
                KlantId = klantFromDb.Id,
                BestellingNummer = "56712936",
                Ingepakt = false,
                Afgekeurd = false,
                Goedgekeurd = false,
                KlaarGemeld = false
            };

            TestHelpers.InjectData(_options, bestelling);
            TestBusContext testBusContext = new TestBusContext();
            MicroserviceHostBuilder hostBuilder = new MicroserviceHostBuilder()
                .WithBusContext(testBusContext)
                .RegisterDependencies(services =>
                {
                    services.AddSingleton(dbContext);
                    services.AddSingleton<IKlantRepository, KlantRepository>();
                    services.AddSingleton<IBestellingRepository, BestellingRepository>();
                })
                .AddEventListener<BestellingEventListeners>();

            using IMicroserviceHost host = hostBuilder.CreateHost();
            host.Start();
            IEventPublisher eventPublisher = new EventPublisher(testBusContext);

            var bestellingFromDB = dbContext.Bestellingen.First();
            BestellingGoedgekeurdEvent goedgekeurdEvent = new BestellingGoedgekeurdEvent()
            {
                BestellingId = bestellingFromDB.Id
            };

            // Act
            eventPublisher.Publish(goedgekeurdEvent);

            Thread.Sleep(WaitTime);

            // Assert
            using var newdbContext = new FrontendContext(_options);

            Bestelling bestellingResult = newdbContext.Bestellingen.First();
            Assert.AreEqual(true, bestellingResult.Goedgekeurd);
        }
    }
}
