using System;
using System.Collections.Generic;
using System.Linq;
using BackOfficeFrontendService.Agents.Abstractions;
using BackOfficeFrontendService.Constants;
using BackOfficeFrontendService.Events;
using BackOfficeFrontendService.Models;
using BackOfficeFrontendService.Repositories.Abstractions;
using BackOfficeFrontendService.Seeding;
using BackOfficeFrontendService.Seeding.Abstractions;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minor.Miffy.TestBus;
using Moq;

namespace BackOfficeFrontendService.Test.Unit.Seeding
{
    [TestClass]
    public class DatabaseCacherTest
    {
        [TestMethod]
        public void EnsureVoorraad_DoesNotCallAgentIfVoorraadExists()
        {
            // Arrange
            Mock<IVoorraadRepository> voorraadRepoMock = new Mock<IVoorraadRepository>();
            Mock<ICatalogusAgent> catalogusAgent = new Mock<ICatalogusAgent>();
            Mock<IEventReplayer> eventReplayerMock = new Mock<IEventReplayer>();
            Mock<IKlantRepository> klantRepositoryMock = new Mock<IKlantRepository>();
            Mock<IBestellingRepository> bestellingRepositoryMock = new Mock<IBestellingRepository>();
            Mock<IVoorraadAgent> voorraadAgentMock = new Mock<IVoorraadAgent>();
            IDatabaseCacher databaseCacher = new DatabaseCacher(voorraadAgentMock.Object, catalogusAgent.Object, voorraadRepoMock.Object, bestellingRepositoryMock.Object, klantRepositoryMock.Object, eventReplayerMock.Object, new NullLoggerFactory());

            voorraadRepoMock.Setup(e => e.IsEmpty()).Returns(false);

            // Act
            databaseCacher.EnsureVoorraad();

            // Assert
            voorraadAgentMock.Verify(e => e.GetAllVoorraadAsync(), Times.Never);
        }

        [TestMethod]
        public void EnsureVoorraad_CallsGetAllVoorraadOnAgent()
        {
            // Arrange
            Mock<IVoorraadRepository> voorraadRepoMock = new Mock<IVoorraadRepository>();
            Mock<ICatalogusAgent> catalogusAgent = new Mock<ICatalogusAgent>();
            Mock<IEventReplayer> eventReplayerMock = new Mock<IEventReplayer>();
            Mock<IKlantRepository> klantRepositoryMock = new Mock<IKlantRepository>();
            Mock<IBestellingRepository> bestellingRepositoryMock = new Mock<IBestellingRepository>();
            Mock<IVoorraadAgent> voorraadAgentMock = new Mock<IVoorraadAgent>();
            IDatabaseCacher databaseCacher = new DatabaseCacher(voorraadAgentMock.Object, catalogusAgent.Object, voorraadRepoMock.Object, bestellingRepositoryMock.Object, klantRepositoryMock.Object, eventReplayerMock.Object, new NullLoggerFactory());

            voorraadRepoMock.Setup(e => e.IsEmpty()).Returns(true);
            voorraadAgentMock.Setup(e => e.GetAllVoorraadAsync())
                .ReturnsAsync(new VoorraadMagazijn[0])
                .Verifiable();

            catalogusAgent.Setup(e => e.GetAlleArtikelenAsync())
                .ReturnsAsync(new Artikel[0]);

            // Act
            databaseCacher.EnsureVoorraad();

            // Assert
            voorraadAgentMock.Verify();
        }

        [TestMethod]
        public void EnsureVoorraad_CallsGetAllArtikelenOnAgent()
        {
            // Arrange
            Mock<IVoorraadRepository> voorraadRepoMock = new Mock<IVoorraadRepository>();
            Mock<ICatalogusAgent> catalogusAgent = new Mock<ICatalogusAgent>();
            Mock<IEventReplayer> eventReplayerMock = new Mock<IEventReplayer>();
            Mock<IKlantRepository> klantRepositoryMock = new Mock<IKlantRepository>();
            Mock<IBestellingRepository> bestellingRepositoryMock = new Mock<IBestellingRepository>();
            Mock<IVoorraadAgent> voorraadAgentMock = new Mock<IVoorraadAgent>();
            IDatabaseCacher databaseCacher = new DatabaseCacher(voorraadAgentMock.Object, catalogusAgent.Object, voorraadRepoMock.Object, bestellingRepositoryMock.Object, klantRepositoryMock.Object, eventReplayerMock.Object, new NullLoggerFactory());

            voorraadRepoMock.Setup(e => e.IsEmpty()).Returns(true);
            voorraadAgentMock.Setup(e => e.GetAllVoorraadAsync())
                .ReturnsAsync(new VoorraadMagazijn[0]);

            catalogusAgent.Setup(e => e.GetAlleArtikelenAsync())
                .ReturnsAsync(new Artikel[0])
                .Verifiable();

            // Act
            databaseCacher.EnsureVoorraad();

            // Assert
            catalogusAgent.Verify();
        }

