using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BackOfficeFrontendService.Agents;
using BackOfficeFrontendService.Commands;
using BackOfficeFrontendService.Constants;
using BackOfficeFrontendService.Exceptions;
using BackOfficeFrontendService.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minor.Miffy;
using Minor.Miffy.MicroServices.Commands;
using Moq;

namespace BackOfficeFrontendService.Test.Unit.Agents
{
    [TestClass]
    public class BestellingAgentTest
    {
        [TestMethod]
        [DataRow(298)]
        [DataRow(1984)]
        public void KeurBestellingGoed_CallsPublishAsyncOnCommandPublisher(int bestellingId)
        {
            // Arrange
            Mock<ICommandPublisher> commandPublisherMock = new Mock<ICommandPublisher>();
            BestellingAgent agent = new BestellingAgent(commandPublisherMock.Object);

            // Act
            agent.KeurBestellingGoedAsync(bestellingId).Wait();

            // Assert
            commandPublisherMock.Verify(e =>
                e.PublishAsync<KeurBestellingGoedCommand>(It.IsAny<KeurBestellingGoedCommand>()));
        }

        [TestMethod]
        [DataRow(298)]
        [DataRow(1984)]
        public void KeurBestellingGoed_CallsPublishAsyncOnCommandPublisherWithBestellingId(int bestellingId)
        {
            // Arrange
            Mock<ICommandPublisher> commandPublisherMock = new Mock<ICommandPublisher>();
            BestellingAgent agent = new BestellingAgent(commandPublisherMock.Object);

            // Act
            agent.KeurBestellingGoedAsync(bestellingId).Wait();

            // Assert
            commandPublisherMock.Verify(e =>
                e.PublishAsync<KeurBestellingGoedCommand>(It.Is<KeurBestellingGoedCommand>(cmd => cmd.BestellingId == bestellingId)));
        }

        [TestMethod]
        [DataRow(298)]
        [DataRow(1984)]
        public void KeurBestellingGoed_ThrowsFunctionalExceptionWhenSequenceIsEmpty(int bestellingId)
        {
            // Arrange
            Mock<ICommandPublisher> commandPublisherMock = new Mock<ICommandPublisher>();
            commandPublisherMock.Setup(publisher => publisher.PublishAsync<KeurBestellingGoedCommand>(It.IsAny<KeurBestellingGoedCommand>())).Throws(new DestinationQueueException("", new Exception("Sequence contains no elements"), "", "", new Guid()));
            BestellingAgent agent = new BestellingAgent(commandPublisherMock.Object);

            // Act
            async Task Act() => await agent.KeurBestellingGoedAsync(bestellingId);

            // Assert
            var exception = Assert.ThrowsExceptionAsync<FunctionalException>(Act);
            Assert.AreEqual(FunctionalExceptionMessages.BestellingNotFound, exception.Result.Message);
        }

        [TestMethod]
        [DataRow(298)]
        [DataRow(1984)]
        public void KeurBestellingAf_CallsPublishAsyncOnCommandPublisher(int bestellingId)
        {
            // Arrange
            Mock<ICommandPublisher> commandPublisherMock = new Mock<ICommandPublisher>();
            BestellingAgent agent = new BestellingAgent(commandPublisherMock.Object);

            // Act
            agent.KeurBestellingAfAsync(bestellingId).Wait();

            // Assert
            commandPublisherMock.Verify(e =>
                e.PublishAsync<KeurBestellingAfCommand>(It.IsAny<KeurBestellingAfCommand>()));
        }

        [TestMethod]
        [DataRow(298)]
        [DataRow(1984)]
        public void KeurBestellingAf_CallsPublishAsyncOnCommandPublisherWithBestellingId(int bestellingId)
        {
            // Arrange
            Mock<ICommandPublisher> commandPublisherMock = new Mock<ICommandPublisher>();
            BestellingAgent agent = new BestellingAgent(commandPublisherMock.Object);

            // Act
            agent.KeurBestellingAfAsync(bestellingId).Wait();

            // Assert
            commandPublisherMock.Verify(e =>
                e.PublishAsync<KeurBestellingAfCommand>(It.Is<KeurBestellingAfCommand>(cmd => cmd.BestellingId == bestellingId)));
        }

