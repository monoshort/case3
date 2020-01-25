using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FrontendService.Agents.Abstractions;
using FrontendService.Constants;
using FrontendService.Events;
using FrontendService.Models;
using FrontendService.Repositories.Abstractions;
using FrontendService.Seeding;
using FrontendService.Seeding.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minor.Miffy;
using Minor.Miffy.TestBus;
using Moq;
using RabbitMQ.Client;

namespace FrontendService.Test.Unit.Seeding
{
    [TestClass]
    public class DatabaseCacherTest
    {
        private static readonly ILoggerFactory Logger = new LoggerFactory();

        [TestMethod]
        public void EnsureArtikelen_CallsGetAllVoorraadIfDatabaseIsEmpty()
        {
            // Arrange
            var artikelRepoMock = new Mock<IArtikelRepository>();
            var klantRepoMock = new Mock<IKlantRepository>();
            var bestellingRepoMock = new Mock<IBestellingRepository>();
            var agentMock = new Mock<ICatalogusAgent>();
            var eventReplayer = new Mock<IEventReplayer>();
            var voorraadMock = new Mock<IVoorraadAgent>();

            artikelRepoMock.Setup(e => e.IsEmpty()).Returns(true);
            voorraadMock.Setup(a => a.GetAllVoorraadAsync())
                .ReturnsAsync(Enumerable.Empty<VoorraadMagazijn>()).Verifiable();

            agentMock.Setup(a => a.GetAlleArtikelenAsync())
                .ReturnsAsync(Enumerable.Empty<Artikel>());

            DatabaseCacher databasecacher = new DatabaseCacher(artikelRepoMock.Object, agentMock.Object, voorraadMock.Object, klantRepoMock.Object, bestellingRepoMock.Object, eventReplayer.Object, Logger);

            // Act
            databasecacher.EnsureArtikelen();

            // Assert
            voorraadMock.Verify();
        }

        [TestMethod]
        public void EnsureArtikelen_CallsGetAlleArtikelenIfDatabaseIsEmpty()
        {
            // Arrange
            var artikelRepoMock = new Mock<IArtikelRepository>();
            var klantRepoMock = new Mock<IKlantRepository>();
            var bestellingRepoMock = new Mock<IBestellingRepository>();
            var eventReplayerMock = new Mock<IEventReplayer>();
            var catalogusMock = new Mock<ICatalogusAgent>();
            var voorraadMock = new Mock<IVoorraadAgent>();

            voorraadMock.Setup(a => a.GetAllVoorraadAsync())
                .ReturnsAsync(Enumerable.Empty<VoorraadMagazijn>());
            catalogusMock.Setup(a => a.GetAlleArtikelenAsync())
                .ReturnsAsync(Enumerable.Empty<Artikel>()).Verifiable();

            DatabaseCacher databasecacher = new DatabaseCacher(artikelRepoMock.Object, catalogusMock.Object, voorraadMock.Object, klantRepoMock.Object, bestellingRepoMock.Object, eventReplayerMock.Object, Logger);

            // Act
            databasecacher.EnsureArtikelen();

            // Assert
            voorraadMock.Verify();
        }

        [TestMethod]
        public void EnsureArtikelen_DoesntCallGetAllVoorraadIfDatabaseIsNotEmpty()
        {
            // Arrange
            var artikelRepoMock = new Mock<IArtikelRepository>();
            var klantRepoMock = new Mock<IKlantRepository>();
            var bestellingRepoMock = new Mock<IBestellingRepository>();
            var agentMock = new Mock<ICatalogusAgent>();
            var eventReplayer = new Mock<IEventReplayer>();
            var voorraadMock = new Mock<IVoorraadAgent>();

            artikelRepoMock.Setup(e => e.IsEmpty()).Returns(false);

            DatabaseCacher databasecacher = new DatabaseCacher(artikelRepoMock.Object, agentMock.Object, voorraadMock.Object, klantRepoMock.Object, bestellingRepoMock.Object, eventReplayer.Object, Logger);

            // Act
            databasecacher.EnsureArtikelen();

            // Assert
            voorraadMock.Verify(x => x.GetAllVoorraadAsync(), Times.Never);
        }

