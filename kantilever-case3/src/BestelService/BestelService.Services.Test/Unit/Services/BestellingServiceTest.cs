using System;
using System.Collections.Generic;
using System.Linq;
using BestelService.Core.Models;
using BestelService.Core.Repositories;
using BestelService.Services.Events;
using BestelService.Services.Services;
using BestelService.Services.Services.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minor.Miffy.MicroServices.Events;
using Moq;

namespace BestelService.Services.Test.Unit.Services
{
    [TestClass]
    public class BestellingServiceTest
    {
        [TestMethod]
        [DataRow(24.64)]
        [DataRow(2423.64)]
        public void MaakBestellingAan_CallsAddOnRepositoryWithBestelling(double subTotaal)
        {
            // Arrange
            Mock<IBestelRepository> bestelRepositoryMock = new Mock<IBestelRepository>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();
            IBestellingService bestellingService = new BestellingService(bestelRepositoryMock.Object, eventPublisherMock.Object);

            Bestelling bestelling = new Bestelling { Subtotaal = (decimal)subTotaal, Klant = new Klant() };

            // Act
            bestellingService.MaakBestellingAan(bestelling);

            // Assert
            bestelRepositoryMock.Verify(e => e.Add(bestelling));
        }

        [TestMethod]
        [DataRow(23)]
        [DataRow(254280)]
        public void MaakBestellingAan_AddsBestelNummer(int id)
        {
            // Arrange
            Mock<IBestelRepository> bestelRepositoryMock = new Mock<IBestelRepository>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();
            IBestellingService bestellingService = new BestellingService(bestelRepositoryMock.Object, eventPublisherMock.Object);

            Bestelling bestelling = new Bestelling { Id = id, Klant = new Klant() };

            // Act
            bestellingService.MaakBestellingAan(bestelling);

            // Assert
            string expectedBestelNummer = (id + Bestelling.BestelNummerStartNumber).ToString();
            Assert.AreEqual(expectedBestelNummer, bestelling.BestellingNummer);
        }

        [TestMethod]
        [DataRow(300.00, true)]
        [DataRow(600.00, false)]
        public void MaakBestellingAan_CallsControleerOfBestellingAutomatischGoedgekeurdIs(double subTotaalInclusiefBtw, bool expectGoedgekeurd)
        {
            // Arrange
            Mock<IBestelRepository> bestelRepositoryMock = new Mock<IBestelRepository>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();
            IBestellingService bestellingService = new BestellingService(bestelRepositoryMock.Object, eventPublisherMock.Object);

            Bestelling bestelling = new Bestelling { OpenstaandBedrag = (decimal)subTotaalInclusiefBtw, Klant = new Klant() };

            // Act
            bestellingService.MaakBestellingAan(bestelling);

            // Assert
            Assert.AreEqual(expectGoedgekeurd, bestelling.Goedgekeurd);
        }

        [TestMethod]
        [DataRow(24.64)]
        [DataRow(2423.64)]
        public void MaakBestellingAan_CallsUpdateOnRepositoryWithBestelling(double subTotaal)
        {
            // Arrange
            Mock<IBestelRepository> bestelRepositoryMock = new Mock<IBestelRepository>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();
            IBestellingService bestellingService = new BestellingService(bestelRepositoryMock.Object, eventPublisherMock.Object);

            Bestelling bestelling = new Bestelling { Subtotaal = (decimal)subTotaal, Klant = new Klant() };

            // Act
            bestellingService.MaakBestellingAan(bestelling);

            // Assert
            bestelRepositoryMock.Verify(e => e.Update(bestelling));
        }

        [TestMethod]
        [DataRow(24.64)]
        [DataRow(2423.64)]
        public void MaakBestellingAan_CallsPublishAsyncOnEventPublisherWithEvent(double subTotaal)
        {
            // Arrange
            Mock<IBestelRepository> bestelRepositoryMock = new Mock<IBestelRepository>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();
            IBestellingService bestellingService = new BestellingService(bestelRepositoryMock.Object, eventPublisherMock.Object);

            Bestelling bestelling = new Bestelling { Subtotaal = (decimal)subTotaal, Klant = new Klant() };

            // Act
            bestellingService.MaakBestellingAan(bestelling);

            // Assert
            eventPublisherMock.Verify(e =>
                e.PublishAsync(It.Is<NieuweBestellingAangemaaktEvent>(b =>
                    b.Bestelling.Subtotaal == (decimal)subTotaal)));
        }

        [TestMethod]
        [DataRow(20)]
        [DataRow(5029)]
        public void KeurBestellingGoed_CallsUpdateOnRepositoryWithBestelling(long id)
        {
            // Arrange
            Mock<IBestelRepository> bestelRepositoryMock = new Mock<IBestelRepository>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();
            IBestellingService bestellingService = new BestellingService(bestelRepositoryMock.Object, eventPublisherMock.Object);

            Bestelling bestelling = new Bestelling { Id = id };

            bestelRepositoryMock.Setup(e => e.GetById(id))
                .Returns(bestelling);

            // Act
            bestellingService.KeurBestellingGoed(id);

            // Assert
            bestelRepositoryMock.Verify(e => e.Update(bestelling));
        }

