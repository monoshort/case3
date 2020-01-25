using FrontendService.EventListeners;
using FrontendService.Events;
using FrontendService.Models;
using FrontendService.Repositories.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FrontendService.Test.Unit.Listeners
{
    [TestClass]
    public class BestellingEventListenerTest
    {
        [TestMethod]
        [DataRow(43627432L, "62386518")]
        [DataRow(32476356L, "7376916")]
        public void HandleNieuweBestellingAangemaaktEvent_CallsAddBestellingOnBestellingRepo(long klantId, string bestellingNummer)
        {
            // Arrange
            var bestellingRepoMock = new Mock<IBestellingRepository>();
            var klantRepoMock = new Mock<IKlantRepository>();
            var target = new BestellingEventListeners(bestellingRepoMock.Object, klantRepoMock.Object);

            Klant klant = new Klant()
            {
                Id = klantId,
                Naam = "Jeroen",
                Telefoonnummer = "8392569214",
            };

            Bestelling bestelling = new Bestelling {BestellingNummer = bestellingNummer, Klant =  klant};
            bestellingRepoMock.Setup(repo => repo.Add(It.IsAny<Bestelling>()));
            klantRepoMock.Setup(repo => repo.GetById(klantId)).Returns(klant);

            NieuweBestellingAangemaaktEvent @event = new NieuweBestellingAangemaaktEvent
            {
                Bestelling = bestelling,
            };

            // Act
            target.HandleNieuweBestellingAangemaaktEvent(@event);

            // Assert
            bestellingRepoMock.Verify(mock => mock.Add(It.Is<Bestelling>(best => best.Equals(bestelling))), Times.Once);
        }

        [TestMethod]
        [DataRow(43627432L, "62386518")]
        [DataRow(32476356L, "7376916")]
        public void HandleNieuweBestellingAangemaaktEvent_CallsGetKlantByIdOnKlantRepository(long klantId, string bestellingNummer)
        {
            // Arrange
            var bestellingRepoMock = new Mock<IBestellingRepository>();
            var klantRepoMock = new Mock<IKlantRepository>();
            var target = new BestellingEventListeners(bestellingRepoMock.Object, klantRepoMock.Object);

            Klant klant = new Klant
            {
                Id = klantId,
                Naam = "Jeroen",
                Telefoonnummer = "8392569214",
            };

            Bestelling bestelling = new Bestelling {BestellingNummer = bestellingNummer, Klant =  klant};

            klantRepoMock.Setup(e => e.GetById(klantId))
                .Returns(klant)
                .Verifiable();

            NieuweBestellingAangemaaktEvent @event = new NieuweBestellingAangemaaktEvent
            {
                Bestelling = bestelling,
            };

            // Act
            target.HandleNieuweBestellingAangemaaktEvent(@event);

            // Assert
            klantRepoMock.Verify();
        }

        [TestMethod]
        [DataRow(43627432L, "62386518")]
        [DataRow(32476356L, "7376916")]
        [DataRow(347134L, "4792159127")]
        [DataRow(4712947L, "724661294")]
        [DataRow(2141412312L, "4y946124921")]
        public void HandleBestelingAfgekeurdEvent_GetsTheBestellingFomTheDbAndChangesAccordingly(long klantId, string bestellingNummer)
        {
            var bestellingRepoMock = new Mock<IBestellingRepository>();
            var klantRepoMock = new Mock<IKlantRepository>();
            var target = new BestellingEventListeners(bestellingRepoMock.Object, klantRepoMock.Object);

            Klant klant = new Klant
            {
                Id = klantId,
                Naam = "Jeroen",
                Telefoonnummer = "8392569214",
            };
            Bestelling bestelling = new Bestelling {Id = 37294124, BestellingNummer = bestellingNummer, Klant = klant, Afgekeurd = false, KlantId = 4792165 };
            bestellingRepoMock.Setup(mock => mock.GetById(37294124)).Returns(bestelling);

            BestellingAfgekeurdEvent @event = new BestellingAfgekeurdEvent
            {
                BestellingId = bestelling.Id,
            };

            // Act
            target.HandleBestellingAfgekeurdEvent(@event);

            // Assert
            bestellingRepoMock.Verify(mock => mock.Update(It.Is<Bestelling>(best => best.BestellingNummer == bestellingNummer || best.Afgekeurd == true)), Times.Once);
        }

        [TestMethod]
        [DataRow(43627432L, "62386518")]
        [DataRow(32476356L, "7376916")]
        [DataRow(347134L, "4792159127")]
        [DataRow(4712947L, "724661294")]
        [DataRow(2141412312L, "4y946124921")]
        public void HandleBestelingKlaargemeldEvent_GetsTheBestellingFomTheDbAndChangesAccordingly(long klantId, string bestellingNummer)
        {
            var bestellingRepoMock = new Mock<IBestellingRepository>();
            var klantRepoMock = new Mock<IKlantRepository>();
            var target = new BestellingEventListeners(bestellingRepoMock.Object, klantRepoMock.Object);

            Klant klant = new Klant
            {
                Id = klantId,
                Naam = "Jeroen",
                Telefoonnummer = "8392569214",
            };
            Bestelling bestelling = new Bestelling { Id = 37294124, BestellingNummer = bestellingNummer, Klant = klant, KlaarGemeld = false, KlantId = 4792165 };
            bestellingRepoMock.Setup(mock => mock.GetById(37294124)).Returns(bestelling);

            BestellingKlaarGemeldEvent @event = new BestellingKlaarGemeldEvent
            {
                BestellingId = bestelling.Id,
            };

            // Act
            target.HandleBestellingKlaargemeldEvent(@event);

            // Assert
            bestellingRepoMock.Verify(mock => mock.Update(It.Is<Bestelling>(best => best.BestellingNummer == bestellingNummer || best.KlaarGemeld == true)), Times.Once);
        }

        [TestMethod]
        [DataRow(43627432L, "62386518")]
        [DataRow(32476356L, "7376916")]
        [DataRow(347134L, "4792159127")]
        [DataRow(4712947L, "724661294")]
        [DataRow(2141412312L, "4y946124921")]
        public void HandleBestelingGoedgekeurdEvent_GetsTheBestellingFomTheDbAndChangesAccordingly(long klantId, string bestellingNummer)
        {
            var bestellingRepoMock = new Mock<IBestellingRepository>();
            var klantRepoMock = new Mock<IKlantRepository>();
            var target = new BestellingEventListeners(bestellingRepoMock.Object, klantRepoMock.Object);

            Klant klant = new Klant
            {
                Id = klantId,
                Naam = "Jeroen",
                Telefoonnummer = "8392569214",
            };
            Bestelling bestelling = new Bestelling { Id = 37294124, BestellingNummer = bestellingNummer, Klant = klant, Goedgekeurd = false, KlantId = 4792165 };
            bestellingRepoMock.Setup(mock => mock.GetById(37294124)).Returns(bestelling);

            BestellingGoedgekeurdEvent @event = new BestellingGoedgekeurdEvent
            {
                BestellingId = bestelling.Id,
            };

            // Act
            target.HandleBestellingGoedgekeurdEvent(@event);

            // Assert
            bestellingRepoMock.Verify(mock => mock.Update(It.Is<Bestelling>(best => best.BestellingNummer == bestellingNummer || best.Goedgekeurd == true)), Times.Once);
        }
    }
}