        [TestMethod]
        public void EnsureArtikelen_HandlesNullFromVoorraadGracefully()
        {
            // Arrange
            var artikelRepoMock = new Mock<IArtikelRepository>();
            var klantRepoMock = new Mock<IKlantRepository>();
            var bestellingRepoMock = new Mock<IBestellingRepository>();
            var agentMock = new Mock<ICatalogusAgent>();
            var eventReplayer = new Mock<IEventReplayer>();
            var voorraadMock = new Mock<IVoorraadAgent>();

            voorraadMock.Setup(a => a.GetAllVoorraadAsync())
                .ReturnsAsync(new List<VoorraadMagazijn>().Select(e => e));

            IEnumerable<Artikel> mockData = new List<Artikel>
            {
                new Artikel {Artikelnummer = 1, Naam = "Eerste Artikel"},
                new Artikel {Artikelnummer = 2, Naam = "Tweede Artikel"}
            };

            artikelRepoMock.Setup(e => e.IsEmpty()).Returns(true);
            agentMock.Setup(a => a.GetAlleArtikelenAsync())
                .ReturnsAsync(mockData);

            DatabaseCacher databasecacher = new DatabaseCacher(artikelRepoMock.Object, agentMock.Object, voorraadMock.Object, klantRepoMock.Object, bestellingRepoMock.Object, eventReplayer.Object, Logger);

            // Act
            databasecacher.EnsureArtikelen();

            // Assert
            artikelRepoMock.Verify(e => e.Add(It.IsAny<Artikel[]>()), Times.Once);
        }

        [TestMethod]
        public void EnsureArtikelen_HandlesNullFromArtikelenGracefully()
        {
            // Arrange
            var artikelRepoMock = new Mock<IArtikelRepository>();
            var klantRepoMock = new Mock<IKlantRepository>();
            var bestellingRepoMock = new Mock<IBestellingRepository>();
            var agentMock = new Mock<ICatalogusAgent>();
            var eventReplayer = new Mock<IEventReplayer>();
            var voorraadMock = new Mock<IVoorraadAgent>();

            IEnumerable<VoorraadMagazijn> mockData = new List<VoorraadMagazijn>
            {
                new VoorraadMagazijn {ArtikelNummer = 1, Voorraad = 4},
            };

            artikelRepoMock.Setup(e => e.IsEmpty()).Returns(true);
            voorraadMock.Setup(a => a.GetAllVoorraadAsync())
                .ReturnsAsync(mockData);

            agentMock.Setup(a => a.GetAlleArtikelenAsync());

            DatabaseCacher databasecacher = new DatabaseCacher(artikelRepoMock.Object, agentMock.Object, voorraadMock.Object, klantRepoMock.Object, bestellingRepoMock.Object, eventReplayer.Object, Logger);

            // Act
            databasecacher.EnsureArtikelen();

            // Assert
            artikelRepoMock.Verify(e =>
                e.Add(It.Is<Artikel[]>(list => !list.Any())));
        }

        [TestMethod]
        public void EnsureArtikelen_CallsFillsDatabaseWithArtikelenFromGetAlleArtikelen()
        {
            // Arrange
            var artikelRepoMock = new Mock<IArtikelRepository>();
            var klantRepoMock = new Mock<IKlantRepository>();
            var bestellingRepoMock = new Mock<IBestellingRepository>();
            var agentMock = new Mock<ICatalogusAgent>();
            var eventReplayer = new Mock<IEventReplayer>();
            var voorraadMock = new Mock<IVoorraadAgent>();

            IEnumerable<Artikel> mockData = new List<Artikel>
            {
                new Artikel {Artikelnummer = 1, Naam = "Eerste Artikel"},
                new Artikel {Artikelnummer = 2, Naam = "Tweede Artikel"}
            };

            artikelRepoMock.Setup(e => e.IsEmpty()).Returns(true);
            voorraadMock.Setup(a => a.GetAllVoorraadAsync())
                .ReturnsAsync(Enumerable.Empty<VoorraadMagazijn>());
            agentMock.Setup(a => a.GetAlleArtikelenAsync())
                .ReturnsAsync(mockData);

            DatabaseCacher databasecacher = new DatabaseCacher(artikelRepoMock.Object, agentMock.Object, voorraadMock.Object, klantRepoMock.Object, bestellingRepoMock.Object, eventReplayer.Object, Logger);

            // Act
            databasecacher.EnsureArtikelen();

            // Assert
            Expression<Func<Artikel[], bool>> match = artikelList =>
                artikelList.Any(a => a.Artikelnummer == 1 && a.Naam == "Eerste Artikel")
                && artikelList.Any(a => a.Artikelnummer == 2 && a.Naam == "Tweede Artikel");

            artikelRepoMock.Verify(e => e.Add(It.Is(match)));
        }