        [TestMethod]
        [DataRow(2)]
        [DataRow(229)]
        public void KeurBestellingGoed_CallsPublishAsyncOnEventPublisherWithEvent(long id)
        {
            // Arrange
            Mock<IBestelRepository> bestelRepositoryMock = new Mock<IBestelRepository>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();
            IBestellingService bestellingService = new BestellingService(bestelRepositoryMock.Object, eventPublisherMock.Object);

            Bestelling bestelling = new Bestelling { Id = id };

            bestelRepositoryMock.Setup(e => e.GetById(id))
                .Returns(bestelling);

            // Act
            bestellingService.KeurBestellingGoed(id);

            // Assert
            eventPublisherMock.Verify(e =>
                e.PublishAsync(It.Is<BestellingGoedgekeurdEvent>(b => b.BestellingId == id)));
        }

        [TestMethod]
        [DataRow(2329)]
        [DataRow(20404)]
        public void KeurBestellingGoed_CallsGetByIdOnRepository(long id)
        {
            // Arrange
            Mock<IBestelRepository> bestelRepositoryMock = new Mock<IBestelRepository>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();
            IBestellingService bestellingService = new BestellingService(bestelRepositoryMock.Object, eventPublisherMock.Object);

            Bestelling bestelling = new Bestelling { Id = id };

            bestelRepositoryMock.Setup(e => e.GetById(id))
                .Returns(bestelling)
                .Verifiable();

            // Act
            bestellingService.KeurBestellingGoed(id);

            // Assert
            bestelRepositoryMock.Verify();
        }

        [TestMethod]
        [DataRow(23)]
        [DataRow(254280)]
        public void KeurBestellingGoed_KeursBestellingGoed(int id)
        {
            // Arrange
            Mock<IBestelRepository> bestelRepositoryMock = new Mock<IBestelRepository>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();
            IBestellingService bestellingService = new BestellingService(bestelRepositoryMock.Object, eventPublisherMock.Object);

            Bestelling bestelling = new Bestelling { Id = id };
            bestelRepositoryMock.Setup(e => e.GetById(id))
                .Returns(bestelling);

            // Act
            bestellingService.KeurBestellingGoed(id);

            // Assert
            Assert.AreEqual(true, bestelling.Goedgekeurd);
        }

        [TestMethod]
        [DataRow(2329)]
        [DataRow(20404)]
        public void KeurBestellingAf_CallsGetByIdOnRepository(long id)
        {
            // Arrange
            Mock<IBestelRepository> bestelRepositoryMock = new Mock<IBestelRepository>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();
            IBestellingService bestellingService = new BestellingService(bestelRepositoryMock.Object, eventPublisherMock.Object);

            Bestelling bestelling = new Bestelling { Id = id };

            bestelRepositoryMock.Setup(e => e.GetById(id))
                .Returns(bestelling)
                .Verifiable();

            // Act
            bestellingService.KeurBestellingAf(id);

            // Assert
            bestelRepositoryMock.Verify();
        }

        [TestMethod]
        [DataRow(2)]
        [DataRow(229)]
        public void KeurBestellingAf_CallsPublishAsyncOnEventPublisherWithEvent(long id)
        {
            // Arrange
            Mock<IBestelRepository> bestelRepositoryMock = new Mock<IBestelRepository>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();
            IBestellingService bestellingService = new BestellingService(bestelRepositoryMock.Object, eventPublisherMock.Object);

            Bestelling bestelling = new Bestelling { Id = id };

            bestelRepositoryMock.Setup(e => e.GetById(id))
                .Returns(bestelling);

            // Act
            bestellingService.KeurBestellingAf(id);

            // Assert
            eventPublisherMock.Verify(e =>
                e.PublishAsync(It.Is<BestellingAfgekeurdEvent>(b => b.BestellingId == id)));
        }

        [TestMethod]
        [DataRow(20)]
        [DataRow(5029)]
        public void KeurBestellingAf_CallsUpdateOnRepositoryWithBestelling(long id)
        {
            // Arrange
            Mock<IBestelRepository> bestelRepositoryMock = new Mock<IBestelRepository>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();
            IBestellingService bestellingService = new BestellingService(bestelRepositoryMock.Object, eventPublisherMock.Object);

            Bestelling bestelling = new Bestelling { Id = id };

            bestelRepositoryMock.Setup(e => e.GetById(id))
                .Returns(bestelling);

            // Act
            bestellingService.KeurBestellingAf(id);

            // Assert
            bestelRepositoryMock.Verify(e => e.Update(bestelling));
        }

        [TestMethod]
        [DataRow(23)]
        [DataRow(254280)]
        public void KeurBestellingAf_KeursBestellingGoed(int id)
        {
            // Arrange
            Mock<IBestelRepository> bestelRepositoryMock = new Mock<IBestelRepository>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();
            IBestellingService bestellingService = new BestellingService(bestelRepositoryMock.Object, eventPublisherMock.Object);

            Bestelling bestelling = new Bestelling { Id = id };
            bestelRepositoryMock.Setup(e => e.GetById(id))
                .Returns(bestelling);

            // Act
            bestellingService.KeurBestellingAf(id);

            // Assert
            Assert.AreEqual(true, bestelling.Afgekeurd);
        }