        [TestMethod]
        public void EnsureVoorraad_CallsAddOnVoorraad()
        {
            // Arrange
            Mock<IVoorraadRepository> voorraadRepoMock = new Mock<IVoorraadRepository>();
            Mock<ICatalogusAgent> catalogusAgent = new Mock<ICatalogusAgent>();
            Mock<IEventReplayer> eventReplayerMock = new Mock<IEventReplayer>();
            Mock<IKlantRepository> klantRepositoryMock = new Mock<IKlantRepository>();
            Mock<IBestellingRepository> bestellingRepositoryMock = new Mock<IBestellingRepository>();
            Mock<IVoorraadAgent> voorraadAgentMock = new Mock<IVoorraadAgent>();
            IDatabaseCacher databaseCacher = new DatabaseCacher(voorraadAgentMock.Object, catalogusAgent.Object, voorraadRepoMock.Object, bestellingRepositoryMock.Object, klantRepositoryMock.Object, eventReplayerMock.Object, new NullLoggerFactory());

            VoorraadMagazijn[] voorraadData = {
                new VoorraadMagazijn { ArtikelNummer = 5, Voorraad = 2 },
                new VoorraadMagazijn { ArtikelNummer = 6, Voorraad = 5 }
            };

            Artikel[] artikelData =
            {
                new Artikel { Artikelnummer = 5, Leverancier = "TestLeverancier", Leveranciercode = "TL" },
                new Artikel { Artikelnummer = 6, Leverancier = "LeverancierTest", Leveranciercode = "LT" }
            };

            voorraadRepoMock.Setup(e => e.IsEmpty()).Returns(true);
            voorraadAgentMock.Setup(e => e.GetAllVoorraadAsync())
                .ReturnsAsync(voorraadData);

            catalogusAgent.Setup(e => e.GetAlleArtikelenAsync())
                .ReturnsAsync(artikelData);

            IEnumerable<VoorraadMagazijn> result = null;
            voorraadRepoMock.Setup(e => e.Add(It.IsAny<VoorraadMagazijn[]>()))
                .Callback<VoorraadMagazijn[]>(v => result = v);

            // Act
            databaseCacher.EnsureVoorraad();

            // Assert
            Assert.AreEqual(2, result.Count());

            VoorraadMagazijn item = result.Single(e => e.ArtikelNummer == 5);
            Assert.AreEqual("TestLeverancier", item.Leverancier);
            Assert.AreEqual("TL", item.Leveranciercode);
            Assert.AreEqual(2, item.Voorraad);
        }

        [TestMethod]
        public void EnsureKlanten_CallsReplayEventsOnEventReplayer()
        {
            // Arrange
            Mock<IVoorraadRepository> voorraadRepoMock = new Mock<IVoorraadRepository>();
            Mock<ICatalogusAgent> catalogusAgent = new Mock<ICatalogusAgent>();
            Mock<IEventReplayer> eventReplayerMock = new Mock<IEventReplayer>();
            Mock<IKlantRepository> klantRepositoryMock = new Mock<IKlantRepository>();
            Mock<IBestellingRepository> bestellingRepositoryMock = new Mock<IBestellingRepository>();
            Mock<IVoorraadAgent> voorraadAgentMock = new Mock<IVoorraadAgent>();
            IDatabaseCacher databaseCacher = new DatabaseCacher(voorraadAgentMock.Object, catalogusAgent.Object, voorraadRepoMock.Object, bestellingRepositoryMock.Object, klantRepositoryMock.Object, eventReplayerMock.Object, new NullLoggerFactory());

            klantRepositoryMock.Setup(e => e.IsEmpty()).Returns(true);

            TestBusContext busContext = new TestBusContext();

            // Act
            databaseCacher.EnsureKlanten(busContext);

            // Assert
            eventReplayerMock.Verify(e =>
                e.ReplayEvents(busContext, TopicNames.NieuweKlantAangemaakt, typeof(NieuweKlantAangemaaktEvent), It.IsAny<DateTime>()));
        }

        [TestMethod]
        public void EnsureKlanten_DoesNotCallCallReplayEventsOnEventReplayerIfDatabaseNotEmpty()
        {
            // Arrange
            Mock<IVoorraadRepository> voorraadRepoMock = new Mock<IVoorraadRepository>();
            Mock<ICatalogusAgent> catalogusAgent = new Mock<ICatalogusAgent>();
            Mock<IEventReplayer> eventReplayerMock = new Mock<IEventReplayer>();
            Mock<IKlantRepository> klantRepositoryMock = new Mock<IKlantRepository>();
            Mock<IBestellingRepository> bestellingRepositoryMock = new Mock<IBestellingRepository>();
            Mock<IVoorraadAgent> voorraadAgentMock = new Mock<IVoorraadAgent>();
            IDatabaseCacher databaseCacher = new DatabaseCacher(voorraadAgentMock.Object, catalogusAgent.Object, voorraadRepoMock.Object, bestellingRepositoryMock.Object, klantRepositoryMock.Object, eventReplayerMock.Object, new NullLoggerFactory());

            klantRepositoryMock.Setup(e => e.IsEmpty()).Returns(false);

            TestBusContext busContext = new TestBusContext();

            // Act
            databaseCacher.EnsureKlanten(busContext);

            // Assert
            eventReplayerMock.Verify(e =>
                e.ReplayEvents(busContext, TopicNames.NieuweKlantAangemaakt, typeof(NieuweKlantAangemaaktEvent), It.IsAny<DateTime>()), Times.Never);
        }

