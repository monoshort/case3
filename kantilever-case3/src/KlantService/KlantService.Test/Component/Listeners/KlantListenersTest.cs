using System.Linq;
using System.Threading;
using KlantService.Commands;
using KlantService.Constants;
using KlantService.DAL;
using KlantService.Events;
using KlantService.Listeners;
using KlantService.Models;
using KlantService.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minor.Miffy.MicroServices.Commands;
using Minor.Miffy.MicroServices.Events;
using Minor.Miffy.MicroServices.Host;
using Minor.Miffy.TestBus;

namespace KlantService.Test.Component.Listeners
{
    [TestClass]
    public class BestellingListenersTest
    {
        private const int WaitTime = 2000;

        private static SqliteConnection _connection;
        private static DbContextOptions<KlantContext> _options;

        [ClassInitialize]
        public static void ClassInitialize(TestContext tc)
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();
            _options = new DbContextOptionsBuilder<KlantContext>()
                .UseSqlite(_connection).Options;

            using var context = new KlantContext(_options);
            context.Database.EnsureCreated();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            _connection.Close();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            using var context = new KlantContext(_options);
            context.Set<Klant>().RemoveRange(context.Set<Klant>());
            context.SaveChanges();
        }

        [TestMethod]
        [DataRow("Jan Peter", "4215BE", "Regenbooglaan", "Bergen op Zoom")]
        [DataRow("Luna Nena", "2452ED", "Spoorstraat", "Breda")]
        public void HandleNieuweKlantAangemaakt_AddsKlantToDatabase(string naam, string postcode, string straat, string woonplaats)
        {
            // Arrange
            using KlantContext dbContext = new KlantContext(_options);
            TestBusContext testBusContext = new TestBusContext();

            MicroserviceHostBuilder hostBuilder = new MicroserviceHostBuilder()
                .WithBusContext(testBusContext)
                .RegisterDependencies(services =>
                {
                    services.AddSingleton(dbContext);
                    services.AddSingleton<IKlantRepository, KlantRepository>();
                    services.AddSingleton<IEventPublisher, EventPublisher>();
                })
                .AddEventListener<KlantListeners>();

            using IMicroserviceHost host = hostBuilder.CreateHost();
            host.Start();

            ICommandPublisher commandPublisher = new CommandPublisher(testBusContext);

            Klant klant = new Klant
            {
                Naam = naam,
                Factuuradres = new Adres
                {
                    Postcode = postcode,
                    StraatnaamHuisnummer = straat,
                    Woonplaats = woonplaats
                }
            };

            MaakNieuweKlantAanCommand aanmaakCommand = new MaakNieuweKlantAanCommand
            {
                Klant = klant
            };

            // Act
            commandPublisher.PublishAsync<MaakNieuweKlantAanCommand>(aanmaakCommand).Wait();

            // Assert
            using KlantContext resultContext = new KlantContext(_options);
            Assert.AreEqual(1, resultContext.Klanten.Count());

            Klant firstItem = resultContext.Klanten.Include(e => e.Factuuradres).First();
            Assert.AreEqual(naam, firstItem.Naam);
            Assert.AreEqual(postcode, firstItem.Factuuradres.Postcode);
            Assert.AreEqual(woonplaats, firstItem.Factuuradres.Woonplaats);
            Assert.AreEqual(straat, firstItem.Factuuradres.StraatnaamHuisnummer);
        }

