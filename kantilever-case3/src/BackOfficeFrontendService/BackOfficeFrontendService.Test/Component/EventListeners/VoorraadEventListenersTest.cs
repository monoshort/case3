using System;
using System.Linq;
using System.Threading;
using BackOfficeFrontendService.Agents;
using BackOfficeFrontendService.Agents.Abstractions;
using BackOfficeFrontendService.Constants;
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
    public class VoorraadEventListenersTest
    {
        private const string VoorraadUrl = "http://example.com";
        private const int WaitTime = 300;

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

            Environment.SetEnvironmentVariable(EnvNames.CatalogusServiceUrl, VoorraadUrl);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _connection.Close();
        }

        [TestMethod]
        [DataRow(2, 4)]
        [DataRow(50, 10)]
        public void HandleVoorraadBesteld_SetsVoorraadBesteldToTrue(int artikelNummer, int amount)
        {
            // Arrange
            VoorraadMagazijn voorraadMagazijn = new VoorraadMagazijn
            {
                ArtikelNummer = artikelNummer,
                VoorraadBesteld = false
            };

            TestHelpers.InjectData(_options, voorraadMagazijn);

            using BackOfficeContext dbContext = new BackOfficeContext(_options);
            TestBusContext testBusContext = new TestBusContext();

            MicroserviceHostBuilder hostBuilder = new MicroserviceHostBuilder()
                .WithBusContext(testBusContext)
                .RegisterDependencies(services =>
                {
                    services.AddSingleton(dbContext);
                    services.AddSingleton<IBestellingRepository, BestellingRepository>();
                    services.AddSingleton<IKlantRepository, KlantRepository>();
                    services.AddSingleton<IEventPublisher, EventPublisher>();
                    services.AddSingleton<IVoorraadAgent, VoorraadAgent>();
                    services.AddSingleton<IHttpAgent, HttpAgent>();
                    services.AddSingleton<IVoorraadRepository, VoorraadRepository>();
                })
                .AddEventListener<VoorraadEventListeners>();

            using IMicroserviceHost host = hostBuilder.CreateHost();
            host.Start();

            IEventPublisher eventPublisher = new EventPublisher(testBusContext);

            VoorraadBesteldEvent evt = new VoorraadBesteldEvent
            {
                Artikelnummer = artikelNummer,
                BesteldeVoorraad = amount
            };

            // Act
            eventPublisher.Publish(evt);

            Thread.Sleep(WaitTime);

            // Assert
            using BackOfficeContext resultContext = new BackOfficeContext(_options);

            VoorraadMagazijn firstItem = resultContext.VoorraadMagazijn.First();
            Assert.AreEqual(artikelNummer, firstItem.ArtikelNummer);
            Assert.AreEqual(true, firstItem.VoorraadBesteld);
        }

        [TestMethod]
        [DataRow(2, 4, 3)]
        [DataRow(50, 10, 9)]
        public void HandleVoorraadVerlaagd_VerlaagdVoorraad(int artikelNummer, int originalAmount, int amount)
        {
            // Arrange
            VoorraadMagazijn voorraadMagazijn = new VoorraadMagazijn
            {
                ArtikelNummer = artikelNummer,
                Voorraad = originalAmount
            };

            TestHelpers.InjectData(_options, voorraadMagazijn);

            using BackOfficeContext dbContext = new BackOfficeContext(_options);
            TestBusContext testBusContext = new TestBusContext();

            MicroserviceHostBuilder hostBuilder = new MicroserviceHostBuilder()
                .WithBusContext(testBusContext)
                .RegisterDependencies(services =>
                {
                    services.AddSingleton(dbContext);
                    services.AddSingleton<IBestellingRepository, BestellingRepository>();
                    services.AddSingleton<IKlantRepository, KlantRepository>();
                    services.AddSingleton<IEventPublisher, EventPublisher>();
                    services.AddSingleton<IVoorraadAgent, VoorraadAgent>();
                    services.AddSingleton<IHttpAgent, HttpAgent>();
                    services.AddSingleton<IVoorraadRepository, VoorraadRepository>();
                })
                .AddEventListener<VoorraadEventListeners>();

            using IMicroserviceHost host = hostBuilder.CreateHost();
            host.Start();

            IEventPublisher eventPublisher = new EventPublisher(testBusContext);

            VoorraadVerlaagdEvent evt = new VoorraadVerlaagdEvent
            {
                Aantal = originalAmount,
                NieuweVoorraad = amount,
                Artikelnummer = artikelNummer
            };

            // Act
            eventPublisher.Publish(evt);

            Thread.Sleep(WaitTime);

            // Assert
            using BackOfficeContext resultContext = new BackOfficeContext(_options);

            VoorraadMagazijn firstItem = resultContext.VoorraadMagazijn.First();
            Assert.AreEqual(artikelNummer, firstItem.ArtikelNummer);
            Assert.AreEqual(amount, firstItem.Voorraad);
        }

        [TestMethod]
        [DataRow(2, 4, 10)]
        [DataRow(50, 10, 12)]
        public void HandleVoorraadVerhoogd_VerhoogdVoorraadAndSetsBesteldToFalse(int artikelNummer, int originalAmount, int amount)
        {
            // Arrange
            VoorraadMagazijn voorraadMagazijn = new VoorraadMagazijn
            {
                ArtikelNummer = artikelNummer,
                Voorraad = originalAmount,
                VoorraadBesteld = true
            };

            TestHelpers.InjectData(_options, voorraadMagazijn);

            using BackOfficeContext dbContext = new BackOfficeContext(_options);
            TestBusContext testBusContext = new TestBusContext();

            MicroserviceHostBuilder hostBuilder = new MicroserviceHostBuilder()
                .WithBusContext(testBusContext)
                .RegisterDependencies(services =>
                {
                    services.AddSingleton(dbContext);
                    services.AddSingleton<IBestellingRepository, BestellingRepository>();
                    services.AddSingleton<IKlantRepository, KlantRepository>();
                    services.AddSingleton<IEventPublisher, EventPublisher>();
                    services.AddSingleton<IVoorraadAgent, VoorraadAgent>();
                    services.AddSingleton<IHttpAgent, HttpAgent>();
                    services.AddSingleton<IVoorraadRepository, VoorraadRepository>();
                })
                .AddEventListener<VoorraadEventListeners>();

            using IMicroserviceHost host = hostBuilder.CreateHost();
            host.Start();

            IEventPublisher eventPublisher = new EventPublisher(testBusContext);

            VoorraadVerhoogdEvent evt = new VoorraadVerhoogdEvent
            {
                Aantal = originalAmount,
                NieuweVoorraad = amount,
                Artikelnummer = artikelNummer
            };

            // Act
            eventPublisher.Publish(evt);

            Thread.Sleep(WaitTime);

            // Assert
            using BackOfficeContext resultContext = new BackOfficeContext(_options);

            VoorraadMagazijn firstItem = resultContext.VoorraadMagazijn.First();
            Assert.AreEqual(artikelNummer, firstItem.ArtikelNummer);
            Assert.AreEqual(amount, firstItem.Voorraad);
            Assert.AreEqual(false, firstItem.VoorraadBesteld);
        }
    }
}