        [TestMethod]
        [DataRow(1, 2)]
        [DataRow(5, 7)]
        [DataRow(492, 1058)]
        public void EnsureArtikelen_CombinesResultsFromCatalogusAndVoorraadProperly(int artikel1Voorraad, int artikel2Voorraad)
        {
            // Arrange
            var artikelRepoMock = new Mock<IArtikelRepository>();
            var klantRepoMock = new Mock<IKlantRepository>();
            var bestellingRepoMock = new Mock<IBestellingRepository>();
            var agentMock = new Mock<ICatalogusAgent>();
            var eventReplayer = new Mock<IEventReplayer>();
            var voorraadMock = new Mock<IVoorraadAgent>();

            IEnumerable<VoorraadMagazijn> mockVoorraadData = new List<VoorraadMagazijn>
            {
                new VoorraadMagazijn {ArtikelNummer = 1, Voorraad = artikel1Voorraad},
                new VoorraadMagazijn {ArtikelNummer = 2, Voorraad = artikel2Voorraad}
            };

            IEnumerable<Artikel> mockArtikelData = new List<Artikel>
            {
                new Artikel { Artikelnummer = 1 },
                new Artikel { Artikelnummer = 2 }
            };

            artikelRepoMock.Setup(e => e.IsEmpty()).Returns(true);
            voorraadMock.Setup(a => a.GetAllVoorraadAsync())
                .ReturnsAsync(mockVoorraadData);
            agentMock.Setup(a => a.GetAlleArtikelenAsync())
                .ReturnsAsync(mockArtikelData);

            DatabaseCacher databasecacher = new DatabaseCacher(artikelRepoMock.Object, agentMock.Object, voorraadMock.Object, klantRepoMock.Object, bestellingRepoMock.Object, eventReplayer.Object, Logger);

            // Act
            databasecacher.EnsureArtikelen();

            // Assert
            Expression<Func<Artikel[], bool>> match = artikelList =>
                artikelList.Any(a => a.Artikelnummer == 1 && a.Voorraad == artikel1Voorraad)
                && artikelList.Any(a => a.Artikelnummer == 2 && a.Voorraad == artikel2Voorraad);

            artikelRepoMock.Verify(e => e.Add(It.Is(match)));
        }

        [TestMethod]
        public void EnsureKlanten_DoesNothingIfKlantenExist()
        {
            // Arrange
            var artikelRepoMock = new Mock<IArtikelRepository>();
            var klantRepoMock = new Mock<IKlantRepository>();
            var bestellingRepoMock = new Mock<IBestellingRepository>();
            var agentMock = new Mock<ICatalogusAgent>();
            var eventReplayer = new Mock<IEventReplayer>();
            var voorraadMock = new Mock<IVoorraadAgent>();

            klantRepoMock.Setup(e => e.IsEmpty()).Returns(false);

            DatabaseCacher databasecacher = new DatabaseCacher(artikelRepoMock.Object, agentMock.Object, voorraadMock.Object, klantRepoMock.Object, bestellingRepoMock.Object, eventReplayer.Object, Logger);

            // Act
            databasecacher.EnsureKlanten(null);

            // Assert
            eventReplayer.Verify(e =>
                e.ReplayEvents(It.IsAny<IBusContext<IConnection>>(), It.IsAny<string>(), It.IsAny<Type>(), It.IsAny<DateTime>()), Times.Never);
        }

