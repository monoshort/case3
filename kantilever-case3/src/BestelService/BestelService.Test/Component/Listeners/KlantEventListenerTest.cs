using System.Linq;
using System.Threading;
using BestelService.Commands;
using BestelService.Core.Models;
using BestelService.Core.Repositories;
using BestelService.Events;
using BestelService.Infrastructure.DAL;
using BestelService.Infrastructure.Repositories;
using BestelService.Listeners;
using BestelService.Services.Services;
using BestelService.Services.Services.Abstractions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minor.Miffy.MicroServices.Commands;
using Minor.Miffy.MicroServices.Events;
using Minor.Miffy.MicroServices.Host;
using Minor.Miffy.TestBus;

namespace BestelService.Test.Component.Listeners
{
    [TestClass]
    public class KlantEventListenerTest
    {
        private const int WaitTime = 500;

        private static SqliteConnection _connection;
        private static DbContextOptions<BestelContext> _options;

        [TestInitialize]
        public void TestInitialize()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();
            _options = new DbContextOptionsBuilder<BestelContext>()
                .UseSqlite(_connection).Options;

            using var context = new BestelContext(_options);
            context.Database.EnsureCreated();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _connection.Close();
        }

        [TestMethod]
        [DataRow("Peter")]
        [DataRow("Jan")]
        public void HandleNieuweBestelling_AddsBestellingToDatabase(string naam)
        {
            // Arrange
            using BestelContext dbContext = new BestelContext(_options);
            TestBusContext testBusContext = new TestBusContext();

            MicroserviceHostBuilder hostBuilder = new MicroserviceHostBuilder()
                .WithBusContext(testBusContext)
                .RegisterDependencies(services =>
                {
                    services.AddSingleton(dbContext);
                    services.AddSingleton<IBestelRepository, BestelRepository>();
                    services.AddSingleton<IKlantRepository, KlantRepository>();
                    services.AddSingleton<IEventPublisher, EventPublisher>();
                    services.AddSingleton<IBestellingService, BestellingService>();
                })
                .AddEventListener<KlantEventListener>();

            using IMicroserviceHost host = hostBuilder.CreateHost();
            host.Start();

            IEventPublisher eventPublisher = new EventPublisher(testBusContext);

            NieuweKlantAangemaaktEvent @event = new NieuweKlantAangemaaktEvent
            {
                Klant = new Klant { Naam = naam }
            };

            // Act
            eventPublisher.Publish(@event);

            Thread.Sleep(WaitTime);

            // Assert
            using BestelContext resultContext = new BestelContext(_options);
            Assert.AreEqual(1, resultContext.Klanten.Count());
            Assert.AreEqual(naam, resultContext.Klanten.First().Naam);
        }
    }
}
