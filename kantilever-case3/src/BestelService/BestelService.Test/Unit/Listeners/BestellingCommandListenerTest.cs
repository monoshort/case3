using System.Collections.Generic;
using System.Linq;
using BestelService.Commands;
using BestelService.Core.Models;
using BestelService.Core.Repositories;
using BestelService.Listeners;
using BestelService.Services.Services.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BestelService.Test.Unit.Listeners
{
    [TestClass]
    public class BestellingCommandListenerTest
    {
        [TestMethod]
        [DataRow(3553.24)]
        [DataRow(32095.46)]
        public void HandleNieuweBestelling_CallsMaakBestellingAanOnService(double subTotaal)
        {
            // Arrange
            Mock<IBestellingService> bestellingServiceMock = new Mock<IBestellingService>();
            Mock<IKlantRepository> klantRepositoryMock = new Mock<IKlantRepository>();

            Bestelling bestelling = new Bestelling
            {
                Klant = new Klant(),
                SubtotaalInclusiefBtw = (decimal) subTotaal
            };
            MaakNieuweBestellingAanCommand command = new MaakNieuweBestellingAanCommand
            {
                Bestelling = bestelling
            };

            BestellingCommandListener bestellingCommandListener = new BestellingCommandListener(bestellingServiceMock.Object, klantRepositoryMock.Object);

            // Act
            bestellingCommandListener.HandleNieuweBestelling(command);

            // Assert
            bestellingServiceMock.Verify(e => e.MaakBestellingAan(bestelling));
        }

        [TestMethod]
        [DataRow(3553.24)]
        [DataRow(32095.46)]
        public void HandleNieuweBestelling_ReturnsCommandGivenAsParameter(double subTotaal)
        {
            // Arrange
            Mock<IBestellingService> bestellingServiceMock = new Mock<IBestellingService>();
            Mock<IKlantRepository> klantRepositoryMock = new Mock<IKlantRepository>();

            Bestelling bestelling = new Bestelling
            {
                Klant = new Klant(),
                Subtotaal = (decimal) subTotaal
            };

            MaakNieuweBestellingAanCommand command = new MaakNieuweBestellingAanCommand {Bestelling = bestelling};

            BestellingCommandListener bestellingCommandListener = new BestellingCommandListener(bestellingServiceMock.Object, klantRepositoryMock.Object);

            // Act
            MaakNieuweBestellingAanCommand result = bestellingCommandListener.HandleNieuweBestelling(command);

            // Assert
            Assert.AreSame(command, result);
            Assert.AreEqual(command.Bestelling, result.Bestelling);
            Assert.AreEqual((decimal) subTotaal, result.Bestelling.Subtotaal);
        }

        [TestMethod]
        [DataRow(321443)]
        [DataRow(698353)]
        public void HandleKeurBestellingGoed_CallsKeurBestellingGoedOnBestelServiceWithBestellingId(long bestellingId)
        {
            // Arrange
            var svcMock = new Mock<IBestellingService>();
            var klantRepoMock = new Mock<IKlantRepository>();
            var target = new BestellingCommandListener(svcMock.Object, klantRepoMock.Object);

            var command = new KeurBestellingGoedCommand
            {
                BestellingId = bestellingId
            };

            // Act
            target.HandleKeurBestellingGoed(command);

            // Arrange
            svcMock.Verify(x => x.KeurBestellingGoed(bestellingId), Times.Once);
        }

        [TestMethod]
        [DataRow(321443)]
        [DataRow(698533)]
        public void HandleKeurBestellingGoed_ReturnsCommandGivenAsParameter(long bestellingId)
        {
            // Arrange
            var svcMock = new Mock<IBestellingService>();
            var klantRepoMock = new Mock<IKlantRepository>();
            var target = new BestellingCommandListener(svcMock.Object, klantRepoMock.Object);

            var command = new KeurBestellingGoedCommand
            {
                BestellingId = bestellingId
            };

            // Act
            KeurBestellingGoedCommand result = target.HandleKeurBestellingGoed(command);

            // Arrange
            Assert.AreSame(command, result);
            Assert.AreEqual(command.BestellingId, result.BestellingId);
        }

        [TestMethod]
        [DataRow(321443)]
        [DataRow(698353)]
        public void HandleKeurBestellingAf_CallsKeurBestellingAfOnBestelServiceWithBestellingId(long bestellingId)
        {
            // Arrange
            var svcMock = new Mock<IBestellingService>();
            var klantRepoMock = new Mock<IKlantRepository>();
            var target = new BestellingCommandListener(svcMock.Object, klantRepoMock.Object);

            var command = new KeurBestellingAfCommand
            {
                BestellingId = bestellingId
            };

            // Act
            target.HandleKeurBestellingAf(command);

            // Arrange
            svcMock.Verify(x => x.KeurBestellingAf(bestellingId), Times.Once);
        }

        [TestMethod]
        [DataRow(321443)]
        [DataRow(698533)]
        public void HandleKeurBestellingAf_ReturnsCommandGivenAsParameter(long bestellingId)
        {
            // Arrange
            var svcMock = new Mock<IBestellingService>();
            var klantRepoMock = new Mock<IKlantRepository>();
            var target = new BestellingCommandListener(svcMock.Object, klantRepoMock.Object);

            var command = new KeurBestellingAfCommand
            {
                BestellingId = bestellingId
            };

            // Act
            KeurBestellingAfCommand result = target.HandleKeurBestellingAf(command);

            // Arrange
            Assert.AreSame(command, result);
            Assert.AreEqual(command.BestellingId, result.BestellingId);
        }

        [TestMethod]
        [DataRow(321443)]
        [DataRow(698353)]
        public void HandleMeldBestellingKlaar_CallsPakBestellingInOnBestelServiceWithBestellingId(long bestellingId)
        {
            // Arrange
            var svcMock = new Mock<IBestellingService>();
            var klantRepoMock = new Mock<IKlantRepository>();
            var target = new BestellingCommandListener(svcMock.Object, klantRepoMock.Object);

            var command = new MeldBestellingKlaarCommand
            {
                BestellingId = bestellingId
            };

            // Act
            target.HandleMeldBestellingKlaar(command);

            // Arrange
            svcMock.Verify(x => x.MeldBestellingKlaar(bestellingId), Times.Once);
        }

        [TestMethod]
        [DataRow(321443)]
        [DataRow(698533)]
        public void HandleMeldBestellingKlaar_ReturnsCommandGivenAsParameter(long bestellingId)
        {
            // Arrange
            var svcMock = new Mock<IBestellingService>();
            var klantRepoMock = new Mock<IKlantRepository>();
            var target = new BestellingCommandListener(svcMock.Object, klantRepoMock.Object);

            var command = new MeldBestellingKlaarCommand
            {
                BestellingId = bestellingId
            };

            // Act
            MeldBestellingKlaarCommand result = target.HandleMeldBestellingKlaar(command);

            // Arrange
            Assert.AreSame(command, result);
            Assert.AreEqual(command.BestellingId, result.BestellingId);
        }

        [TestMethod]
        [DataRow(321443, 10)]
        [DataRow(698353, 203)]
        public void HandlePakBestelRegelIn_CallsPakBestelRegelInOnBestelServiceWithBestellingId(long bestellingId, long bestelRegelId)
        {
            // Arrange
            var svcMock = new Mock<IBestellingService>();
            var klantRepoMock = new Mock<IKlantRepository>();
            var target = new BestellingCommandListener(svcMock.Object, klantRepoMock.Object);

            var command = new PakBestelRegelInCommand
            {
                BestellingId = bestellingId,
                BestelRegelId = bestelRegelId
            };

            // Act
            target.HandlePakBestelRegelIn(command);

            // Arrange
            svcMock.Verify(x => x.PakBestelRegelIn(bestellingId, bestelRegelId), Times.Once);
        }

        [TestMethod]
        [DataRow(321443, 10)]
        [DataRow(698533, 203)]
        public void HandlePakBestelRegelIn_ReturnsCommandGivenAsParameter(long bestellingId, long bestelRegelId)
        {
            // Arrange
            var svcMock = new Mock<IBestellingService>();
            var klantRepoMock = new Mock<IKlantRepository>();
            var target = new BestellingCommandListener(svcMock.Object, klantRepoMock.Object);

            var command = new PakBestelRegelInCommand
            {
                BestellingId = bestellingId,
                BestelRegelId = bestelRegelId
            };

            // Act
            PakBestelRegelInCommand result = target.HandlePakBestelRegelIn(command);

            // Arrange
            Assert.AreSame(command, result);
            Assert.AreEqual(command.BestellingId, result.BestellingId);
        }

        [TestMethod]
        [DataRow("10001", "10.10")]
        [DataRow("10002", "99.99")]
        public void HandleRegistreerBetaling_ReturnsCommandGivenAsParameter(string bestellingNummer, string betaaldBedragStr)
        {
            // Arrange
            var betaaldBedrag = decimal.Parse(betaaldBedragStr);
            var svcMock = new Mock<IBestellingService>();
            var klantRepoMock = new Mock<IKlantRepository>();
            var target = new BestellingCommandListener(svcMock.Object, klantRepoMock.Object);

            var command = new RegistreerBetalingCommand
            {
                BestellingNummer = bestellingNummer,
                BetaaldBedrag = betaaldBedrag
            };

            // Act
            RegistreerBetalingCommand result = target.HandleRegistreerBetaling(command);

            // Arrange
            Assert.AreSame(command, result);
            Assert.AreEqual(command.BetaaldBedrag, result.BetaaldBedrag);
            Assert.AreEqual(command.BestellingNummer, result.BestellingNummer);
        }

        [TestMethod]
        [DataRow("10001", "10.10")]
        [DataRow("10002", "99.99")]
        public void HandlePakBestelRegelIn_CallsPakBestelRegelInOnBestelServiceWithBestellingId(string bestellingNummer, string betaaldBedragStr)
        {
            // Arrange
            var betaaldBedrag = decimal.Parse(betaaldBedragStr);
            var svcMock = new Mock<IBestellingService>();
            var klantRepoMock = new Mock<IKlantRepository>();
            var target = new BestellingCommandListener(svcMock.Object, klantRepoMock.Object);

            var command = new RegistreerBetalingCommand
            {
                BestellingNummer = bestellingNummer,
                BetaaldBedrag = betaaldBedrag
            };

            // Act
            target.HandleRegistreerBetaling(command);

            // Arrange
            svcMock.Verify(x => x.RegistreerBetaling(bestellingNummer, betaaldBedrag), Times.Once);
        }

        [TestMethod]
        public void HandleWanbetalingenZijnCommand_CallsControleerOpWanbetalingenOnService()
        {
            // Arrange
            var svcMock = new Mock<IBestellingService>();
            var klantRepoMock = new Mock<IKlantRepository>();
            var target = new BestellingCommandListener(svcMock.Object, klantRepoMock.Object);

            var command = new ControleerOfErWanbetalingenZijnCommand();

            // Act
            target.HandleWanbetalingenZijnCommand(command);

            // Arrange
            svcMock.Verify(x => x.ControleerOpWanbetalingen(), Times.Once);
        }

        [TestMethod]
        public void HandleWanbetalingenZijnCommand_ReturnsListOfBestellingen()
        {
            // Arrange
            var svcMock = new Mock<IBestellingService>();
            var klantRepoMock = new Mock<IKlantRepository>();
            var target = new BestellingCommandListener(svcMock.Object, klantRepoMock.Object);

            IEnumerable<Bestelling> expectedData = new []
            {
                new Bestelling(),
                new Bestelling(),
                new Bestelling()
            };

            svcMock.Setup(e => e.ControleerOpWanbetalingen())
                .Returns(expectedData);

            var command = new ControleerOfErWanbetalingenZijnCommand();

            // Act
            IEnumerable<Bestelling> result = target.HandleWanbetalingenZijnCommand(command);

            // Arrange
            Assert.AreEqual(3, result.Count());
        }

        [TestMethod]
        [DataRow(10)]
        [DataRow(5920)]
        public void HandlePrintAdresLabel_CallsPrintAdresLablelOnService(int bestelId)
        {
            // Arrange
            var svcMock = new Mock<IBestellingService>();
            var klantRepoMock = new Mock<IKlantRepository>();
            var target = new BestellingCommandListener(svcMock.Object, klantRepoMock.Object);

            var command = new PrintAdresLabelCommand { BestellingId = bestelId };

            // Act
            target.HandlePrintAdresLabel(command);

            // Arrange
            svcMock.Verify(x => x.PrintAdresLabel(bestelId), Times.Once);
        }

        [TestMethod]
        [DataRow(10)]
        [DataRow(5920)]
        public void HandlePrintAdresLabel_ReturnsCommand(int bestelId)
        {
            // Arrange
            var svcMock = new Mock<IBestellingService>();
            var klantRepoMock = new Mock<IKlantRepository>();
            var target = new BestellingCommandListener(svcMock.Object, klantRepoMock.Object);

            var command = new PrintAdresLabelCommand { BestellingId = bestelId };

            // Act
            PrintAdresLabelCommand result = target.HandlePrintAdresLabel(command);

            // Arrange
            Assert.AreEqual(command, result);
        }

        [TestMethod]
        [DataRow(10)]
        [DataRow(5920)]
        public void HandlePrintFactuur_CallsPrintFactuurlOnService(int bestelId)
        {
            // Arrange
            var svcMock = new Mock<IBestellingService>();
            var klantRepoMock = new Mock<IKlantRepository>();
            var target = new BestellingCommandListener(svcMock.Object, klantRepoMock.Object);

            var command = new PrintFactuurCommand { BestellingId = bestelId };

            // Act
            target.HandlePrintFactuur(command);

            // Arrange
            svcMock.Verify(x => x.PrintFactuur(bestelId), Times.Once);
        }

        [TestMethod]
        [DataRow(10)]
        [DataRow(5920)]
        public void HandlePrintFactuur_ReturnsCommand(int bestelId)
        {
            // Arrange
            var svcMock = new Mock<IBestellingService>();
            var klantRepoMock = new Mock<IKlantRepository>();
            var target = new BestellingCommandListener(svcMock.Object, klantRepoMock.Object);

            var command = new PrintFactuurCommand { BestellingId = bestelId };

            // Act
            PrintFactuurCommand result = target.HandlePrintFactuur(command);

            // Arrange
            Assert.AreEqual(command, result);
        }
    }
}
