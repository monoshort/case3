using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BackOfficeFrontendService.Agents;
using BackOfficeFrontendService.Agents.Abstractions;
using BackOfficeFrontendService.Commands;
using BackOfficeFrontendService.Constants;
using BackOfficeFrontendService.DAL;
using BackOfficeFrontendService.EventListeners;
using BackOfficeFrontendService.Events;
using BackOfficeFrontendService.Models;
using BackOfficeFrontendService.Repositories;
using BackOfficeFrontendService.Repositories.Abstractions;
using Flurl.Http.Testing;
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
    public class BestellingEventListenersTest
    {
        private const int WaitTime = 300;
        private const string VoorraadUrl = "http://example.com";

        private SqliteConnection _connection;
        private DbContextOptions<BackOfficeContext> _options;
        private HttpTest _httpTest;

        [TestInitialize]
        public void TestInitialize()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();
            _options = new DbContextOptionsBuilder<BackOfficeContext>()
                .UseSqlite(_connection).Options;

            using BackOfficeContext context = new BackOfficeContext(_options);
            context.Database.EnsureCreated();

            Environment.SetEnvironmentVariable(EnvNames.VoorraadServiceUrl, VoorraadUrl);

            _httpTest = new HttpTest();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _connection.Close();
            _httpTest.Dispose();
        }

        [TestMethod]
        [DataRow("165635", "Jan Peter", "4215BE")]
        [DataRow("294028", "Luna Nena", "2452ED")]
        public void HandleNewBestelling_AddsBestelling(string bestellingNummer, string naam, string postcode)
        {
            // Arrange
            Klant klant = new Klant
            {
                Naam = naam,
                Factuuradres = new Adres {Postcode = postcode}
            };

            TestHelpers.InjectData(_options, klant);

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
                .AddEventListener<BestellingEventListeners>();

            using IMicroserviceHost host = hostBuilder.CreateHost();
            host.Start();

            IEventPublisher eventPublisher = new EventPublisher(testBusContext);

            NieuweBestellingAangemaaktEvent aangemaaktEvent = new NieuweBestellingAangemaaktEvent
            {
                Bestelling = new Bestelling
                {
                    BestellingNummer = bestellingNummer,
                    Klant = klant
                }
            };

            // Act
            eventPublisher.Publish(aangemaaktEvent);

            Thread.Sleep(WaitTime);

            // Assert
            using BackOfficeContext resultContext = new BackOfficeContext(_options);
            Assert.AreEqual(1, resultContext.Bestellingen.Count());

            Bestelling firstItem = resultContext.Bestellingen
                .Include(e => e.Klant.Factuuradres)
                .First();

            Assert.AreEqual(bestellingNummer, firstItem.BestellingNummer);
            Assert.AreEqual(naam, firstItem.Klant.Naam);
            Assert.AreEqual(postcode, firstItem.Klant.Factuuradres.Postcode);
        }

        [TestMethod]
        [DataRow(2049)]
        [DataRow(819472)]
        public void HandleBestellingGoedgekeurd_KeursBestellingGoed(int bestellingId)
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                Id = bestellingId,
                Goedgekeurd = false
            };

            TestHelpers.InjectData(_options, bestelling);

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
                .AddEventListener<BestellingEventListeners>();

            using IMicroserviceHost host = hostBuilder.CreateHost();
            host.Start();

            IEventPublisher eventPublisher = new EventPublisher(testBusContext);

            BestellingGoedgekeurdEvent aangemaaktEvent = new BestellingGoedgekeurdEvent
            {
                BestellingId = bestellingId
            };

            // Act
            eventPublisher.Publish(aangemaaktEvent);

            Thread.Sleep(WaitTime);

            // Assert
            using BackOfficeContext resultContext = new BackOfficeContext(_options);
            Bestelling resultBestelling = resultContext.Bestellingen.Find((long) bestellingId);
            Assert.AreEqual(true, resultBestelling.Goedgekeurd);
        }

        [TestMethod]
        [DataRow(2049)]
        [DataRow(819472)]
        public void HandleBestellingAfgekeurd_KeursBestellingAf(int bestellingId)
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                Id = bestellingId,
                Afgekeurd = false
            };

            TestHelpers.InjectData(_options, bestelling);

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
                .AddEventListener<BestellingEventListeners>();

            using IMicroserviceHost host = hostBuilder.CreateHost();
            host.Start();

            IEventPublisher eventPublisher = new EventPublisher(testBusContext);

            BestellingAfgekeurdEvent aangemaaktEvent = new BestellingAfgekeurdEvent
            {
                BestellingId = bestellingId
            };

            // Act
            eventPublisher.Publish(aangemaaktEvent);

            Thread.Sleep(WaitTime);

            // Assert
            using BackOfficeContext resultContext = new BackOfficeContext(_options);
            Bestelling resultBestelling = resultContext.Bestellingen.Find((long) bestellingId);
            Assert.AreEqual(true, resultBestelling.Afgekeurd);
        }

        [TestMethod]
        [DataRow(2049)]
        [DataRow(819472)]
        public void HandleBestellingKlaargemeld_MeldsBestellingKlaar(int bestellingId)
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                Id = bestellingId,
                KlaarGemeld = false
            };

            TestHelpers.InjectData(_options, bestelling);

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
                .AddEventListener<BestellingEventListeners>();

            using IMicroserviceHost host = hostBuilder.CreateHost();
            host.Start();

            IEventPublisher eventPublisher = new EventPublisher(testBusContext);

            BestellingKlaarGemeldEvent klaarGemeldEvent = new BestellingKlaarGemeldEvent
            {
                BestellingId = bestellingId
            };

            // Act
            eventPublisher.Publish(klaarGemeldEvent);

            Thread.Sleep(WaitTime);

            // Assert
            using BackOfficeContext resultContext = new BackOfficeContext(_options);
            Bestelling resultBestelling = resultContext.Bestellingen.Find((long) bestellingId);
            Assert.AreEqual(true, resultBestelling.KlaarGemeld);
        }

        [TestMethod]
        [DataRow(2049, 502, 20, 30)]
        [DataRow(819472, 1023, 30, 40)]
        public void HandleBestellingKlaargemeld_SendsCommandsToVoorraadService(int bestellingId, int artikelNummer, int aantal, int voorraad)
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                Id = bestellingId,
                BestelRegels = new List<BestelRegel>
                {
                    new BestelRegel
                    {
                        ArtikelNummer = artikelNummer,
                        Aantal = aantal,
                        Voorraad = new VoorraadMagazijn { Voorraad = voorraad, ArtikelNummer = artikelNummer }
                    }
                }
            };

            TestHelpers.InjectData(_options, bestelling);

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
                .AddEventListener<BestellingEventListeners>();

            using IMicroserviceHost host = hostBuilder.CreateHost();
            host.Start();

            IEventPublisher eventPublisher = new EventPublisher(testBusContext);

            BestellingKlaarGemeldEvent klaarGemeldEvent = new BestellingKlaarGemeldEvent
            {
                BestellingId = bestellingId
            };

            // Act
            eventPublisher.Publish(klaarGemeldEvent);

            Thread.Sleep(WaitTime);

            // Assert
            HaalVoorraadUitMagazijnCommand expectedCommand = new HaalVoorraadUitMagazijnCommand
            {
                Aantal = voorraad - aantal,
                Artikelnummer = artikelNummer
            };

            _httpTest.ShouldHaveCalled($"{VoorraadUrl}/{Endpoints.HaalVoorraadUitMagazijn}")
                .WithRequestJson(expectedCommand);
        }

        [TestMethod]
        [DataRow(2049)]
        [DataRow(819472)]
        public void HandleBestellingAdresLabelGeprint_SetsAdresLabel(int bestellingId)
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                Id = bestellingId,
                Goedgekeurd = false
            };

            TestHelpers.InjectData(_options, bestelling);

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
                .AddEventListener<BestellingEventListeners>();

            using IMicroserviceHost host = hostBuilder.CreateHost();
            host.Start();

            IEventPublisher eventPublisher = new EventPublisher(testBusContext);

            BestellingAdresLabelGeprintEvent labelEvent = new BestellingAdresLabelGeprintEvent
            {
                BestellingId = bestellingId
            };

            // Act
            eventPublisher.Publish(labelEvent);

            Thread.Sleep(WaitTime);

            // Assert
            using BackOfficeContext resultContext = new BackOfficeContext(_options);
            Bestelling resultBestelling = resultContext.Bestellingen.Find((long)bestellingId);
            Assert.AreEqual(true, resultBestelling.AdresLabelGeprint);
        }

        [TestMethod]
        [DataRow(2049)]
        [DataRow(819472)]
        public void HandleBestellingFactuurGeprint_SetsFactuur(int bestellingId)
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                Id = bestellingId,
                FactuurGeprint = false

            };

            TestHelpers.InjectData(_options, bestelling);

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
                .AddEventListener<BestellingEventListeners>();

            using IMicroserviceHost host = hostBuilder.CreateHost();
            host.Start();

            IEventPublisher eventPublisher = new EventPublisher(testBusContext);

            BestellingFactuurGeprintEvent factuurEvent = new BestellingFactuurGeprintEvent
            {
                BestellingId = bestellingId
            };

            // Act
            eventPublisher.Publish(factuurEvent);

            Thread.Sleep(WaitTime);

            // Assert
            using BackOfficeContext resultContext = new BackOfficeContext(_options);
            Bestelling resultBestelling = resultContext.Bestellingen.Find((long) bestellingId);
            Assert.AreEqual(true, resultBestelling.FactuurGeprint);
        }

        [TestMethod]
        [DataRow(2049)]
        [DataRow(819472)]
        public void HandleKanKlaarGemeldWorden_SetsKanKlaarGemeldWorden(int bestellingId)
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                Id = bestellingId,
                KanKlaarGemeldWorden = false
            };

            TestHelpers.InjectData(_options, bestelling);

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
                .AddEventListener<BestellingEventListeners>();

            using IMicroserviceHost host = hostBuilder.CreateHost();
            host.Start();

            IEventPublisher eventPublisher = new EventPublisher(testBusContext);

            BestellingKanKlaarGemeldWordenEvent @event = new BestellingKanKlaarGemeldWordenEvent
            {
                BestellingId = bestellingId
            };

            // Act
            eventPublisher.Publish(@event);

            Thread.Sleep(WaitTime);

            // Assert
            using BackOfficeContext resultContext = new BackOfficeContext(_options);
            Bestelling resultBestelling = resultContext.Bestellingen.Find((long) bestellingId);
            Assert.AreEqual(true, resultBestelling.KanKlaarGemeldWorden);
        }

        [TestMethod]
        [DataRow(2049, "10.00")]
        [DataRow(819472, "99.99")]
        public void HandleBetalingGeregistreerd_RegistersBetaling(int bestellingId, string bedragStr)
        {
            // Arrange
            var bedrag = decimal.Parse(bedragStr);
            Bestelling bestelling = new Bestelling
            {
                Id = bestellingId,
                OpenstaandBedrag = 100M
            };

            TestHelpers.InjectData(_options, bestelling);

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
                    services.AddSingleton<IVoorraadRepository, VoorraadRepository>();
                    services.AddSingleton<IHttpAgent, HttpAgent>();
                })
                .AddEventListener<BestellingEventListeners>();

            using IMicroserviceHost host = hostBuilder.CreateHost();
            host.Start();

            IEventPublisher eventPublisher = new EventPublisher(testBusContext);

            BetalingGeregistreerdEvent @event = new BetalingGeregistreerdEvent
            {
                BestellingId = bestellingId,
                OpenstaandBedrag = bedrag
            };

            // Act
            eventPublisher.Publish(@event);

            Thread.Sleep(WaitTime);

            // Assert
            using BackOfficeContext resultContext = new BackOfficeContext(_options);
            Bestelling resultBestelling = resultContext.Bestellingen.Find((long)bestellingId);
            Assert.AreEqual(bedrag, resultBestelling.OpenstaandBedrag);
        }

        [TestMethod]
        [DataRow(2049)]
        [DataRow(819472)]
        public void HandleKlantIsWanbetalerGeworden_SetsIsKlantWanbetaler(long bestellingId)
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                Id = bestellingId
            };

            TestHelpers.InjectData(_options, bestelling);

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
                    services.AddSingleton<IVoorraadRepository, VoorraadRepository>();
                    services.AddSingleton<IHttpAgent, HttpAgent>();
                })
                .AddEventListener<BestellingEventListeners>();

            using IMicroserviceHost host = hostBuilder.CreateHost();
            host.Start();

            IEventPublisher eventPublisher = new EventPublisher(testBusContext);

            KlantIsWanbetalerGewordenEvent @event = new KlantIsWanbetalerGewordenEvent
            {
                BestellingId = bestellingId
            };

            // Act
            eventPublisher.Publish(@event);

            Thread.Sleep(WaitTime);

            // Assert
            using BackOfficeContext resultContext = new BackOfficeContext(_options);
            Bestelling resultBestelling = resultContext.Bestellingen.Find(bestellingId);
            Assert.AreEqual(true, resultBestelling.IsKlantWanbetaler);
        }
    }
}