        [TestMethod]
        [DataRow(2329)]
        [DataRow(20404)]
        public void MeldBestellingKlaar_CallsGetByIdOnRepository(long id)
        {
            // Arrange
            Mock<IBestelRepository> bestelRepositoryMock = new Mock<IBestelRepository>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();
            IBestellingService bestellingService = new BestellingService(bestelRepositoryMock.Object, eventPublisherMock.Object);

            Bestelling bestelling = new Bestelling { FactuurGeprint = true, AdresLabelGeprint = true, Id = id };

            bestelRepositoryMock.Setup(e => e.GetById(id))
                .Returns(bestelling)
                .Verifiable();

            // Act
            bestellingService.MeldBestellingKlaar(id);

            // Assert
            bestelRepositoryMock.Verify();
        }

        [TestMethod]
        [DataRow(20)]
        [DataRow(5029)]
        public void MeldBestellingKlaar_CallsUpdateOnRepositoryWithBestelling(long id)
        {
            // Arrange
            Mock<IBestelRepository> bestelRepositoryMock = new Mock<IBestelRepository>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();
            IBestellingService bestellingService = new BestellingService(bestelRepositoryMock.Object, eventPublisherMock.Object);

            Bestelling bestelling = new Bestelling { FactuurGeprint = true, AdresLabelGeprint = true, Id = id };

            bestelRepositoryMock.Setup(e => e.GetById(id))
                .Returns(bestelling);

            // Act
            bestellingService.MeldBestellingKlaar(id);

            // Assert
            bestelRepositoryMock.Verify(e => e.Update(bestelling));
        }

        [TestMethod]
        [DataRow(2)]
        [DataRow(229)]
        public void MeldBestellingKlaar_CallsPublishAsyncOnEventPublisherWithEvent(long id)
        {
            // Arrange
            Mock<IBestelRepository> bestelRepositoryMock = new Mock<IBestelRepository>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();
            IBestellingService bestellingService = new BestellingService(bestelRepositoryMock.Object, eventPublisherMock.Object);

            Bestelling bestelling = new Bestelling { FactuurGeprint = true, AdresLabelGeprint = true, Id = id };

            bestelRepositoryMock.Setup(e => e.GetById(id))
                .Returns(bestelling);

            // Act
            bestellingService.MeldBestellingKlaar(id);

            // Assert
            eventPublisherMock.Verify(e =>
                e.PublishAsync(It.Is<BestellingKlaarGemeldEvent>(b => b.BestellingId == id)));
        }

        [TestMethod]
        [DataRow(23)]
        [DataRow(254280)]
        public void MeldBestellingKlaar_PaktBestellingIn(int id)
        {
            // Arrange
            Mock<IBestelRepository> bestelRepositoryMock = new Mock<IBestelRepository>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();
            IBestellingService bestellingService = new BestellingService(bestelRepositoryMock.Object, eventPublisherMock.Object);

            Bestelling bestelling = new Bestelling { FactuurGeprint = true, AdresLabelGeprint = true, Id = id };
            bestelRepositoryMock.Setup(e => e.GetById(id))
                .Returns(bestelling);

            // Act
            bestellingService.MeldBestellingKlaar(id);

            // Assert
            Assert.AreEqual(true, bestelling.KlaarGemeld);
        }

        [TestMethod]
        [DataRow(2329, 20)]
        [DataRow(20404, 50)]
        public void PakBestelRegelIn_CallsGetByIdOnRepository(long id, long bestelRegelId)
        {
            // Arrange
            Mock<IBestelRepository> bestelRepositoryMock = new Mock<IBestelRepository>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();
            IBestellingService bestellingService = new BestellingService(bestelRepositoryMock.Object, eventPublisherMock.Object);

            Bestelling bestelling = new Bestelling
            {
                Id = id,
                BestelRegels = new List<BestelRegel> { new BestelRegel { Id = bestelRegelId } }
            };

            bestelRepositoryMock.Setup(e => e.GetById(id))
                .Returns(bestelling)
                .Verifiable();

            // Act
            bestellingService.PakBestelRegelIn(id, bestelRegelId);

            // Assert
            bestelRepositoryMock.Verify();
        }

        [TestMethod]
        [DataRow(2, 1)]
        [DataRow(229, 230)]
        public void PakBestelRegelIn_CallsPublishAsyncOnEventPublisherWithEvent(long id, long bestelRegelId)
        {
            // Arrange
            Mock<IBestelRepository> bestelRepositoryMock = new Mock<IBestelRepository>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();
            IBestellingService bestellingService = new BestellingService(bestelRepositoryMock.Object, eventPublisherMock.Object);

            Bestelling bestelling = new Bestelling
            {
                Id = id,
                BestelRegels = new List<BestelRegel> { new BestelRegel { Id = bestelRegelId } }
            };

            bestelRepositoryMock.Setup(e => e.GetById(id))
                .Returns(bestelling);

            // Act
            bestellingService.PakBestelRegelIn(id, bestelRegelId);

            // Assert
            eventPublisherMock.Verify(e => e.PublishAsync(It.Is<BestelRegelIngepaktEvent>(
                b => b.BestellingId == id && b.BestelRegelId == bestelRegelId)));
        }

