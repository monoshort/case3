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
    public class CatalogusListenersTest
    {
        private const int WaitTime = 200;

        private static SqliteConnection _connection;
        private static DbContextOptions<FrontendContext> _options;

        [ClassInitialize]
        public static void ClassInitialize(TestContext ctx)
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();
            _options = new DbContextOptionsBuilder<FrontendContext>()
                .UseSqlite(_connection).Options;

            using FrontendContext context = new FrontendContext(_options);
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
            using FrontendContext context = new FrontendContext(_options);
            context.Artikelen.RemoveRange(context.Artikelen);
            context.Klanten.RemoveRange(context.Klanten);
            context.SaveChanges();
        }

        [TestMethod]
        [DataRow(2049, "Test Artikel")]
        [DataRow(819472, "Artikel Test")]
        public void HandleArtikelToegevoegd_VoegtArtikelToe(long artikelnummer, string naam)
        {
            // Arrange
            using var dbContext = new FrontendContext(_options);
            TestBusContext testBusContext = new TestBusContext();

            MicroserviceHostBuilder hostBuilder = new MicroserviceHostBuilder()
                .WithBusContext(testBusContext)
                .RegisterDependencies(services =>
                {
                    services.AddSingleton(dbContext);
                    services.AddSingleton<IArtikelRepository, ArtikelRepository>();
                })
                .AddEventListener<CatalogusEventListeners>();

            using IMicroserviceHost host = hostBuilder.CreateHost();
            host.Start();

            IEventPublisher eventPublisher = new EventPublisher(testBusContext);

            ArtikelAanCatalogusToegevoegdEvent aangemaaktEvent = new ArtikelAanCatalogusToegevoegdEvent
            {
                Artikelnummer = artikelnummer,
                Naam = naam
            };

            // Act
            eventPublisher.Publish(aangemaaktEvent);

            Thread.Sleep(WaitTime);

            // Assert
            using FrontendContext resultContext = new FrontendContext(_options);
            Artikel result = resultContext.Artikelen.FirstOrDefault(artikel => artikel.Artikelnummer == artikelnummer);
            Assert.AreEqual(naam, result.Naam);
        }
    }
}