        [TestMethod]
        public void EnsureKlanten_CallsReplayEventsWithProperValues()
        {
            // Arrange
            var artikelRepoMock = new Mock<IArtikelRepository>();
            var klantRepoMock = new Mock<IKlantRepository>();
            var bestellingRepoMock = new Mock<IBestellingRepository>();
            var agentMock = new Mock<ICatalogusAgent>();
            var eventReplayer = new Mock<IEventReplayer>();
            var voorraadMock = new Mock<IVoorraadAgent>();

            klantRepoMock.Setup(e => e.IsEmpty()).Returns(true);

            DatabaseCacher databasecacher = new DatabaseCacher(artikelRepoMock.Object, agentMock.Object, voorraadMock.Object, klantRepoMock.Object, bestellingRepoMock.Object, eventReplayer.Object, Logger);

            IBusContext<IConnection> context = new TestBusContext();

            // Act
            databasecacher.EnsureKlanten(context);

            // Assert
            eventReplayer.Verify(e => e.ReplayEvents(context, TopicNames.NieuweKlantAangemaakt, typeof(NieuweKlantAangemaaktEvent), It.IsAny<DateTime>()));
        }

        [TestMethod]
        public void EnsureBestellingen_DoesNothingIfBetellingenExist()
        {
            // Arrange
            var artikelRepoMock = new Mock<IArtikelRepository>();
            var klantRepoMock = new Mock<IKlantRepository>();
            var bestellingRepoMock = new Mock<IBestellingRepository>();
            var agentMock = new Mock<ICatalogusAgent>();
            var eventReplayer = new Mock<IEventReplayer>();
            var voorraadMock = new Mock<IVoorraadAgent>();

            bestellingRepoMock.Setup(e => e.IsEmpty()).Returns(false);

            DatabaseCacher databasecacher = new DatabaseCacher(artikelRepoMock.Object, agentMock.Object, voorraadMock.Object, klantRepoMock.Object, bestellingRepoMock.Object, eventReplayer.Object, Logger);

            // Act
            databasecacher.EnsureBestellingen(null);

            // Assert
            eventReplayer.Verify(e =>
                e.ReplayEvents(It.IsAny<IBusContext<IConnection>>(), It.IsAny<string>(), It.IsAny<Type>(), It.IsAny<DateTime>()), Times.Never);
        }

        [TestMethod]
        public void EnsureBestellingen_CallsReplayEventsWithProperValues()
        {
            // Arrange
            var artikelRepoMock = new Mock<IArtikelRepository>();
            var klantRepoMock = new Mock<IKlantRepository>();
            var bestellingRepoMock = new Mock<IBestellingRepository>();
            var agentMock = new Mock<ICatalogusAgent>();
            var eventReplayer = new Mock<IEventReplayer>();
            var voorraadMock = new Mock<IVoorraadAgent>();

            bestellingRepoMock.Setup(e => e.IsEmpty()).Returns(true);

            DatabaseCacher databasecacher = new DatabaseCacher(artikelRepoMock.Object, agentMock.Object, voorraadMock.Object, klantRepoMock.Object, bestellingRepoMock.Object, eventReplayer.Object, Logger);

            IBusContext<IConnection> context = new TestBusContext();

            // Act
            databasecacher.EnsureBestellingen(context);

            // Assert
            eventReplayer.Verify(e => e.ReplayEvents(context, TopicNames.NieuweBestellingAangemaakt, typeof(NieuweBestellingAangemaaktEvent), It.IsAny<DateTime>()));
            eventReplayer.Verify(e => e.ReplayEvents(context, TopicNames.BestellingGoedgekeurd, typeof(BestellingGoedgekeurdEvent), It.IsAny<DateTime>()));
            eventReplayer.Verify(e => e.ReplayEvents(context, TopicNames.BestellingAfgekeurd, typeof(BestellingAfgekeurdEvent), It.IsAny<DateTime>()));
        }
    }
}