        [TestMethod]
        [DataRow(2, 1)]
        [DataRow(229, 230)]
        public void PakBestelRegelIn_PublishesKanKlaarGemeldWordenEventIfReady(long id, long bestelRegelId)
        {
            // Arrange
            Mock<IBestelRepository> bestelRepositoryMock = new Mock<IBestelRepository>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();
            IBestellingService bestellingService = new BestellingService(bestelRepositoryMock.Object, eventPublisherMock.Object);

            Bestelling bestelling = new Bestelling
            {
                Id = id,
                FactuurGeprint = true,
                AdresLabelGeprint = true,
                BestelRegels = new List<BestelRegel> { new BestelRegel { Id = bestelRegelId } }
            };

            bestelRepositoryMock.Setup(e => e.GetById(id))
                .Returns(bestelling);

            // Act
            bestellingService.PakBestelRegelIn(id, bestelRegelId);

            // Assert
            eventPublisherMock.Verify(e => e.PublishAsync(It.Is<BestellingKanKlaarGemeldWordenEvent>(
                b => b.BestellingId == id)));
        }

        [TestMethod]
        [DataRow(23, 10)]
        [DataRow(254280, 2049)]
        public void PakBestelRegelIn_PaktBestelRegelIn(int id, int bestelRegelId)
        {
            // Arrange
            Mock<IBestelRepository> bestelRepositoryMock = new Mock<IBestelRepository>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();
            IBestellingService bestellingService = new BestellingService(bestelRepositoryMock.Object, eventPublisherMock.Object);

            Bestelling bestelling = new Bestelling
            {
                Id = id,
                BestelRegels = new List<BestelRegel> { new BestelRegel { Id = bestelRegelId } }
            };
            bestelRepositoryMock.Setup(e => e.GetById(id))
                .Returns(bestelling);

            // Act
            bestellingService.PakBestelRegelIn(id, bestelRegelId);

            // Assert
            Assert.AreEqual(true, bestelling.BestelRegels.First().Ingepakt);
        }

        [TestMethod]
        [DataRow(2329)]
        [DataRow(20404)]
        public void PrintFactuur_CallsGetByIdOnRepository(long id)
        {
            // Arrange
            Mock<IBestelRepository> bestelRepositoryMock = new Mock<IBestelRepository>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();
            IBestellingService bestellingService = new BestellingService(bestelRepositoryMock.Object, eventPublisherMock.Object);

            Bestelling bestelling = new Bestelling { Id = id };

            bestelRepositoryMock.Setup(e => e.GetById(id))
                .Returns(bestelling)
                .Verifiable();

            // Act
            bestellingService.PrintFactuur(id);

            // Assert
            bestelRepositoryMock.Verify();
        }

        [TestMethod]
        [DataRow(2)]
        [DataRow(229)]
        public void PrintFactuur_CallsPublishAsyncOnEventPublisherWithEvent(long id)
        {
            // Arrange
            Mock<IBestelRepository> bestelRepositoryMock = new Mock<IBestelRepository>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();
            IBestellingService bestellingService = new BestellingService(bestelRepositoryMock.Object, eventPublisherMock.Object);

            Bestelling bestelling = new Bestelling { Id = id };

            bestelRepositoryMock.Setup(e => e.GetById(id))
                .Returns(bestelling);

            // Act
            bestellingService.PrintFactuur(id);

            // Assert
            eventPublisherMock.Verify(e => e.PublishAsync(It.Is<BestellingFactuurGeprintEvent>(
                b => b.BestellingId == id)));
        }

        [TestMethod]
        [DataRow(20)]
        [DataRow(5029)]
        public void PrintFactuur_CallsUpdateOnRepositoryWithBestelling(long id)
        {
            // Arrange
            Mock<IBestelRepository> bestelRepositoryMock = new Mock<IBestelRepository>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();
            IBestellingService bestellingService = new BestellingService(bestelRepositoryMock.Object, eventPublisherMock.Object);

            Bestelling bestelling = new Bestelling { Id = id };

            bestelRepositoryMock.Setup(e => e.GetById(id))
                .Returns(bestelling);

            // Act
            bestellingService.PrintFactuur(id);

            // Assert
            bestelRepositoryMock.Verify(e => e.Update(bestelling));
        }

        [TestMethod]
        [DataRow(2)]
        [DataRow(229)]
        public void PrintFactuur_PublishesKanKlaarGemeldWordenEventIfReady(long id)
        {
            // Arrange
            Mock<IBestelRepository> bestelRepositoryMock = new Mock<IBestelRepository>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();
            IBestellingService bestellingService = new BestellingService(bestelRepositoryMock.Object, eventPublisherMock.Object);

            Bestelling bestelling = new Bestelling
            {
                Id = id,
                AdresLabelGeprint = true
            };

            bestelRepositoryMock.Setup(e => e.GetById(id))
                .Returns(bestelling);

            // Act
            bestellingService.PrintFactuur(id);

            // Assert
            eventPublisherMock.Verify(e => e.PublishAsync(It.Is<BestellingKanKlaarGemeldWordenEvent>(
                b => b.BestellingId == id)));
        }

        [TestMethod]
        [DataRow(23)]
        [DataRow(254280)]
        public void PrintFactuur_SetsFactuurGeprintToTrue(int id)
        {
            // Arrange
            Mock<IBestelRepository> bestelRepositoryMock = new Mock<IBestelRepository>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();
            IBestellingService bestellingService = new BestellingService(bestelRepositoryMock.Object, eventPublisherMock.Object);

            Bestelling bestelling = new Bestelling { Id = id };
            bestelRepositoryMock.Setup(e => e.GetById(id))
                .Returns(bestelling);

            // Act
            bestellingService.PrintFactuur(id);

            // Assert
            Assert.AreEqual(true, bestelling.FactuurGeprint);
        }

