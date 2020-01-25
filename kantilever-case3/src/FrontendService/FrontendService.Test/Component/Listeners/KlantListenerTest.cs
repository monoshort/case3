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
    public class KlantListenerTest
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
        public void HandleKlantAangemaaktEvent_MaaktKlantAan()
        {
            using var dbContext = new FrontendContext(_options);
            TestBusContext testBusContext = new TestBusContext();

            MicroserviceHostBuilder hostBuilder = new MicroserviceHostBuilder()
                .WithBusContext(testBusContext)
                .RegisterDependencies(services =>
                {
                    services.AddSingleton(dbContext);
                    services.AddSingleton<IKlantRepository, KlantRepository>();
                })
                .AddEventListener<KlantEventListeners>();

            using IMicroserviceHost host = hostBuilder.CreateHost();
            host.Start();

            IEventPublisher eventPublisher = new EventPublisher(testBusContext);

            Klant klant = new Klant
            {
                Naam = "Harrie",
                Telefoonnummer = "204878954273"
            };

            NieuweKlantAangemaaktEvent aangemaaktEvent = new NieuweKlantAangemaaktEvent
            {
                Klant = klant
            };

            // Act
            eventPublisher.Publish(aangemaaktEvent);

            Thread.Sleep(WaitTime);

            // Assert
            using FrontendContext resultContext = new FrontendContext(_options);
            Klant result = resultContext.Klanten.FirstOrDefault(klant => klant.Naam == "Harrie");
            Assert.AreEqual("Harrie", result.Naam);
        }
    }
}
