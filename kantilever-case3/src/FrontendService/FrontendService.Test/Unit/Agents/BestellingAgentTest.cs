using FrontendService.Agents;
using FrontendService.Commands;
using FrontendService.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minor.Miffy.MicroServices.Commands;
using Moq;

namespace FrontendService.Test.Unit.Agents
{
    [TestClass]
    public class BestellingAgentTest
    {
        [TestMethod]
        public void Bestel_CallsPublishAsyncOnCommandPublisherWithBestellingCommand()
        {
            // Arrange
            Mock<ICommandPublisher> commandPublisherMock = new Mock<ICommandPublisher>();
            BestellingAgent agent = new BestellingAgent(commandPublisherMock.Object);

            commandPublisherMock.Setup(e => e.PublishAsync<MaakNieuweKlantAanCommand>(It.IsAny<MaakNieuweKlantAanCommand>()))
                .ReturnsAsync(() => new MaakNieuweKlantAanCommand { Klant = new Klant() });

            // Act
            agent.Bestel(new Bestelling { Klant = new Klant() });

            // Assert
            commandPublisherMock.Verify(
                e => e.PublishAsync<MaakNieuweBestellingAanCommand>(It.IsAny<MaakNieuweBestellingAanCommand>()));
        }

        [TestMethod]
        [DataRow(20)]
        [DataRow(194)]
        public void Bestel_CallsPublishAsyncOnCommandPublisherWithBestelling(int bestellingId)
        {
            // Arrange
            Mock<ICommandPublisher> commandPublisherMock = new Mock<ICommandPublisher>();
            BestellingAgent agent = new BestellingAgent(commandPublisherMock.Object);

            commandPublisherMock.Setup(e => e.PublishAsync<MaakNieuweKlantAanCommand>(It.IsAny<MaakNieuweKlantAanCommand>()))
                .ReturnsAsync(new MaakNieuweKlantAanCommand { Klant = new Klant() });

            Bestelling bestelling = new Bestelling { Id = bestellingId, Klant = new Klant() };

            // Act
            agent.Bestel(bestelling);

            // Assert
            commandPublisherMock.Verify(
                e => e.PublishAsync<MaakNieuweBestellingAanCommand>(It.Is<MaakNieuweBestellingAanCommand>(b => b.Bestelling.Equals(bestelling))));
        }
    }
}
