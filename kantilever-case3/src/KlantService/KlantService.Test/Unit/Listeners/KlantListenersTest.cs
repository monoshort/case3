using KlantService.Commands;
using KlantService.Events;
using KlantService.Listeners;
using KlantService.Models;
using KlantService.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minor.Miffy.MicroServices.Events;
using Moq;

namespace KlantService.Test.Unit.Listeners
{
    [TestClass]
    public class KlantListenersTest
    {
        [TestMethod]
        public void HandleNieuwKlantCommand_CallsAddOnRepository()
        {
            // Arrange
            Mock<IKlantRepository> klantRepositoryMock = new Mock<IKlantRepository>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();

            KlantListeners klantListeners = new KlantListeners(klantRepositoryMock.Object, eventPublisherMock.Object);

            MaakNieuweKlantAanCommand command = new MaakNieuweKlantAanCommand { Klant = new Klant() };

            // Act
            klantListeners.HandleNieuwKlantCommand(command);

            // Assert
            klantRepositoryMock.Verify(e => e.Add(It.IsAny<Klant>()));
        }

        [TestMethod]
        [DataRow("Jan de Wild")]
        [DataRow("Luna Jannekes")]
        public void HandleNieuweKlantCommand_CallsAddOnRepositoryWithKlant(string naam)
        {
            // Arrange
            Mock<IKlantRepository> klantRepositoryMock = new Mock<IKlantRepository>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();

            KlantListeners klantListeners = new KlantListeners(klantRepositoryMock.Object, eventPublisherMock.Object);

            Klant klant = new Klant
            {
                Naam = naam
            };
            MaakNieuweKlantAanCommand command = new MaakNieuweKlantAanCommand { Klant = klant };

            // Act
            klantListeners.HandleNieuwKlantCommand(command);

            // Assert
            klantRepositoryMock.Verify(e => e.Add(klant));
        }

        [TestMethod]
        public void HandleNieuweKlantCommand_CallsPublishAsyncOnEventPublisher()
        {
            // Arrange
            Mock<IKlantRepository> klantRepositoryMock = new Mock<IKlantRepository>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();

            KlantListeners klantListeners = new KlantListeners(klantRepositoryMock.Object, eventPublisherMock.Object);

            MaakNieuweKlantAanCommand command = new MaakNieuweKlantAanCommand { Klant = new Klant() };

            // Act
            klantListeners.HandleNieuwKlantCommand(command);

            // Assert
            eventPublisherMock.Verify(e => e.PublishAsync(It.IsAny<NieuweKlantAangemaaktEvent>()));
        }

        [TestMethod]
        [DataRow("Jan de Man")]
        [DataRow("Peter van de Hoek")]
        public void HandleNieuweKlantCommand_CallsPublishAsyncOnEventPublisherWithKlant(string naam)
        {
            // Arrange
            Mock<IKlantRepository> klantRepositoryMock = new Mock<IKlantRepository>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();

            KlantListeners klantListeners = new KlantListeners(klantRepositoryMock.Object, eventPublisherMock.Object);

            Klant klant = new Klant
            {
                Naam = naam
            };
            MaakNieuweKlantAanCommand command = new MaakNieuweKlantAanCommand { Klant = klant };

            // Act
            klantListeners.HandleNieuwKlantCommand(command);

            // Assert
            eventPublisherMock.Verify(e =>
                e.PublishAsync(It.Is<NieuweKlantAangemaaktEvent>(ev => ev.Klant.Equals(klant))));
        }

        [TestMethod]
        public void HandleNieuweKlantCommand_ReturnsKlantCommand()
        {
            // Arrange
            Mock<IKlantRepository> klantRepositoryMock = new Mock<IKlantRepository>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();

            KlantListeners klantListeners = new KlantListeners(klantRepositoryMock.Object, eventPublisherMock.Object);

            MaakNieuweKlantAanCommand command = new MaakNieuweKlantAanCommand { Klant = new Klant() };

            // Act
            MaakNieuweKlantAanCommand result = klantListeners.HandleNieuwKlantCommand(command);

            // Assert
            Assert.AreSame(command, result);
        }
    }
}