        [TestMethod]
        [DataRow(2329)]
        [DataRow(20404)]
        public void PrintAdresLabel_CallsGetByIdOnRepository(long id)
        {
            // Arrange
            Mock<IBestelRepository> bestelRepositoryMock = new Mock<IBestelRepository>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();
            IBestellingService bestellingService = new BestellingService(bestelRepositoryMock.Object, eventPublisherMock.Object);

            Bestelling bestelling = new Bestelling { Id = id };

            bestelRepositoryMock.Setup(e => e.GetById(id))
                .Returns(bestelling)
                .Verifiable();

            // Act
            bestellingService.PrintAdresLabel(id);

            // Assert
            bestelRepositoryMock.Verify();
        }

        [TestMethod]
        [DataRow(2)]
        [DataRow(229)]
        public void PrintAdresLabel_CallsPublishAsyncOnEventPublisherWithEvent(long id)
        {
            // Arrange
            Mock<IBestelRepository> bestelRepositoryMock = new Mock<IBestelRepository>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();
            IBestellingService bestellingService = new BestellingService(bestelRepositoryMock.Object, eventPublisherMock.Object);

            Bestelling bestelling = new Bestelling { Id = id };

            bestelRepositoryMock.Setup(e => e.GetById(id))
                .Returns(bestelling);

            // Act
            bestellingService.PrintAdresLabel(id);

            // Assert
            eventPublisherMock.Verify(e => e.PublishAsync(It.Is<BestellingAdresLabelGeprintEvent>(
                b => b.BestellingId == id)));
        }

        [TestMethod]
        [DataRow(20)]
        [DataRow(5029)]
        public void PrintAdresLabel_CallsUpdateOnRepositoryWithBestelling(long id)
        {
            // Arrange
            Mock<IBestelRepository> bestelRepositoryMock = new Mock<IBestelRepository>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();
            IBestellingService bestellingService = new BestellingService(bestelRepositoryMock.Object, eventPublisherMock.Object);

            Bestelling bestelling = new Bestelling { Id = id };

            bestelRepositoryMock.Setup(e => e.GetById(id))
                .Returns(bestelling);

            // Act
            bestellingService.PrintAdresLabel(id);

            // Assert
            bestelRepositoryMock.Verify(e => e.Update(bestelling));
        }

        [TestMethod]
        [DataRow(23)]
        [DataRow(254280)]
        public void PrintAdresLabel_SetsAdresLabelGeprintToTrue(int id)
        {
            // Arrange
            Mock<IBestelRepository> bestelRepositoryMock = new Mock<IBestelRepository>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();
            IBestellingService bestellingService = new BestellingService(bestelRepositoryMock.Object, eventPublisherMock.Object);

            Bestelling bestelling = new Bestelling { Id = id };
            bestelRepositoryMock.Setup(e => e.GetById(id))
                .Returns(bestelling);

            // Act
            bestellingService.PrintAdresLabel(id);

            // Assert
            Assert.AreEqual(true, bestelling.AdresLabelGeprint);
        }

        [TestMethod]
        [DataRow(2)]
        [DataRow(229)]
        public void PrintAdresLabel_PublishesKanKlaarGemeldWordenEventIfReady(long id)
        {
            // Arrange
            Mock<IBestelRepository> bestelRepositoryMock = new Mock<IBestelRepository>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();
            IBestellingService bestellingService = new BestellingService(bestelRepositoryMock.Object, eventPublisherMock.Object);

            Bestelling bestelling = new Bestelling
            {
                Id = id,
                FactuurGeprint = true
            };

            bestelRepositoryMock.Setup(e => e.GetById(id))
                .Returns(bestelling);

            // Act
            bestellingService.PrintAdresLabel(id);

            // Assert
            eventPublisherMock.Verify(e => e.PublishAsync(It.Is<BestellingKanKlaarGemeldWordenEvent>(
                b => b.BestellingId == id)));
        }

        [TestMethod]
        [DataRow(1, "10001", "100.10", "50.99")]
        [DataRow(2, "10002", "199.99", "199.98")]
        public void RegistreerBetaling_CalculatesTheNewOpenstaandeBedrag(int id, string bestellingNummer, string openstaandBedragStr, string betaaldBedragStr)
        {
            // Arrange
            decimal openstaandBedrag = decimal.Parse(openstaandBedragStr);
            decimal betaaldBedrag = decimal.Parse(betaaldBedragStr);
            Mock<IBestelRepository> bestelRepositoryMock = new Mock<IBestelRepository>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();
            IBestellingService bestellingService = new BestellingService(bestelRepositoryMock.Object, eventPublisherMock.Object);

            Bestelling bestelling = new Bestelling
            {
                Id = id,
                OpenstaandBedrag = openstaandBedrag,
                BestellingNummer = bestellingNummer
            };
            bestelRepositoryMock.Setup(e => e.GetByBestellingNummer(bestellingNummer))
                .Returns(bestelling);

            // Act
            bestellingService.RegistreerBetaling(bestellingNummer, betaaldBedrag);

            // Assert
            var expectedBedrag = openstaandBedrag - betaaldBedrag;
            Assert.AreEqual(expectedBedrag, bestelling.OpenstaandBedrag);
        }

