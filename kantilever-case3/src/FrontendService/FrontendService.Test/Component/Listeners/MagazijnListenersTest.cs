using System.Linq;
using System.Threading;
using FrontendService.DAL;
using FrontendService.Events;
using FrontendService.Listeners;
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
    public class MagazijnListenersTest
    {
        private const int WaitTime = 500;

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
        [DataRow(2049, 45)]
        [DataRow(819472, 5)]
        public void HandleVoorraadVerhoogd_VerhoogtVoorraad(int artikelnummer, int nieuweVoorraad)
        {
            // Arrange
            Artikel bestelling = new Artikel
            {
                Artikelnummer = artikelnummer,
                Voorraad = nieuweVoorraad / 2
            };

            TestHelpers.InjectData(_options, bestelling);

            using var dbContext = new FrontendContext(_options);
            TestBusContext testBusContext = new TestBusContext();

            MicroserviceHostBuilder hostBuilder = new MicroserviceHostBuilder()
                .WithBusContext(testBusContext)
                .RegisterDependencies(services =>
                {
                    services.AddSingleton(dbContext);
                    services.AddSingleton<IArtikelRepository, ArtikelRepository>();
                })
                .AddEventListener<MagazijnEventListeners>();

            using IMicroserviceHost host = hostBuilder.CreateHost();
            host.Start();

            IEventPublisher eventPublisher = new EventPublisher(testBusContext);

            var voorraadEvent = new VoorraadVerhoogdEvent
            {
                Artikelnummer = artikelnummer,
                NieuweVoorraad = nieuweVoorraad
            };

            // Act
            eventPublisher.Publish(voorraadEvent);

            Thread.Sleep(WaitTime);

            // Assert
            using FrontendContext resultContext = new FrontendContext(_options);
            Artikel result = resultContext.Artikelen.FirstOrDefault(artikel =>
                artikel.Artikelnummer == (long)artikelnummer);
            Assert.AreEqual(nieuweVoorraad, result.Voorraad);
        }

        [TestMethod]
        [DataRow(2049, 45)]
        [DataRow(819472, 5)]
        public void HandleVoorraadVerlaagd_VerlaagtVoorraad(int artikelnummer, int nieuweVoorraad)
        {
            // Arrange
            Artikel bestelling = new Artikel
            {
                Artikelnummer = artikelnummer,
                Voorraad = nieuweVoorraad * 2
            };

            TestHelpers.InjectData(_options, bestelling);

            using var dbContext = new FrontendContext(_options);
            TestBusContext testBusContext = new TestBusContext();

            MicroserviceHostBuilder hostBuilder = new MicroserviceHostBuilder()
                .WithBusContext(testBusContext)
                .RegisterDependencies(services =>
                {
                    services.AddSingleton(dbContext);
                    services.AddSingleton<IArtikelRepository, ArtikelRepository>();
                })
                .AddEventListener<MagazijnEventListeners>();

            using IMicroserviceHost host = hostBuilder.CreateHost();
            host.Start();

            IEventPublisher eventPublisher = new EventPublisher(testBusContext);

            var voorraadEvent = new VoorraadVerlaagdEvent
            {
                Artikelnummer = artikelnummer,
                NieuweVoorraad = nieuweVoorraad
            };

            // Act
            eventPublisher.Publish(voorraadEvent);

            Thread.Sleep(WaitTime);

            // Assert
            using FrontendContext resultContext = new FrontendContext(_options);
            Artikel result = resultContext.Artikelen.FirstOrDefault(artikel =>
                artikel.Artikelnummer == (long)artikelnummer);
            Assert.AreEqual(nieuweVoorraad, result.Voorraad);
        }
    }
}
