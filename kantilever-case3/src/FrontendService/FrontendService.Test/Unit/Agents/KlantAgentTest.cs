using FrontendService.Agents;
using FrontendService.Agents.Abstractions;
using FrontendService.Commands;
using FrontendService.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minor.Miffy.MicroServices.Commands;
using Moq;

namespace FrontendService.Test.Unit.Agents
{
    [TestClass]
    public class KlantAgentTest
    {
        [TestMethod]
        [DataRow(1, "Jan-Dirk")]
        [DataRow(5, "Peter")]
        public void MaakKlantAanAsync_CallsPublishAsyncWithKlant(long id, string naam)
        {
            // Arrange
            Mock<ICommandPublisher> commandPublisherMock = new Mock<ICommandPublisher>();
            IKlantAgent klantAgent = new KlantAgent(commandPublisherMock.Object);

            Klant inputKlant = new Klant
            {
                Id = id,
                Naam = naam
            };


            commandPublisherMock.Setup(e => e.PublishAsync<MaakNieuweKlantAanCommand>(
                    It.Is<MaakNieuweKlantAanCommand>(c => c.Klant.Equals(inputKlant))))
                .ReturnsAsync(new MaakNieuweKlantAanCommand {Klant = new Klant()})
                .Verifiable();

            // Act
            klantAgent.MaakKlantAanAsync(inputKlant).Wait();

            // Assert
            commandPublisherMock.Verify();
        }

        [TestMethod]
        [DataRow(1, "Jan-Dirk")]
        [DataRow(5, "Peter")]
        public void MaakKlantAanAsync_ReturnsExpectedKlant(long id, string naam)
        {
            // Arrange
            Mock<ICommandPublisher> commandPublisherMock = new Mock<ICommandPublisher>();
            IKlantAgent klantAgent = new KlantAgent(commandPublisherMock.Object);

            Klant inputKlant = new Klant
            {
                Naam = naam
            };

            Klant expectedKlant = new Klant
            {
                Id = id,
                Naam = naam
            };

            commandPublisherMock.Setup(e =>
                    e.PublishAsync<MaakNieuweKlantAanCommand>(It.IsAny<MaakNieuweKlantAanCommand>()))
                .ReturnsAsync(new MaakNieuweKlantAanCommand { Klant = expectedKlant });

            // Act
            Klant klant = klantAgent.MaakKlantAanAsync(inputKlant).Result;

            // Assert
            Assert.AreEqual(id, klant.Id);
            Assert.AreEqual(naam, klant.Naam);
        }
    }
}