        [TestMethod]
        [DataRow(1, "10001", "100.10", "50.99")]
        [DataRow(2, "10002", "199.99", "199.98")]
        public void RegistreerBetaling_UpdatesBestelling(int id, string bestellingNummer, string openstaandBedragStr, string betaaldBedragStr)
        {
            // Arrange
            decimal openstaandBedrag = decimal.Parse(openstaandBedragStr);
            decimal betaaldBedrag = decimal.Parse(betaaldBedragStr);
            Mock<IBestelRepository> bestelRepositoryMock = new Mock<IBestelRepository>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();
            IBestellingService bestellingService = new BestellingService(bestelRepositoryMock.Object, eventPublisherMock.Object);

            Bestelling bestelling = new Bestelling
            {
                Id = id,
                OpenstaandBedrag = openstaandBedrag,
                BestellingNummer = bestellingNummer
            };
            bestelRepositoryMock.Setup(e => e.GetByBestellingNummer(bestellingNummer))
                .Returns(bestelling);

            // Act
            bestellingService.RegistreerBetaling(bestellingNummer, betaaldBedrag);

            // Assert
            var expectedBedrag = openstaandBedrag - betaaldBedrag;
            bestelRepositoryMock.Verify(e => e.Update(It.Is<Bestelling>(
                b => b.Id == id && b.OpenstaandBedrag == expectedBedrag)));
        }

        [TestMethod]
        [DataRow(1, "10001", "100.10", "50.99")]
        [DataRow(2, "10002", "199.99", "199.98")]
        public void RegistreerBetaling_PublishesAsyncEvent(int id, string bestellingNummer, string openstaandBedragStr, string betaaldBedragStr)
        {
            // Arrange
            decimal openstaandBedrag = decimal.Parse(openstaandBedragStr);
            decimal betaaldBedrag = decimal.Parse(betaaldBedragStr);
            Mock<IBestelRepository> bestelRepositoryMock = new Mock<IBestelRepository>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();
            IBestellingService bestellingService = new BestellingService(bestelRepositoryMock.Object, eventPublisherMock.Object);

            Bestelling bestelling = new Bestelling
            {
                Id = id,
                OpenstaandBedrag = openstaandBedrag,
                BestellingNummer = bestellingNummer
            };
            bestelRepositoryMock.Setup(e => e.GetByBestellingNummer(bestellingNummer))
                .Returns(bestelling);

            // Act
            bestellingService.RegistreerBetaling(bestellingNummer, betaaldBedrag);

            // Assert
            var expectedBedrag = openstaandBedrag - betaaldBedrag;
            eventPublisherMock.Verify(e => e.PublishAsync(It.Is<BetalingGeregistreerdEvent>(
                b => b.BestellingId == id && b.OpenstaandBedrag == expectedBedrag)));
        }

        [TestMethod]
        public void ControleerOpWanbetalingen_CallsGetAllOnRepository()
        {
            // Arrange
            Mock<IBestelRepository> bestelRepositoryMock = new Mock<IBestelRepository>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();
            IBestellingService bestellingService = new BestellingService(bestelRepositoryMock.Object, eventPublisherMock.Object);

            // Act
            bestellingService.ControleerOpWanbetalingen();

            // Assert
            bestelRepositoryMock.Verify(e => e.GetAll());
        }

        [TestMethod]
        public void ControleerOpWanbetalingen_CallsControleerOfKlantWanbetalerIsOnBestelling()
        {
            // Arrange
            Mock<IBestelRepository> bestelRepositoryMock = new Mock<IBestelRepository>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();
            Mock<Bestelling> bestellingMock = new Mock<Bestelling>();
            IBestellingService bestellingService = new BestellingService(bestelRepositoryMock.Object, eventPublisherMock.Object);

            bestelRepositoryMock.Setup(e => e.GetAll())
                .Returns(new List<Bestelling> { bestellingMock.Object });

            // Act
            bestellingService.ControleerOpWanbetalingen();

            // Assert
            bestellingMock.Verify(e => e.ControleerOfKlantWanbetalerIs());
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(20)]
        public void ControleerOpWanbetalingen_CallsControleerOfKlantWanbetalerIsOnAllBestellingen(int amount)
        {
            // Arrange
            Mock<IBestelRepository> bestelRepositoryMock = new Mock<IBestelRepository>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();
            var bestellingMocks = Enumerable.Repeat(new Mock<Bestelling>(), amount).ToList();
            IBestellingService bestellingService = new BestellingService(bestelRepositoryMock.Object, eventPublisherMock.Object);

            bestelRepositoryMock.Setup(e => e.GetAll())
                .Returns(bestellingMocks.Select(e => e.Object ));

            // Act
            bestellingService.ControleerOpWanbetalingen();

            // Assert
            foreach (var bestellingMock in bestellingMocks)
            {
                bestellingMock.Verify(e => e.ControleerOfKlantWanbetalerIs());
            }
        }

        [TestMethod]
        [DataRow(5)]
        [DataRow(2342)]
        public void ControleerOpWanbetalingen_CallsPublishAsyncOnEventPublisher(long id)
        {
            // Arrange
            Mock<IBestelRepository> bestelRepositoryMock = new Mock<IBestelRepository>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();
            IBestellingService bestellingService = new BestellingService(bestelRepositoryMock.Object, eventPublisherMock.Object);

            Bestelling bestelling = new Bestelling
            {
                Goedgekeurd = false,
                Id = id,
                BestelDatum = DateTime.Now.AddDays(-Bestelling.DagenOmTeBetalen - 1)
            };

            bestelRepositoryMock.Setup(e => e.GetAll())
                .Returns(new List<Bestelling> { bestelling });

            // Act
            bestellingService.ControleerOpWanbetalingen();

            // Assert
            eventPublisherMock.Verify(e =>
                e.PublishAsync(It.Is<KlantIsWanbetalerGewordenEvent>(e => e.BestellingId == id)));
        }