        [TestMethod]
        [DataRow(298)]
        [DataRow(1984)]
        public void KeurBestellingAf_ThrowsFunctionalExceptionWhenSequenceIsEmpty(int bestellingId)
        {
            // Arrange
            Mock<ICommandPublisher> commandPublisherMock = new Mock<ICommandPublisher>();
            commandPublisherMock.Setup(publisher => publisher.PublishAsync<KeurBestellingAfCommand>(It.IsAny<KeurBestellingAfCommand>())).Throws(new DestinationQueueException("", new Exception("Sequence contains no elements"), "", "", new Guid()));
            BestellingAgent agent = new BestellingAgent(commandPublisherMock.Object);

            // Act
            async Task Act() => await agent.KeurBestellingAfAsync(bestellingId);

            // Assert
            var exception = Assert.ThrowsExceptionAsync<FunctionalException>(Act);
            Assert.AreEqual(FunctionalExceptionMessages.BestellingNotFound, exception.Result.Message);
        }

        [TestMethod]
        [DataRow(298)]
        [DataRow(1984)]
        public void MeldBestellingKlaar_CallsPublishAsyncOnCommandPublisher(int bestellingId)
        {
            // Arrange
            Mock<ICommandPublisher> commandPublisherMock = new Mock<ICommandPublisher>();
            BestellingAgent agent = new BestellingAgent(commandPublisherMock.Object);

            // Act
            agent.MeldBestellingKlaarAsync(bestellingId).Wait();

            // Assert
            commandPublisherMock.Verify(e =>
                e.PublishAsync<MeldBestellingKlaarCommand>(It.IsAny<MeldBestellingKlaarCommand>()));
        }

        [TestMethod]
        [DataRow(298)]
        [DataRow(1984)]
        public void MeldBestellingKlaar_CallsPublishAsyncOnCommandPublisherWithBestellingId(int bestellingId)
        {
            // Arrange
            Mock<ICommandPublisher> commandPublisherMock = new Mock<ICommandPublisher>();
            BestellingAgent agent = new BestellingAgent(commandPublisherMock.Object);

            // Act
            agent.MeldBestellingKlaarAsync(bestellingId).Wait();

            // Assert
            commandPublisherMock.Verify(e =>
                e.PublishAsync<MeldBestellingKlaarCommand>(It.Is<MeldBestellingKlaarCommand>(cmd => cmd.BestellingId == bestellingId)));
        }

        [TestMethod]
        [DataRow(298, 20)]
        [DataRow(1984, 1000)]
        public void PakBestelregelIn_CallsPublishAsyncOnCommandPublisher(int bestellingId, int bestelregelId)
        {
            // Arrange
            Mock<ICommandPublisher> commandPublisherMock = new Mock<ICommandPublisher>();
            BestellingAgent agent = new BestellingAgent(commandPublisherMock.Object);

            // Act
            agent.PakBestelregelInAsync(bestellingId, bestelregelId).Wait();

            // Assert
            commandPublisherMock.Verify(e =>
                e.PublishAsync<PakBestelRegelInCommand>(It.IsAny<PakBestelRegelInCommand>()));
        }

        [TestMethod]
        [DataRow(298, 20)]
        [DataRow(1984, 1000)]
        public void PakBestelregelIn_CallsPublishAsyncOnCommandPublisherWithBestellingId(int bestellingId, int bestelregelId)
        {
            // Arrange
            Mock<ICommandPublisher> commandPublisherMock = new Mock<ICommandPublisher>();
            BestellingAgent agent = new BestellingAgent(commandPublisherMock.Object);

            // Act
            agent.PakBestelregelInAsync(bestellingId, bestelregelId).Wait();

            // Assert
            commandPublisherMock.Verify(e =>
                e.PublishAsync<PakBestelRegelInCommand>(It.Is<PakBestelRegelInCommand>(cmd => cmd.BestellingId == bestellingId && cmd.BestelRegelId == bestelregelId)));
        }

        [TestMethod]
        [DataRow(298)]
        [DataRow(1984)]
        public void PrintAdresLabel_CallsPublishAsyncOnCommandPublisher(int bestellingId)
        {
            // Arrange
            Mock<ICommandPublisher> commandPublisherMock = new Mock<ICommandPublisher>();
            BestellingAgent agent = new BestellingAgent(commandPublisherMock.Object);

            // Act
            agent.BestellingPrintAdresLabelAsync(bestellingId).Wait();

            // Assert
            commandPublisherMock.Verify(e =>
                e.PublishAsync<PrintAdresLabelCommand>(It.IsAny<PrintAdresLabelCommand>()));
        }

