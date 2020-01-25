using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BestelService.Commands;
using BestelService.Core.Models;
using BestelService.Core.Repositories;
using BestelService.Infrastructure.DAL;
using BestelService.Infrastructure.Repositories;
using BestelService.Listeners;
using BestelService.Services.Constants;
using BestelService.Services.Events;
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
    public class BestellingCommandListenerTest
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

        #region NieuweBestelling

        [TestMethod]
        [DataRow(24.64)]
        [DataRow(2423.64)]
        public void HandleNieuweBestelling_AddsBestellingToDatabase(double subTotaal)
        {
            // Arrange
            Klant klant = new Klant {Bestellingen = new List<Bestelling>(), Id = 5};
            TestHelpers.InjectData(_options, klant);

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
                .AddEventListener<BestellingCommandListener>();

            using IMicroserviceHost host = hostBuilder.CreateHost();
            host.Start();

            ICommandPublisher commandPublisher = new CommandPublisher(testBusContext);

            MaakNieuweBestellingAanCommand command = new MaakNieuweBestellingAanCommand
            {
                Bestelling = new Bestelling
                {
                    Subtotaal = (decimal) subTotaal,
                    Klant = klant
                }
            };

            // Act
            commandPublisher.PublishAsync<MaakNieuweBestellingAanCommand>(command).Wait();

            // Assert
            using BestelContext resultContext = new BestelContext(_options);
            Assert.AreEqual(1, resultContext.Bestellingen.Count());
            Assert.AreEqual((decimal)subTotaal, resultContext.Bestellingen.First().Subtotaal);
        }

        /// <summary>
        /// Test event listener
        /// </summary>
        private class TestMaakNieuweBestellingAanEventListener
        {
            public static NieuweBestellingAangemaaktEvent ResultEvent;

            [EventListener]
            [Topic(TopicNames.NieuweBestellingAangemaakt)]
            public void Handle(NieuweBestellingAangemaaktEvent aangemaaktEvent)
            {
                ResultEvent = aangemaaktEvent;
            }
        }

        [TestMethod]
        [DataRow(24.64)]
        [DataRow(2423.64)]
        public void HandleNieuweBestelling_PublishesNieuweBestellingEvent(double subTotaal)
        {
            // Arrange
            Klant klant = new Klant {Id = 2};
            TestHelpers.InjectData(_options, klant);

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
                .AddEventListener<BestellingCommandListener>()
                .AddEventListener<TestMaakNieuweBestellingAanEventListener>();

            using IMicroserviceHost host = hostBuilder.CreateHost();
            host.Start();

            ICommandPublisher commandPublisher = new CommandPublisher(testBusContext);

            MaakNieuweBestellingAanCommand command = new MaakNieuweBestellingAanCommand
            {
                Bestelling = new Bestelling
                {
                    Subtotaal = (decimal) subTotaal,
                    Klant = klant
                }
            };

            // Act
            commandPublisher.PublishAsync<MaakNieuweBestellingAanCommand>(command).Wait();

            Thread.Sleep(WaitTime);

            // Assert
            Assert.AreEqual((decimal)subTotaal, TestMaakNieuweBestellingAanEventListener.ResultEvent.Bestelling.Subtotaal);
        }

        #endregion

        #region KeurBestellingGoed

        [TestMethod]
        [DataRow(5028)]
        [DataRow(104028)]
        public void HandleKeurBestellingGoed_KeursBestellingGoed(int bestellingId)
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                Klant = new Klant(),
                Id = bestellingId
            };

            TestHelpers.InjectData(_options, bestelling);

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
                .AddEventListener<BestellingCommandListener>();

            using IMicroserviceHost host = hostBuilder.CreateHost();
            host.Start();

            ICommandPublisher commandPublisher = new CommandPublisher(testBusContext);

            KeurBestellingGoedCommand command = new KeurBestellingGoedCommand
            {
                BestellingId = bestellingId
            };

            // Act
            commandPublisher.PublishAsync<KeurBestellingGoedCommand>(command).Wait();

            // Assert
            using BestelContext resultContext = new BestelContext(_options);
            Assert.IsTrue(resultContext.Bestellingen.Find((long)bestellingId).Goedgekeurd);
        }

        /// <summary>
        /// Test event listener
        /// </summary>
        private class TestBestellingGoedGekeurdEventListener
        {
            public static BestellingGoedgekeurdEvent ResultEvent;

            [EventListener]
            [Topic(TopicNames.BestellingGoedgekeurd)]
            public void Handle(BestellingGoedgekeurdEvent aangemaaktEvent)
            {
                ResultEvent = aangemaaktEvent;
            }
        }

        [TestMethod]
        [DataRow(5028)]
        [DataRow(104028)]
        public void HandleKeurBestellingGoed_PublishesGoedgekeurdEvent(int bestellingId)
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                Klant = new Klant(),
                Id = bestellingId
            };

            TestHelpers.InjectData(_options, bestelling);

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
                .AddEventListener<BestellingCommandListener>()
                .AddEventListener<TestBestellingGoedGekeurdEventListener>();

            using IMicroserviceHost host = hostBuilder.CreateHost();
            host.Start();

            ICommandPublisher commandPublisher = new CommandPublisher(testBusContext);

            KeurBestellingGoedCommand command = new KeurBestellingGoedCommand
            {
                BestellingId = bestellingId
            };

            // Act
            commandPublisher.PublishAsync<KeurBestellingGoedCommand>(command).Wait();

            Thread.Sleep(WaitTime);

            // Assert
            Assert.AreEqual(bestellingId, TestBestellingGoedGekeurdEventListener.ResultEvent.BestellingId);
        }

        #endregion

        #region KeurBestellingAf

        /// <summary>
        /// Test event listener
        /// </summary>
        private class TestBestellingAfGekeurdEventListener
        {
            public static BestellingAfgekeurdEvent ResultEvent;

            [EventListener]
            [Topic(TopicNames.BestellingAfgekeurd)]
            public void Handle(BestellingAfgekeurdEvent aangemaaktEvent)
            {
                ResultEvent = aangemaaktEvent;
            }
        }

        [TestMethod]
        [DataRow(5028)]
        [DataRow(104028)]
        public void HandleKeurBestellingAf_KeursBestellingAf(int bestellingId)
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                Klant = new Klant(),
                Id = bestellingId
            };

            TestHelpers.InjectData(_options, bestelling);

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
                .AddEventListener<BestellingCommandListener>();

            using IMicroserviceHost host = hostBuilder.CreateHost();
            host.Start();

            ICommandPublisher commandPublisher = new CommandPublisher(testBusContext);

            KeurBestellingAfCommand command = new KeurBestellingAfCommand
            {
                BestellingId = bestellingId
            };

            // Act
            commandPublisher.PublishAsync<KeurBestellingAfCommand>(command).Wait();

            // Assert
            using BestelContext resultContext = new BestelContext(_options);
            Assert.IsTrue(resultContext.Bestellingen.Find((long)bestellingId).Afgekeurd);
        }

        [TestMethod]
        [DataRow(5028)]
        [DataRow(104028)]
        public void HandleKeurBestellingAf_PublishesAfgekeurdEvent(int bestellingId)
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                Klant = new Klant(),
                Id = bestellingId
            };

            TestHelpers.InjectData(_options, bestelling);

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
                .AddEventListener<BestellingCommandListener>()
                .AddEventListener<TestBestellingAfGekeurdEventListener>();

            using IMicroserviceHost host = hostBuilder.CreateHost();
            host.Start();

            ICommandPublisher commandPublisher = new CommandPublisher(testBusContext);

            KeurBestellingAfCommand command = new KeurBestellingAfCommand
            {
                BestellingId = bestellingId
            };

            // Act
            commandPublisher.PublishAsync<KeurBestellingAfCommand>(command).Wait();

            Thread.Sleep(WaitTime);

            // Assert
            Assert.AreEqual(bestellingId, TestBestellingAfGekeurdEventListener.ResultEvent.BestellingId);
        }

        #endregion

        #region MeldBestellingKlaar

        /// <summary>
        /// Test event listener
        /// </summary>
        private class TestBestellingIngepaktEventListener
        {
            public static BestellingKlaarGemeldEvent ResultEvent;

            [EventListener]
            [Topic(TopicNames.BestellingKlaarGemeld)]
            public void Handle(BestellingKlaarGemeldEvent aangemaaktEvent)
            {
                ResultEvent = aangemaaktEvent;
            }
        }

        [TestMethod]
        [DataRow(5028)]
        [DataRow(104028)]
        public void HandleMeldBestellingKlaar_MeldsBestellingKlaar(int bestellingId)
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                Klant = new Klant(),
                Id = bestellingId,
                FactuurGeprint = true,
                AdresLabelGeprint = true
            };

            TestHelpers.InjectData(_options, bestelling);

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
                .AddEventListener<BestellingCommandListener>();

            using IMicroserviceHost host = hostBuilder.CreateHost();
            host.Start();

            ICommandPublisher commandPublisher = new CommandPublisher(testBusContext);

            MeldBestellingKlaarCommand command = new MeldBestellingKlaarCommand
            {
                BestellingId = bestellingId
            };

            // Act
            commandPublisher.PublishAsync<MeldBestellingKlaarCommand>(command).Wait();

            // Assert
            using BestelContext resultContext = new BestelContext(_options);
            Assert.IsTrue(resultContext.Bestellingen.Find((long)bestellingId).KlaarGemeld);
        }

        [TestMethod]
        [DataRow(5028)]
        [DataRow(104028)]
        public void HandleMeldBestellingKlaar_PublishesKlaarGemeldEvent(int bestellingId)
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                Klant = new Klant(),
                Id = bestellingId,
                FactuurGeprint = true,
                AdresLabelGeprint = true
            };

            TestHelpers.InjectData(_options, bestelling);

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
                .AddEventListener<BestellingCommandListener>()
                .AddEventListener<TestBestellingIngepaktEventListener>();

            using IMicroserviceHost host = hostBuilder.CreateHost();
            host.Start();

            ICommandPublisher commandPublisher = new CommandPublisher(testBusContext);

            MeldBestellingKlaarCommand command = new MeldBestellingKlaarCommand
            {
                BestellingId = bestellingId
            };

            // Act
            commandPublisher.PublishAsync<MeldBestellingKlaarCommand>(command).Wait();

            Thread.Sleep(WaitTime);

            // Assert
            Assert.AreEqual(bestellingId, TestBestellingIngepaktEventListener.ResultEvent.BestellingId);
        }

        #endregion

        #region PakBestelRegelIn

        /// <summary>
        /// Test event listener
        /// </summary>
        private class TestBestelRegelIngepaktEventListener
        {
            public static BestelRegelIngepaktEvent ResultEvent;

            [EventListener]
            [Topic(TopicNames.BestelRegelIngepakt)]
            public void Handle(BestelRegelIngepaktEvent aangemaaktEvent)
            {
                ResultEvent = aangemaaktEvent;
            }
        }

        /// <summary>
        /// Test event listener
        /// </summary>
        private class TestBestellingFactuurGeprintEventListener
        {
            public static BestellingFactuurGeprintEvent ResultEvent;

            [EventListener]
            [Topic(TopicNames.FactuurGeprint)]
            public void Handle(BestellingFactuurGeprintEvent aangemaaktEvent)
            {
                ResultEvent = aangemaaktEvent;
            }
        }

        [TestMethod]
        [DataRow(5028, 200)]
        [DataRow(104028, 3000)]
        public void HandlePakBestelRegelIn_PaksBestelregelIn(int bestellingId, int bestelRegelId)
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                Klant = new Klant(),
                Id = bestellingId,
                BestelRegels = new List<BestelRegel> { new BestelRegel { Id = bestelRegelId } }
            };

            TestHelpers.InjectData(_options, bestelling);

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
                .AddEventListener<BestellingCommandListener>();

            using IMicroserviceHost host = hostBuilder.CreateHost();
            host.Start();

            ICommandPublisher commandPublisher = new CommandPublisher(testBusContext);

            PakBestelRegelInCommand command = new PakBestelRegelInCommand
            {
                BestellingId = bestellingId,
                BestelRegelId = bestelRegelId
            };

            // Act
            commandPublisher.PublishAsync<PakBestelRegelInCommand>(command).Wait();

            // Assert
            using BestelContext resultContext = new BestelContext(_options);
            Bestelling resultBestelling = resultContext.Bestellingen
                .Include(b => b.BestelRegels)
                .First(b => b.Id == bestellingId);

            Assert.IsTrue(resultBestelling.BestelRegels.First().Ingepakt);
        }

        [TestMethod]
        [DataRow(5028, 200)]
        [DataRow(104028, 3000)]
        public void HandlePakBestelRegelIn_PublishesBestelRegelIngepaktEvent(long bestellingId, long bestelRegelId)
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                Klant = new Klant(),
                Id = bestellingId,
                BestelRegels = new List<BestelRegel> { new BestelRegel { Id = bestelRegelId } }
            };

            TestHelpers.InjectData(_options, bestelling);

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
                .AddEventListener<BestellingCommandListener>()
                .AddEventListener<TestBestelRegelIngepaktEventListener>();

            using IMicroserviceHost host = hostBuilder.CreateHost();
            host.Start();

            ICommandPublisher commandPublisher = new CommandPublisher(testBusContext);

            PakBestelRegelInCommand command = new PakBestelRegelInCommand
            {
                BestellingId = bestellingId,
                BestelRegelId = bestelRegelId
            };

            // Act
            commandPublisher.PublishAsync<PakBestelRegelInCommand>(command).Wait();

            Thread.Sleep(WaitTime);

            // Assert
            Assert.AreEqual(bestellingId, TestBestelRegelIngepaktEventListener.ResultEvent.BestellingId);
            Assert.AreEqual(bestelRegelId, TestBestelRegelIngepaktEventListener.ResultEvent.BestelRegelId);
        }

        [TestMethod]
        [DataRow(5028, 20)]
        [DataRow(104028, 942)]
        public void HandlePakBestelRegelIn_PublishesKanKlaarGemeldWordenEvent(int bestellingId, int bestelRegelId)
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                Klant = new Klant(),
                Id = bestellingId,
                FactuurGeprint = true,
                AdresLabelGeprint = true,
                BestelRegels = new List<BestelRegel> { new BestelRegel { Id = bestelRegelId }}
            };

            TestHelpers.InjectData(_options, bestelling);

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
                .AddEventListener<BestellingCommandListener>()
                .AddEventListener<TestBestellingKanKlaarGemeldWordenEventListener>();

            using IMicroserviceHost host = hostBuilder.CreateHost();
            host.Start();

            ICommandPublisher commandPublisher = new CommandPublisher(testBusContext);

            PakBestelRegelInCommand command = new PakBestelRegelInCommand
            {
                BestellingId = bestellingId,
                BestelRegelId = bestelRegelId
            };

            // Act
            commandPublisher.PublishAsync<PrintFactuurCommand>(command).Wait();

            Thread.Sleep(WaitTime);

            // Assert
            Assert.AreEqual(bestellingId, TestBestellingKanKlaarGemeldWordenEventListener.ResultEvent.BestellingId);
        }

        #endregion

        #region PrintFactuur

        [TestMethod]
        [DataRow(5028)]
        [DataRow(104028)]
        public void HandlePrintFactuur_PrintsFactuur(int bestellingId)
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                Klant = new Klant(),
                Id = bestellingId
            };

            TestHelpers.InjectData(_options, bestelling);

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
                .AddEventListener<BestellingCommandListener>();

            using IMicroserviceHost host = hostBuilder.CreateHost();
            host.Start();

            ICommandPublisher commandPublisher = new CommandPublisher(testBusContext);

            PrintFactuurCommand command = new PrintFactuurCommand
            {
                BestellingId = bestellingId
            };

            // Act
            commandPublisher.PublishAsync<PrintFactuurCommand>(command).Wait();

            // Assert
            using BestelContext resultContext = new BestelContext(_options);
            Assert.IsTrue(resultContext.Bestellingen.Find((long)bestellingId).FactuurGeprint);
        }

        [TestMethod]
        [DataRow(5028)]
        [DataRow(104028)]
        public void HandlePrintFactuur_PublishesFactuurGeprintEvent(int bestellingId)
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                Klant = new Klant(),
                Id = bestellingId
            };

            TestHelpers.InjectData(_options, bestelling);

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
                .AddEventListener<BestellingCommandListener>()
                .AddEventListener<TestBestellingFactuurGeprintEventListener>();

            using IMicroserviceHost host = hostBuilder.CreateHost();
            host.Start();

            ICommandPublisher commandPublisher = new CommandPublisher(testBusContext);

            PrintFactuurCommand command = new PrintFactuurCommand
            {
                BestellingId = bestellingId
            };

            // Act
            commandPublisher.PublishAsync<PrintFactuurCommand>(command).Wait();

            Thread.Sleep(WaitTime);

            // Assert
            Assert.AreEqual(bestellingId, TestBestellingFactuurGeprintEventListener.ResultEvent.BestellingId);
        }

        /// <summary>
        /// Test event listener
        /// </summary>
        private class TestBestellingKanKlaarGemeldWordenEventListener
        {
            public static BestellingKanKlaarGemeldWordenEvent ResultEvent;

            [EventListener]
            [Topic(TopicNames.BestellingKanKlaarGemeldWorden)]
            public void Handle(BestellingKanKlaarGemeldWordenEvent aangemaaktEvent)
            {
                ResultEvent = aangemaaktEvent;
            }
        }

        [TestMethod]
        [DataRow(5028)]
        [DataRow(104028)]
        public void HandlePrintFactuur_PublishesKanKlaarGemeldWordenEventProperly(int bestellingId)
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                Id = bestellingId,
                AdresLabelGeprint = true,
                Klant = new Klant()
            };

            TestHelpers.InjectData(_options, bestelling);

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
                .AddEventListener<BestellingCommandListener>()
                .AddEventListener<TestBestellingKanKlaarGemeldWordenEventListener>();

            using IMicroserviceHost host = hostBuilder.CreateHost();
            host.Start();

            ICommandPublisher commandPublisher = new CommandPublisher(testBusContext);

            PrintFactuurCommand command = new PrintFactuurCommand
            {
                BestellingId = bestellingId
            };

            // Act
            commandPublisher.PublishAsync<PrintFactuurCommand>(command).Wait();

            Thread.Sleep(WaitTime);

            // Assert
            Assert.AreEqual(bestellingId, TestBestellingKanKlaarGemeldWordenEventListener.ResultEvent.BestellingId);
        }

        #endregion

        #region PrintAdresLabel

        /// <summary>
        /// Test event listener
        /// </summary>
        private class TestBestellingAdresLabelGeprintEventListener
        {
            public static BestellingAdresLabelGeprintEvent ResultEvent;

            [EventListener]
            [Topic(TopicNames.AdresLabelGeprint)]
            public void Handle(BestellingAdresLabelGeprintEvent aangemaaktEvent)
            {
                ResultEvent = aangemaaktEvent;
            }
        }

        [TestMethod]
        [DataRow(5028)]
        [DataRow(104028)]
        public void HandlePrintAdresLabel_PrintsLabel(int bestellingId)
        {
            // Arrange
            Klant klant = new Klant();
            Bestelling bestelling = new Bestelling
            {
                Id = bestellingId,
                Klant = klant
            };
            klant.Bestellingen.Add(bestelling);

            TestHelpers.InjectData(_options, bestelling);

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
                .AddEventListener<BestellingCommandListener>();

            using IMicroserviceHost host = hostBuilder.CreateHost();
            host.Start();

            ICommandPublisher commandPublisher = new CommandPublisher(testBusContext);

            PrintAdresLabelCommand command = new PrintAdresLabelCommand
            {
                BestellingId = bestellingId
            };

            // Act
            commandPublisher.PublishAsync<PrintAdresLabelCommand>(command).Wait();

            // Assert
            using BestelContext resultContext = new BestelContext(_options);
            Assert.IsTrue(resultContext.Bestellingen.Find((long)bestellingId).AdresLabelGeprint);
        }

        [TestMethod]
        [DataRow(5028)]
        [DataRow(104028)]
        public void HandlePrintAdresLabel_PublishesAdresLabelGeprintEvent(int bestellingId)
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                Klant = new Klant(),
                Id = bestellingId
            };

            TestHelpers.InjectData(_options, bestelling);

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
                .AddEventListener<BestellingCommandListener>()
                .AddEventListener<TestBestellingAdresLabelGeprintEventListener>();

            using IMicroserviceHost host = hostBuilder.CreateHost();
            host.Start();

            ICommandPublisher commandPublisher = new CommandPublisher(testBusContext);

            PrintAdresLabelCommand command = new PrintAdresLabelCommand
            {
                BestellingId = bestellingId
            };

            // Act
            commandPublisher.PublishAsync<PrintAdresLabelCommand>(command).Wait();

            Thread.Sleep(WaitTime);

            // Assert
            Assert.AreEqual(bestellingId, TestBestellingAdresLabelGeprintEventListener.ResultEvent.BestellingId);
        }

        [TestMethod]
        [DataRow(5028)]
        [DataRow(104028)]
        public void HandlePrintAdresLabel_PublishesKanKlaarGemeldWordenEventProperly(int bestellingId)
        {
            // Arrange
            Klant klant = new Klant();
            Bestelling bestelling = new Bestelling
            {
                Id = bestellingId,
                FactuurGeprint = true,
                Klant = klant
            };
            klant.Bestellingen.Add(bestelling);

            TestHelpers.InjectData(_options, bestelling);

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
                .AddEventListener<BestellingCommandListener>()
                .AddEventListener<TestBestellingKanKlaarGemeldWordenEventListener>();

            using IMicroserviceHost host = hostBuilder.CreateHost();
            host.Start();

            ICommandPublisher commandPublisher = new CommandPublisher(testBusContext);

            PrintAdresLabelCommand command = new PrintAdresLabelCommand
            {
                BestellingId = bestellingId
            };

            // Act
            commandPublisher.PublishAsync<PrintFactuurCommand>(command).Wait();

            Thread.Sleep(WaitTime);

            // Assert
            Assert.AreEqual(bestellingId, TestBestellingKanKlaarGemeldWordenEventListener.ResultEvent.BestellingId);
        }

        #endregion

        #region RegistreerBetaling

        /// <summary>
        /// Test event listener
        /// </summary>
        private class TestBestellingBetalingGeregistreerdEventListener
        {
            public static BetalingGeregistreerdEvent ResultEvent;

            [EventListener]
            [Topic(TopicNames.BetalingGeregistreerd)]
            public void Handle(BetalingGeregistreerdEvent aangemaaktEvent)
            {
                ResultEvent = aangemaaktEvent;
            }
        }

        [TestMethod]
        [DataRow(1, "10001", "10.99", "100.00")]
        [DataRow(2, "10002", "99.99", "100.00")]
        public void HandleRegistreerBetaling_RegistreertBetaling(int bestellingId, string bestellingNummer, string betaaldBedragStr, string openstaandBedragStr)
        {
            // Arrange
            var betaaldBedrag = decimal.Parse(betaaldBedragStr);
            var openstaandBedrag = decimal.Parse(openstaandBedragStr);
            Bestelling bestelling = new Bestelling
            {
                Id = bestellingId,
                OpenstaandBedrag = openstaandBedrag,
                BestellingNummer = bestellingNummer,
                Klant = new Klant()
            };

            TestHelpers.InjectData(_options, bestelling);

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
                .AddEventListener<BestellingCommandListener>();

            using IMicroserviceHost host = hostBuilder.CreateHost();
            host.Start();

            ICommandPublisher commandPublisher = new CommandPublisher(testBusContext);

            RegistreerBetalingCommand command = new RegistreerBetalingCommand
            {
                BestellingNummer = bestellingNummer,
                BetaaldBedrag = betaaldBedrag
            };

            // Act
            commandPublisher.PublishAsync<RegistreerBetalingCommand>(command).Wait();

            // Assert
            using BestelContext resultContext = new BestelContext(_options);
            var expectedBedrag = openstaandBedrag - betaaldBedrag;
            Assert.AreEqual(expectedBedrag, resultContext.Bestellingen.Single(b => b.BestellingNummer == bestellingNummer).OpenstaandBedrag);
        }

        [TestMethod]
        [DataRow(1, "10001", "10.99", "100.00")]
        [DataRow(2, "10002", "99.99", "100.00")]
        public void HandleRegistreerBetaling_PublishesBetalingGeregistreerdEventProperly(int bestellingId, string bestellingNummer, string betaaldBedragStr, string openstaandBedragStr)
        {
            // Arrange
            var betaaldBedrag = decimal.Parse(betaaldBedragStr);
            var openstaandBedrag = decimal.Parse(openstaandBedragStr);
            Bestelling bestelling = new Bestelling
            {
                Id = bestellingId,
                OpenstaandBedrag = openstaandBedrag,
                BestellingNummer = bestellingNummer,
                Klant = new Klant()
            };

            TestHelpers.InjectData(_options, bestelling);

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
                .AddEventListener<BestellingCommandListener>()
                .AddEventListener<TestBestellingBetalingGeregistreerdEventListener>();

            using IMicroserviceHost host = hostBuilder.CreateHost();
            host.Start();

            ICommandPublisher commandPublisher = new CommandPublisher(testBusContext);

            RegistreerBetalingCommand command = new RegistreerBetalingCommand
            {
                BestellingNummer = bestellingNummer,
                BetaaldBedrag = betaaldBedrag
            };

            // Act
            commandPublisher.PublishAsync<RegistreerBetalingCommand>(command).Wait();

            Thread.Sleep(WaitTime);

            // Assert
            var expectedBedrag = openstaandBedrag - betaaldBedrag;
            Assert.AreEqual(bestellingId, TestBestellingBetalingGeregistreerdEventListener.ResultEvent.BestellingId);
            Assert.AreEqual(expectedBedrag, TestBestellingBetalingGeregistreerdEventListener.ResultEvent.OpenstaandBedrag);
        }

        #endregion

        #region controleerwanbetalers

        /// <summary>
        /// Test event listener
        /// </summary>
        private class TestWanbetalerEventListener
        {
            public static KlantIsWanbetalerGewordenEvent ResultEvent;

            [EventListener]
            [Topic(TopicNames.KlantIsWanbetalerGeworden)]
            public void Handle(KlantIsWanbetalerGewordenEvent aangemaaktEvent)
            {
                ResultEvent = aangemaaktEvent;
            }
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(59)]
        public void HandleWanbetalingenZijnCommand_SetsBestellingKlantAsWanbetaler(long id)
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                Klant = new Klant(),
                Id = id,
                BestelDatum = DateTime.Now.AddDays(-Bestelling.DagenOmTeBetalen - 1)
            };

            TestHelpers.InjectData(_options, bestelling);

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
                .AddEventListener<BestellingCommandListener>()
                .AddEventListener<TestWanbetalerEventListener>();

            using IMicroserviceHost host = hostBuilder.CreateHost();
            host.Start();

            ICommandPublisher commandPublisher = new CommandPublisher(testBusContext);

            ControleerOfErWanbetalingenZijnCommand command = new ControleerOfErWanbetalingenZijnCommand();

            // Act
            commandPublisher.PublishAsync<IEnumerable<Bestelling>>(command).Wait();

            Thread.Sleep(WaitTime);

            // Assert
            Assert.AreEqual(id, TestWanbetalerEventListener.ResultEvent.BestellingId);
        }

        [TestMethod]
        public void HandleWanbetalingenZijnCommand_PublishesKlantIsWanbetalerGewordenEvent()
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                Klant = new Klant(),
                BestellingNummer = "1",
                BestelDatum = DateTime.Now.AddDays(-Bestelling.DagenOmTeBetalen - 1)
            };

            TestHelpers.InjectData(_options, bestelling);

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
                .AddEventListener<BestellingCommandListener>()
                .AddEventListener<TestBestellingBetalingGeregistreerdEventListener>();

            using IMicroserviceHost host = hostBuilder.CreateHost();
            host.Start();

            ICommandPublisher commandPublisher = new CommandPublisher(testBusContext);

            ControleerOfErWanbetalingenZijnCommand command = new ControleerOfErWanbetalingenZijnCommand();

            // Act
            commandPublisher.PublishAsync<IEnumerable<Bestelling>>(command).Wait();

            Thread.Sleep(WaitTime);

            // Assert
            using BestelContext resultContext = new BestelContext(_options);
            Bestelling besteling = resultContext.Bestellingen.Find(1L);
            Assert.AreEqual("1", besteling.BestellingNummer);
            Assert.AreEqual(true, besteling.IsKlantWanbetaler);
        }

        [TestMethod]
        public void HandleWanbetalingenZijnCommand_ReturnListOfNewWanbetalerBestellingen()
        {
            // Arrange
            Bestelling[] bestellingen = {
                new Bestelling
                {
                    Id = 1,
                    BestelDatum = DateTime.Now.AddDays(-Bestelling.DagenOmTeBetalen - 1)
                },
                new Bestelling
                {
                    Id = 2,
                    BestelDatum = DateTime.Now
                },
                new Bestelling
                {
                    Id = 3,
                    BestelDatum = DateTime.Now.AddDays(-Bestelling.DagenOmTeBetalen - 6)
                },
                new Bestelling
                {
                    Id = 4,
                    BestelDatum = DateTime.Now.AddDays(-Bestelling.DagenOmTeBetalen - 40),
                    IsKlantWanbetaler = true
                },
                new Bestelling
                {
                    Id = 5,
                    BestelDatum = DateTime.Now.AddDays(-Bestelling.DagenOmTeBetalen - 17),
                    Goedgekeurd = true
                },
                new Bestelling
                {
                    Id = 6,
                    BestelDatum = DateTime.Now.AddDays(-Bestelling.DagenOmTeBetalen - 17),
                    Afgekeurd = true
                },
            };

            TestHelpers.InjectData(_options, bestellingen);

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
                .AddEventListener<BestellingCommandListener>()
                .AddEventListener<TestBestellingBetalingGeregistreerdEventListener>();

            using IMicroserviceHost host = hostBuilder.CreateHost();
            host.Start();

            ICommandPublisher commandPublisher = new CommandPublisher(testBusContext);

            ControleerOfErWanbetalingenZijnCommand command = new ControleerOfErWanbetalingenZijnCommand();

            // Act
            IEnumerable<Bestelling> result = commandPublisher.PublishAsync<IEnumerable<Bestelling>>(command).Result
                .ToList();

            // Assert
            Assert.AreEqual(2, result.Count());
            Assert.IsNotNull(result.Single(e => e.Id == 1));
            Assert.IsNotNull(result.Single(e => e.Id == 3));
            Assert.IsNull(result.SingleOrDefault(e => e.Id == 2));
            Assert.IsNull(result.SingleOrDefault(e => e.Id == 5));
        }

        #endregion
    }
}