        [TestMethod]
        public void EnsureBestellingen_CallsReplayEventsOnEventReplayer()
        {
            // Arrange
            Mock<IVoorraadRepository> voorraadRepoMock = new Mock<IVoorraadRepository>();
            Mock<ICatalogusAgent> catalogusAgent = new Mock<ICatalogusAgent>();
            Mock<IEventReplayer> eventReplayerMock = new Mock<IEventReplayer>();
            Mock<IKlantRepository> klantRepositoryMock = new Mock<IKlantRepository>();
            Mock<IBestellingRepository> bestellingRepositoryMock = new Mock<IBestellingRepository>();
            Mock<IVoorraadAgent> voorraadAgentMock = new Mock<IVoorraadAgent>();
            IDatabaseCacher databaseCacher = new DatabaseCacher(voorraadAgentMock.Object, catalogusAgent.Object, voorraadRepoMock.Object, bestellingRepositoryMock.Object, klantRepositoryMock.Object, eventReplayerMock.Object, new NullLoggerFactory());

            bestellingRepositoryMock.Setup(e => e.IsEmpty()).Returns(true);

            TestBusContext busContext = new TestBusContext();

            // Act
            databaseCacher.EnsureBestellingen(busContext);

            // Assert
            eventReplayerMock.Verify(e =>
                e.ReplayEvents(busContext, TopicNames.NieuweBestellingAangemaakt, typeof(NieuweBestellingAangemaaktEvent), It.IsAny<DateTime>()));
            eventReplayerMock.Verify(e =>
                e.ReplayEvents(busContext, TopicNames.BestellingGoedgekeurd, typeof(BestellingGoedgekeurdEvent), It.IsAny<DateTime>()));
            eventReplayerMock.Verify(e =>
                e.ReplayEvents(busContext, TopicNames.BestellingAfgekeurd, typeof(BestellingAfgekeurdEvent), It.IsAny<DateTime>()));
            eventReplayerMock.Verify(e =>
                e.ReplayEvents(busContext, TopicNames.BestellingAdresLabelGeprint, typeof(BestellingAdresLabelGeprintEvent), It.IsAny<DateTime>()));
            eventReplayerMock.Verify(e =>
                e.ReplayEvents(busContext, TopicNames.BestellingFactuurGeprint, typeof(BestellingFactuurGeprintEvent), It.IsAny<DateTime>()));
            eventReplayerMock.Verify(e =>
                e.ReplayEvents(busContext, TopicNames.BestelRegelIngepakt, typeof(BestelRegelIngepaktEvent), It.IsAny<DateTime>()));
            eventReplayerMock.Verify(e =>
                e.ReplayEvents(busContext, TopicNames.BestellingKanKlaarGemeldWorden, typeof(BestellingKanKlaarGemeldWordenEvent), It.IsAny<DateTime>()));
            eventReplayerMock.Verify(e =>
                e.ReplayEvents(busContext, TopicNames.BestellingKanKlaarGemeldWorden, typeof(BestellingKanKlaarGemeldWordenEvent), It.IsAny<DateTime>()));
        }

        [TestMethod]
        public void EnsureBestellingen_DoesNotCallCallReplayEventsOnEventReplayerIfDatabaseNotEmpty()
        {
            // Arrange
            Mock<IVoorraadRepository> voorraadRepoMock = new Mock<IVoorraadRepository>();
            Mock<ICatalogusAgent> catalogusAgent = new Mock<ICatalogusAgent>();
            Mock<IEventReplayer> eventReplayerMock = new Mock<IEventReplayer>();
            Mock<IKlantRepository> klantRepositoryMock = new Mock<IKlantRepository>();
            Mock<IBestellingRepository> bestellingRepositoryMock = new Mock<IBestellingRepository>();
            Mock<IVoorraadAgent> voorraadAgentMock = new Mock<IVoorraadAgent>();
            IDatabaseCacher databaseCacher = new DatabaseCacher(voorraadAgentMock.Object, catalogusAgent.Object, voorraadRepoMock.Object, bestellingRepositoryMock.Object, klantRepositoryMock.Object, eventReplayerMock.Object, new NullLoggerFactory());

            bestellingRepositoryMock.Setup(e => e.IsEmpty()).Returns(false);

            TestBusContext busContext = new TestBusContext();

            // Act
            databaseCacher.EnsureBestellingen(busContext);

            // Assert
            eventReplayerMock.Verify(e =>
                e.ReplayEvents(busContext, TopicNames.NieuweKlantAangemaakt, typeof(NieuweKlantAangemaaktEvent), It.IsAny<DateTime>()), Times.Never);
        }
    }
}