        [TestMethod]
        [DataRow(298)]
        [DataRow(1984)]
        public void PrintAdresLabel_CallsPublishAsyncOnCommandPublisherWithBestellingId(int bestellingId)
        {
            // Arrange
            Mock<ICommandPublisher> commandPublisherMock = new Mock<ICommandPublisher>();
            BestellingAgent agent = new BestellingAgent(commandPublisherMock.Object);

            // Act
            agent.BestellingPrintAdresLabelAsync(bestellingId).Wait();

            // Assert
            commandPublisherMock.Verify(e =>
                e.PublishAsync<PrintAdresLabelCommand>(It.Is<PrintAdresLabelCommand>(cmd => cmd.BestellingId == bestellingId)));
        }

        [TestMethod]
        [DataRow(298)]
        [DataRow(1984)]
        public void PrintFactuur_CallsPublishAsyncOnCommandPublisherWithBestellingId(int bestellingId)
        {
            // Arrange
            Mock<ICommandPublisher> commandPublisherMock = new Mock<ICommandPublisher>();
            BestellingAgent agent = new BestellingAgent(commandPublisherMock.Object);

            // Act
            agent.BestellingPrintFactuurAsync(bestellingId).Wait();

            // Assert
            commandPublisherMock.Verify(e =>
                e.PublishAsync<PrintFactuurCommand>(It.Is<PrintFactuurCommand>(cmd => cmd.BestellingId == bestellingId)));
        }

        [TestMethod]
        [DataRow(298)]
        [DataRow(1984)]
        public void PrintFactuur_CallsPublishAsyncOnCommandPublisher(int bestellingId)
        {
            // Arrange
            Mock<ICommandPublisher> commandPublisherMock = new Mock<ICommandPublisher>();
            BestellingAgent agent = new BestellingAgent(commandPublisherMock.Object);

            // Act
            agent.BestellingPrintFactuurAsync(bestellingId).Wait();

            // Assert
            commandPublisherMock.Verify(e =>
                e.PublishAsync<PrintFactuurCommand>(It.IsAny<PrintFactuurCommand>()));
        }

        [DataRow(298)]
        [DataRow(1984)]
        [TestMethod]
        public void MeldBestellingKlaar_CallsPublishAsyncOnCommandPublisher(long bestellingId)
        {
            Mock<ICommandPublisher> commandPublisherMock = new Mock<ICommandPublisher>();
            BestellingAgent agent = new BestellingAgent(commandPublisherMock.Object);

            // Act
            agent.MeldBestellingKlaarAsync(bestellingId).Wait();

            // Assert
            commandPublisherMock.Verify(e =>
                e.PublishAsync<MeldBestellingKlaarCommand>(It.IsAny<MeldBestellingKlaarCommand>()));
        }

        [DataRow(298, 21321)]
        [DataRow(1984, 124)]
        [TestMethod]
        public void PakBestelregelIn_CallsPublishAsyncOnCommandPublisher(long bestellingId, long regelId)
        {
            Mock<ICommandPublisher> commandPublisherMock = new Mock<ICommandPublisher>();
            BestellingAgent agent = new BestellingAgent(commandPublisherMock.Object);

            // Act
            agent.PakBestelregelInAsync(bestellingId, regelId).Wait();

            // Assert
            commandPublisherMock.Verify(e =>
                e.PublishAsync<PakBestelRegelInCommand>(It.IsAny<PakBestelRegelInCommand>()));
        }

        [TestMethod]
        [DataRow("10001", "20.55")]
        [DataRow("10002", "39.99")]
        public void RegistreerBetaling_CallsPublishAsyncOnCommandPublisher(string bestellingNummer, string bedragStr)
        {
            // Arrange
            var bedrag = decimal.Parse(bedragStr);
            Mock<ICommandPublisher> commandPublisherMock = new Mock<ICommandPublisher>();
            BestellingAgent agent = new BestellingAgent(commandPublisherMock.Object);

            // Act
            agent.RegistreerBetalingAsync(bestellingNummer, bedrag).Wait();

            // Assert
            commandPublisherMock.Verify(e =>
                e.PublishAsync<RegistreerBetalingCommand>(It.IsAny<RegistreerBetalingCommand>()));
        }

        [TestMethod]
        public void ControleerOfErWanbetalersZijnAsync_CallsPublishAsyncOnCommandPublisher()
        {
            // Arrange
            Mock<ICommandPublisher> commandPublisherMock = new Mock<ICommandPublisher>();
            BestellingAgent agent = new BestellingAgent(commandPublisherMock.Object);

            // Act
            agent.ControleerOfErWanbetalersZijnAsync().Wait();

            // Assert
            commandPublisherMock.Verify(e =>
                e.PublishAsync<IEnumerable<Bestelling>>(It.IsAny<ControleerOfErWanbetalingenZijnCommand>()));
        }
    }
}
