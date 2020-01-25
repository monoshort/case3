using System.Linq;
using System.Threading;
using BackOfficeFrontendService.DAL;
using BackOfficeFrontendService.EventListeners;
using BackOfficeFrontendService.Events;
using BackOfficeFrontendService.Models;
using BackOfficeFrontendService.Repositories;
using BackOfficeFrontendService.Repositories.Abstractions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minor.Miffy.MicroServices.Events;
using Minor.Miffy.MicroServices.Host;
using Minor.Miffy.TestBus;

namespace BackOfficeFrontendService.Test.Component.EventListeners
{
    [TestClass]
    public class KlantEventListenersTest
    {
        private const int WaitTime = 2000;

        private SqliteConnection _connection;
        private DbContextOptions<BackOfficeContext> _options;

        [TestInitialize]
        public void TestInitialize()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();
            _options = new DbContextOptionsBuilder<BackOfficeContext>()
                .UseSqlite(_connection).Options;

            using BackOfficeContext context = new BackOfficeContext(_options);
            context.Database.EnsureCreated();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _connection.Close();
        }

        [TestMethod]
        [DataRow("Jan Peter", "4215BE")]
        [DataRow("Luna Nena", "2452ED")]
        public void HandleNieuweKlant_AddsNieuweKlant(string naam, string postcode)
        {
            // Arrange
            Klant klant = new Klant
            {
                Naam = naam,
                Factuuradres = new Adres { Postcode = postcode }
            };

            using BackOfficeContext dbContext = new BackOfficeContext(_options);
            TestBusContext testBusContext = new TestBusContext();

            MicroserviceHostBuilder hostBuilder = new MicroserviceHostBuilder()
                .WithBusContext(testBusContext)
                .RegisterDependencies(services =>
                {
                    services.AddSingleton(dbContext);
                    services.AddSingleton<IKlantRepository, KlantRepository>();
                    services.AddSingleton<IEventPublisher, EventPublisher>();
                })
                .AddEventListener<KlantEventListeners>();

            using IMicroserviceHost host = hostBuilder.CreateHost();
            host.Start();

            IEventPublisher eventPublisher = new EventPublisher(testBusContext);

            NieuweKlantAangemaaktEvent aangemaaktEvent = new NieuweKlantAangemaaktEvent
            {
                Klant = klant
            };

            // Act
            eventPublisher.Publish(aangemaaktEvent);

            Thread.Sleep(WaitTime);

            // Assert
            using BackOfficeContext resultContext = new BackOfficeContext(_options);
            Assert.AreEqual(1, resultContext.Klanten.Count());

            Klant firstKlant = resultContext.Klanten.Include(e => e.Factuuradres).First();
            Assert.AreEqual(naam, firstKlant.Naam);
            Assert.AreEqual(postcode, firstKlant.Factuuradres.Postcode);
        }
    }
}