        [TestMethod]
        [DataRow("Jan Peter", "4215BE", "Regenbooglaan", "Bergen op Zoom")]
        [DataRow("Luna Nena", "2452ED", "Spoorstraat", "Breda")]
        public void HandleNieuweKlantAangemaakt_ReturnsCommandWithId(string naam, string postcode, string straat, string woonplaats)
        {
            // Arrange
            Klant[] existingKlanten =
            {
                new Klant(),
                new Klant(),
                new Klant(),
                new Klant(),
            };
            TestHelpers.InjectData(_options, existingKlanten);

            using KlantContext dbContext = new KlantContext(_options);
            TestBusContext testBusContext = new TestBusContext();

            MicroserviceHostBuilder hostBuilder = new MicroserviceHostBuilder()
                .WithBusContext(testBusContext)
                .RegisterDependencies(services =>
                {
                    services.AddSingleton(dbContext);
                    services.AddSingleton<IKlantRepository, KlantRepository>();
                    services.AddSingleton<IEventPublisher, EventPublisher>();
                })
                .AddEventListener<KlantListeners>();

            using IMicroserviceHost host = hostBuilder.CreateHost();
            host.Start();

            ICommandPublisher commandPublisher = new CommandPublisher(testBusContext);

            Klant klant = new Klant
            {
                Naam = naam,
                Factuuradres = new Adres
                {
                    Postcode = postcode,
                    StraatnaamHuisnummer = straat,
                    Woonplaats = woonplaats
                }
            };

            MaakNieuweKlantAanCommand aanmaakCommand = new MaakNieuweKlantAanCommand
            {
                Klant = klant
            };

            // Act
            MaakNieuweKlantAanCommand result = commandPublisher.PublishAsync<MaakNieuweKlantAanCommand>(aanmaakCommand).Result;

            // Assert
            using KlantContext resultContext = new KlantContext(_options);
            Assert.AreEqual(result.Klant.Id, resultContext.Klanten.Single(k => k.Naam.Equals(naam)).Id);
        }

        private class TestEventListener
        {
            public static NieuweKlantAangemaaktEvent AangemaaktEvent;

            [EventListener]
            [Topic(TopicNames.NieuweKlantAangemaakt)]
            public void HandleEvent(NieuweKlantAangemaaktEvent aangemaaktEvent)
            {
                AangemaaktEvent = aangemaaktEvent;
            }
        }

        [TestMethod]
        [DataRow("Jan Peter", "4215BE", "Regenbooglaan", "Bergen op Zoom")]
        [DataRow("Luna Nena", "2452ED", "Spoorstraat", "Breda")]
        public void HandleNieuweKlantAangemaakt_PublishesAangemaaktEvent(string naam, string postcode, string straat, string woonplaats)
        {
            // Arrange
            using KlantContext dbContext = new KlantContext(_options);
            TestBusContext testBusContext = new TestBusContext();

            MicroserviceHostBuilder hostBuilder = new MicroserviceHostBuilder()
                .WithBusContext(testBusContext)
                .RegisterDependencies(services =>
                {
                    services.AddSingleton(dbContext);
                    services.AddSingleton<IKlantRepository, KlantRepository>();
                    services.AddSingleton<IEventPublisher, EventPublisher>();
                })
                .AddEventListener<TestEventListener>()
                .AddEventListener<KlantListeners>();

            using IMicroserviceHost host = hostBuilder.CreateHost();
            host.Start();

            ICommandPublisher commandPublisher = new CommandPublisher(testBusContext);

            Klant klant = new Klant
            {
                Naam = naam,
                Factuuradres = new Adres
                {
                    Postcode = postcode,
                    StraatnaamHuisnummer = straat,
                    Woonplaats = woonplaats
                }
            };

            MaakNieuweKlantAanCommand aanmaakCommand = new MaakNieuweKlantAanCommand
            {
                Klant = klant
            };

            // Act
            commandPublisher.PublishAsync<MaakNieuweKlantAanCommand>(aanmaakCommand).Wait();

            Thread.Sleep(WaitTime);

            // Assert
            Assert.AreEqual(naam, TestEventListener.AangemaaktEvent.Klant.Naam);
            Assert.AreEqual(postcode, TestEventListener.AangemaaktEvent.Klant.Factuuradres.Postcode);
            Assert.AreEqual(straat, TestEventListener.AangemaaktEvent.Klant.Factuuradres.StraatnaamHuisnummer);
            Assert.AreEqual(woonplaats, TestEventListener.AangemaaktEvent.Klant.Factuuradres.Woonplaats);
        }
    }
}