        [TestMethod]
        public void ControleerOpWanbetalingen_CallsPublishAsyncOnEventPublisherForEveryBestelling()
        {
            // Arrange
            Mock<IBestelRepository> bestelRepositoryMock = new Mock<IBestelRepository>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();
            IBestellingService bestellingService = new BestellingService(bestelRepositoryMock.Object, eventPublisherMock.Object);

            Bestelling bestelling1 = new Bestelling
            {
                Id = 1,
                BestelDatum = DateTime.Now.AddDays(-Bestelling.DagenOmTeBetalen - 1)
            };

            Bestelling bestelling2 = new Bestelling
            {
                Id = 2,
                BestelDatum = DateTime.Now.AddDays(-Bestelling.DagenOmTeBetalen - 1)
            };

            Bestelling bestelling3 = new Bestelling
            {
                Id = 3,
                BestelDatum = DateTime.Now.AddDays(-Bestelling.DagenOmTeBetalen - 1)
            };

            bestelRepositoryMock.Setup(e => e.GetAll())
                .Returns(new List<Bestelling> { bestelling1, bestelling2, bestelling3 });

            // Act
            bestellingService.ControleerOpWanbetalingen();

            // Assert
            eventPublisherMock.Verify(e =>
                e.PublishAsync(It.Is<KlantIsWanbetalerGewordenEvent>(e => e.BestellingId == 1)));
            eventPublisherMock.Verify(e =>
                e.PublishAsync(It.Is<KlantIsWanbetalerGewordenEvent>(e => e.BestellingId == 2)));
            eventPublisherMock.Verify(e =>
                e.PublishAsync(It.Is<KlantIsWanbetalerGewordenEvent>(e => e.BestellingId == 3)));
        }

        [TestMethod]
        [DataRow("5")]
        [DataRow("2342")]
        public void ControleerOpWanbetalingen_DoesNotCallPublishAsyncOnEventPublisherIfBestellingIsAlreadyWanbetaler(string bestellingNummer)
        {
            // Arrange
            Mock<IBestelRepository> bestelRepositoryMock = new Mock<IBestelRepository>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();
            IBestellingService bestellingService = new BestellingService(bestelRepositoryMock.Object, eventPublisherMock.Object);

            Bestelling bestelling = new Bestelling
            {
                Goedgekeurd = false,
                BestellingNummer = bestellingNummer,
                BestelDatum = DateTime.Now.AddDays(-Bestelling.DagenOmTeBetalen - 1),
                IsKlantWanbetaler = true
            };

            bestelRepositoryMock.Setup(e => e.GetAll())
                .Returns(new List<Bestelling> { bestelling });

            // Act
            bestellingService.ControleerOpWanbetalingen();

            // Assert
            eventPublisherMock.Verify(e => e.PublishAsync(It.IsAny<KlantIsWanbetalerGewordenEvent>()), Times.Never);
        }

        [TestMethod]
        [DataRow("5")]
        [DataRow("2342")]
        public void ControleerOpWanbetalingen_CallsUpdateOnRepositoryWithBestelling(string bestellingNummer)
        {
            // Arrange
            Mock<IBestelRepository> bestelRepositoryMock = new Mock<IBestelRepository>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();
            IBestellingService bestellingService = new BestellingService(bestelRepositoryMock.Object, eventPublisherMock.Object);

            Bestelling bestelling = new Bestelling
            {
                Goedgekeurd = false,
                BestellingNummer = bestellingNummer,
                IsKlantWanbetaler = false,
                BestelDatum = DateTime.Now.AddDays(-Bestelling.DagenOmTeBetalen - 1)
            };

            bestelRepositoryMock.Setup(e => e.GetAll())
                .Returns(new List<Bestelling> { bestelling });

            // Act
            bestellingService.ControleerOpWanbetalingen();

            // Assert
            bestelRepositoryMock.Verify(e => e.Update(bestelling));
        }

        [TestMethod]
        public void ControleerOpWanbetalingen_ReturnsNewWanBetalingen()
        {
            // Arrange
            Mock<IBestelRepository> bestelRepositoryMock = new Mock<IBestelRepository>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();
            IBestellingService bestellingService = new BestellingService(bestelRepositoryMock.Object, eventPublisherMock.Object);

            Bestelling oldBestelling1 = new Bestelling
            {
                Goedgekeurd = false,
                IsKlantWanbetaler = true,
                BestelDatum = DateTime.Now.AddDays(-Bestelling.DagenOmTeBetalen - 1)
            };

            Bestelling oldBestelling2 = new Bestelling
            {
                Goedgekeurd = true,
                IsKlantWanbetaler = false,
                BestelDatum = DateTime.Now.AddDays(-Bestelling.DagenOmTeBetalen - 200)
            };

            Bestelling expectedBestelling1 = new Bestelling
            {
                Goedgekeurd = false,
                IsKlantWanbetaler = false,
                BestelDatum = DateTime.Now.AddDays(-Bestelling.DagenOmTeBetalen - 1)
            };

            Bestelling expectedBestelling2 = new Bestelling
            {
                Goedgekeurd = false,
                IsKlantWanbetaler = false,
                BestelDatum = DateTime.Now.AddDays(-Bestelling.DagenOmTeBetalen - 5)
            };

            bestelRepositoryMock.Setup(e => e.GetAll())
                .Returns(new List<Bestelling> { oldBestelling1, oldBestelling2, expectedBestelling1, expectedBestelling2 });

            // Act
            IEnumerable<Bestelling> result = bestellingService.ControleerOpWanbetalingen().ToList();

            // Assert
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(true, result.Contains(expectedBestelling1));
            Assert.AreEqual(true, result.Contains(expectedBestelling2));
            Assert.AreEqual(false, result.Contains(oldBestelling1));
        }

        [TestMethod]
        [DataRow(1, "10001", "100.10", "50.99")]
        [DataRow(2, "10002", "199.99", "199.98")]
        public void RegistreerBetaling_LooksAtTheMostRecentUnassessedBestelling(int id, string bestellingNummer, string openstaandBedragStr, string betaaldBedragStr)
        {
            // Arrange
            decimal openstaandBedrag = decimal.Parse(openstaandBedragStr);
            decimal betaaldBedrag = decimal.Parse(betaaldBedragStr);
            Mock<IBestelRepository> bestelRepositoryMock = new Mock<IBestelRepository>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();
            IBestellingService bestellingService = new BestellingService(bestelRepositoryMock.Object, eventPublisherMock.Object);

            Bestelling bestelling = new Bestelling
            {
                Id = id,
                OpenstaandBedrag = openstaandBedrag,
                BestellingNummer = bestellingNummer
            };
            bestelRepositoryMock.Setup(e => e.GetByBestellingNummer(bestellingNummer))
                .Returns(bestelling);

            // Act
            bestellingService.RegistreerBetaling(bestellingNummer, betaaldBedrag);

            // Assert
            bestelRepositoryMock.Verify(e => e.GetMostRecentUnassessedBestelling());
        }

        [TestMethod]
        [DataRow(1, 10)]
        [DataRow(2, 20)]
        public void RegistreerBetaling_LooksAtTheMostRecentUnassessedBestellingAndPublishesEventIfItGetsGoedgekeurd(int id, int secondId)
        {
            // Arrange
            var bestelnummer = "1000" + id;
            var bestelnummer2 = "1000" + secondId;
            Mock<IBestelRepository> bestelRepositoryMock = new Mock<IBestelRepository>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();
            IBestellingService bestellingService = new BestellingService(bestelRepositoryMock.Object, eventPublisherMock.Object);

            Bestelling bestelling = new Bestelling
            {
                Id = id,
                OpenstaandBedrag = 400,
                BestellingNummer = bestelnummer
            };
            Bestelling bestelling2 = new Bestelling
            {
                Id = secondId,
                OpenstaandBedrag = 200,
                BestellingNummer = bestelnummer2,
                Klant = new Klant
                {
                    Bestellingen = new Bestelling[0]
                }
            };
            bestelRepositoryMock.Setup(e => e.GetByBestellingNummer(bestelnummer))
                .Returns(bestelling);
            bestelRepositoryMock.Setup(b => b.GetMostRecentUnassessedBestelling())
                .Returns(bestelling2);

            // Act
            bestellingService.RegistreerBetaling(bestelnummer, 400);

            // Assert
            eventPublisherMock.Verify(e => e.PublishAsync(It.Is<BestellingGoedgekeurdEvent>(
                b => b.BestellingId == secondId)));
        }

        [TestMethod]
        [DataRow(1, 10)]
        [DataRow(2, 20)]
        public void RegistreerBetaling_LooksAtTheMostRecentUnassessedBestellingAndDoesNotPublishesEventIfItDoesNotGetsGoedgekeurd(int id, int secondId)
        {
            // Arrange
            var bestelnummer = "1000" + id;
            var bestelnummer2 = "1000" + secondId;
            Mock<IBestelRepository> bestelRepositoryMock = new Mock<IBestelRepository>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();
            IBestellingService bestellingService = new BestellingService(bestelRepositoryMock.Object, eventPublisherMock.Object);

            Bestelling bestelling = new Bestelling
            {
                Id = id,
                OpenstaandBedrag = 400,
                BestellingNummer = bestelnummer
            };
            Bestelling bestelling2 = new Bestelling
            {
                Id = secondId,
                OpenstaandBedrag = 200,
                BestellingNummer = bestelnummer2,
                Klant = new Klant
                {
                    Bestellingen = new[]
                    {
                        new Bestelling
                        {
                            OpenstaandBedrag = 3000
                        }
                    }
                }
            };
            bestelRepositoryMock.Setup(e => e.GetByBestellingNummer(bestelnummer))
                .Returns(bestelling);
            bestelRepositoryMock.Setup(b => b.GetMostRecentUnassessedBestelling())
                .Returns(bestelling2);

            // Act
            bestellingService.RegistreerBetaling(bestelnummer, 400);

            // Assert
            eventPublisherMock.Verify(e => e.PublishAsync(It.Is<BestellingGoedgekeurdEvent>(
                b => b.BestellingId == secondId)), Times.Never);
        }
    }
}
