using System.Collections.Generic;
using System.Linq;
using BackOfficeFrontendService.Agents.Abstractions;
using BackOfficeFrontendService.Commands;
using BackOfficeFrontendService.EventListeners;
using BackOfficeFrontendService.Events;
using BackOfficeFrontendService.Models;
using BackOfficeFrontendService.Repositories.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BackOfficeFrontendService.Test.Unit.EventListeners
{
    [TestClass]
    public class BestellingEventListenersTest
    {
        [TestMethod]
        [DataRow("Sint Ansfridusstraat 121", "Amersfoort", "3817 BG", "123ksssda")]
        [DataRow("Donkerstraat 134", "Ravenswaaij", "4119 LX", "1233123")]
        public void HandleBestellingAangemaakt_CallsAddOnRepositoryWithBestelling(string straat, string plaats, string postcode, string bestellingNummer)
        {
            // Arrange
            var bestelling = new Bestelling
            {
                Id = 100,
                Klant = new Klant
                {
                    Factuuradres = new Adres
                    {
                        StraatnaamHuisnummer = straat,
                        Postcode = postcode,
                        Woonplaats = plaats
                    }
                },
                Goedgekeurd = false,
                BestellingNummer = bestellingNummer,
                KlaarGemeld = false
            };


            var bestelRepository = new Mock<IBestellingRepository>();
            var klantRepository = new Mock<IKlantRepository>();
            var voorraadRepository = new Mock<IVoorraadRepository>();
            var voorraadAgent = new Mock<IVoorraadAgent>();
            klantRepository.Setup(e => e.FindById(It.IsAny<long>()))
                .Returns(bestelling.Klant);

            var listener = new BestellingEventListeners(bestelRepository.Object, klantRepository.Object, voorraadAgent.Object, voorraadRepository.Object);
            var @event = new NieuweBestellingAangemaaktEvent { Bestelling = bestelling };

            // Act
            listener.HandleBestellingAangemaakt(@event);

            // Assert
            bestelRepository.Verify(e => e.Add(bestelling));
        }

        [TestMethod]
        [DataRow(20)]
        [DataRow(5920)]
        public void HandleBestellingAangemaakt_CallsGetByArtikelNummerOnVoorraadRepository(long artikelNummer)
        {
            // Arrange
            var bestelling = new Bestelling
            {
                Id = 100,
                Klant = new Klant
                {
                    Factuuradres = new Adres()
                },
                BestelRegels = new List<BestelRegel>
                {
                    new BestelRegel { ArtikelNummer = artikelNummer }
                }
            };

            VoorraadMagazijn voorraadMagazijn = new VoorraadMagazijn { ArtikelNummer = artikelNummer, Voorraad = 5 };

            var bestelRepository = new Mock<IBestellingRepository>();
            var klantRepository = new Mock<IKlantRepository>();
            var voorraadRepository = new Mock<IVoorraadRepository>();
            var voorraadAgent = new Mock<IVoorraadAgent>();
            klantRepository.Setup(e => e.FindById(It.IsAny<long>()))
                .Returns(bestelling.Klant);

            voorraadRepository.Setup(e => e.GetByArtikelNummer(artikelNummer))
                .Returns(voorraadMagazijn)
                .Verifiable();

            var listener = new BestellingEventListeners(bestelRepository.Object, klantRepository.Object, voorraadAgent.Object, voorraadRepository.Object);
            var @event = new NieuweBestellingAangemaaktEvent { Bestelling = bestelling };

            // Act
            listener.HandleBestellingAangemaakt(@event);

            // Assert
            voorraadRepository.Verify();
        }

        [TestMethod]
        [DataRow(20)]
        [DataRow(5920)]
        public void HandleBestellingAangemaakt_CallsGetByIdOnKlantRepository(long artikelNummer)
        {
            // Arrange
            var bestelling = new Bestelling
            {
                Id = 100,
                Klant = new Klant
                {
                    Factuuradres = new Adres()
                },
                BestelRegels = new List<BestelRegel>
                {
                    new BestelRegel { ArtikelNummer = artikelNummer }
                }
            };

            VoorraadMagazijn voorraadMagazijn = new VoorraadMagazijn { ArtikelNummer = artikelNummer, Voorraad = 5 };

            var bestelRepository = new Mock<IBestellingRepository>();
            var klantRepository = new Mock<IKlantRepository>();
            var voorraadRepository = new Mock<IVoorraadRepository>();
            var voorraadAgent = new Mock<IVoorraadAgent>();
            klantRepository.Setup(e => e.FindById(bestelling.Klant.Id))
                .Returns(bestelling.Klant)
                .Verifiable(); ;

            var listener = new BestellingEventListeners(bestelRepository.Object, klantRepository.Object, voorraadAgent.Object, voorraadRepository.Object);
            var @event = new NieuweBestellingAangemaaktEvent { Bestelling = bestelling };

            // Act
            listener.HandleBestellingAangemaakt(@event);

            // Assert
            klantRepository.Verify();
        }

        [TestMethod]
        [DataRow(424)]
        [DataRow(4624)]
        public void HandleBestellingGoedgekeurd_CallsUpdateOnRepositoriesWithProperValues(long bestellingId)
        {
            // Arrange
            var bestelRepository = new Mock<IBestellingRepository>();
            var klantRepository = new Mock<IKlantRepository>();
            var voorraadRepository = new Mock<IVoorraadRepository>();
            var voorraadAgent = new Mock<IVoorraadAgent>();

            var listener = new BestellingEventListeners(bestelRepository.Object, klantRepository.Object, voorraadAgent.Object, voorraadRepository.Object);
            var @event = new BestellingGoedgekeurdEvent { BestellingId = bestellingId };

            Bestelling bestelling = new Bestelling
            {
                Goedgekeurd = false
            };

            bestelRepository.Setup(e => e.GetInpakOpdrachtMetId(bestellingId))
                .Returns(bestelling);

            // Act
            listener.HandleBestellingGoedgekeurd(@event);

            // Assert
            bestelRepository.Verify(e => e.Update(It.Is<Bestelling>(b => b.Goedgekeurd)));
        }

        [TestMethod]
        [DataRow(424)]
        [DataRow(4624)]
        public void HandleBestellingAfgekeurd_CallsUpdateOnRepositoriesWithProperValues(long bestellingId)
        {
            // Arrange
            var bestelRepository = new Mock<IBestellingRepository>();
            var klantRepository = new Mock<IKlantRepository>();
            var voorraadRepository = new Mock<IVoorraadRepository>();
            var voorraadAgent = new Mock<IVoorraadAgent>();

            var listener = new BestellingEventListeners(bestelRepository.Object, klantRepository.Object, voorraadAgent.Object, voorraadRepository.Object);
            var @event = new BestellingAfgekeurdEvent { BestellingId = bestellingId };

            Bestelling bestelling = new Bestelling
            {
                Afgekeurd = false
            };

            bestelRepository.Setup(e => e.GetInpakOpdrachtMetId(bestellingId))
                .Returns(bestelling);

            // Act
            listener.HandleBestellingAfgekeurd(@event);

            // Assert
            bestelRepository.Verify(e => e.Update(It.Is<Bestelling>(b => b.Afgekeurd)));
        }

        [TestMethod]
        [DataRow(424)]
        [DataRow(4624)]
        public void HandleBestellingFactuurGeprint_CallsUpdateOnRepositoriesWithProperValues(long bestellingId)
        {
            // Arrange
            var bestelRepository = new Mock<IBestellingRepository>();
            var klantRepository = new Mock<IKlantRepository>();
            var voorraadRepository = new Mock<IVoorraadRepository>();
            var voorraadAgent = new Mock<IVoorraadAgent>();

            var listener = new BestellingEventListeners(bestelRepository.Object, klantRepository.Object, voorraadAgent.Object, voorraadRepository.Object);
            var @event = new BestellingFactuurGeprintEvent { BestellingId = bestellingId };

            Bestelling bestelling = new Bestelling
            {
                Afgekeurd = false
            };

            bestelRepository.Setup(e => e.GetInpakOpdrachtMetId(bestellingId))
                .Returns(bestelling);

            // Act
            listener.HandleBestellingFactuurGeprint(@event);

            // Assert
            bestelRepository.Verify(e => e.Update(It.Is<Bestelling>(b => b.FactuurGeprint)));
        }

        [TestMethod]
        [DataRow(424)]
        [DataRow(4624)]
        public void HandleBestellingAdresLabelGeprintEvent_CallsAdresLabelGeprintWithBestellingId(long bestellingId)
        {
            // Arrange
            var bestelRepository = new Mock<IBestellingRepository>();
            var klantRepository = new Mock<IKlantRepository>();
            var voorraadRepository = new Mock<IVoorraadRepository>();
            var voorraadAgent = new Mock<IVoorraadAgent>();

            var listener = new BestellingEventListeners(bestelRepository.Object, klantRepository.Object, voorraadAgent.Object, voorraadRepository.Object);
            var @event = new BestellingAdresLabelGeprintEvent { BestellingId = bestellingId };

            Bestelling bestelling = new Bestelling
            {
                Afgekeurd = false
            };

            bestelRepository.Setup(e => e.GetInpakOpdrachtMetId(bestellingId))
                .Returns(bestelling);

            // Act
            listener.HandleBestellingAdresLabelGeprint(@event);

            // Assert
            bestelRepository.Verify(e => e.Update(It.Is<Bestelling>(b => b.AdresLabelGeprint)));
        }

        [TestMethod]
        [DataRow(424, 2873)]
        [DataRow(4624, 37249)]
        public void HandleBestellingKlaargemeld_CallsUpdateOnRepositoryWithKlaargemeldTrue(long bestellingId, long regelId)
        {
            // Arrange
            var bestelRepository = new Mock<IBestellingRepository>();
            var klantRepository = new Mock<IKlantRepository>();
            var voorraadRepository = new Mock<IVoorraadRepository>();
            var voorraadAgent = new Mock<IVoorraadAgent>();

            var listener = new BestellingEventListeners(bestelRepository.Object, klantRepository.Object, voorraadAgent.Object, voorraadRepository.Object);
            var @event = new BestellingKlaarGemeldEvent { BestellingId = bestellingId };

            Bestelling bestelling = new Bestelling { KlaarGemeld = false };

            bestelRepository.Setup(e => e.GetInpakOpdrachtMetId(bestellingId))
                .Returns(bestelling);

            // Act
            listener.HandleBestellingKlaargemeld(@event);

            // Assert
            bestelRepository.Verify(e => e.Update(It.Is<Bestelling>(b => b.KlaarGemeld)));
        }

        [TestMethod]
        [DataRow(424, 2873)]
        [DataRow(4624, 37249)]
        public void HandleBestelRegelIngepakt_CallsUpdateOnRepositoryWithIngepaktTrue(long bestellingId, long regelId)
        {
            // Arrange
            var bestelRepository = new Mock<IBestellingRepository>();
            var klantRepository = new Mock<IKlantRepository>();
            var voorraadRepository = new Mock<IVoorraadRepository>();
            var voorraadAgent = new Mock<IVoorraadAgent>();

            var listener = new BestellingEventListeners(bestelRepository.Object, klantRepository.Object, voorraadAgent.Object, voorraadRepository.Object);
            var @event = new BestelRegelIngepaktEvent { BestellingId = bestellingId, BestelRegelId = regelId };

            BestelRegel regel1 = new BestelRegel { Id = regelId, Ingepakt = false };
            Bestelling bestelling = new Bestelling();
            bestelling.BestelRegels.Add(regel1);

            bestelRepository.Setup(e => e.GetInpakOpdrachtMetId(bestellingId))
                .Returns(bestelling);

            // Act
            listener.HandleBestelRegelIngepakt(@event);

            // Assert
            bestelRepository.Verify(e => e.Update(It.Is<Bestelling>(b => b.BestelRegels.FirstOrDefault().Ingepakt)));
        }

        [TestMethod]
        [DataRow(424)]
        [DataRow(4624)]
        public void HandleKanKlaarGemeldWorden_CallsUpdateOnRepositoryWithKanKlaarGemeldWordenTrue(long bestellingId)
        {
            // Arrange
            var bestelRepository = new Mock<IBestellingRepository>();
            var klantRepository = new Mock<IKlantRepository>();
            var voorraadRepository = new Mock<IVoorraadRepository>();
            var voorraadAgent = new Mock<IVoorraadAgent>();

            var listener = new BestellingEventListeners(bestelRepository.Object, klantRepository.Object, voorraadAgent.Object, voorraadRepository.Object);
            var @event = new BestellingKanKlaarGemeldWordenEvent { BestellingId = bestellingId };

            Bestelling bestelling = new Bestelling { KanKlaarGemeldWorden = true };

            bestelRepository.Setup(e => e.GetInpakOpdrachtMetId(bestellingId))
                .Returns(bestelling);

            // Act
            listener.HandleKanKlaarGemeldWorden(@event);

            // Assert
            bestelRepository.Verify(e => e.Update(It.Is<Bestelling>(b => b.KanKlaarGemeldWorden)));
        }

        [TestMethod]
        [DataRow(2)]
        [DataRow(7)]
        public void HandleBestellingKlaarGemeld_CallsHaalVoorraadUitMagazijnForEachBestelRegel(int amount)
        {
            // Arrange
            var bestelRepository = new Mock<IBestellingRepository>();
            var klantRepository = new Mock<IKlantRepository>();
            var voorraadRepository = new Mock<IVoorraadRepository>();
            var voorraadAgent = new Mock<IVoorraadAgent>();

            var listener = new BestellingEventListeners(bestelRepository.Object, klantRepository.Object, voorraadAgent.Object, voorraadRepository.Object);
            var @event = new BestellingKlaarGemeldEvent { BestellingId = 5 };

            Bestelling bestelling = new Bestelling
            {
                KanKlaarGemeldWorden = true,
                BestelRegels = Enumerable.Range(0, amount).Select(e => new BestelRegel { Voorraad = new VoorraadMagazijn() }).ToList()
            };

            bestelRepository.Setup(e => e.GetInpakOpdrachtMetId(5))
                .Returns(bestelling);

            // Act
            listener.HandleBestellingKlaargemeld(@event);

            // Assert
            voorraadAgent.Verify(e =>
                e.HaalVoorraadUitMagazijnAsync(It.IsAny<HaalVoorraadUitMagazijnCommand>()), Times.Exactly(amount));
        }

        [TestMethod]
        [DataRow(2, 10, 15, 5)]
        [DataRow(7, 30, 32, 2)]
        [DataRow(25, 2, 4, 2)]
        public void HandleBestellingKlaarGemeld_CallsHaalVoorraadUitMagazijnWithProperValues(int artikelNummer, int aantal, int voorraad, int newVoorraad)
        {
            // Arrange
            var bestelRepository = new Mock<IBestellingRepository>();
            var klantRepository = new Mock<IKlantRepository>();
            var voorraadRepository = new Mock<IVoorraadRepository>();
            var voorraadAgent = new Mock<IVoorraadAgent>();

            var listener = new BestellingEventListeners(bestelRepository.Object, klantRepository.Object, voorraadAgent.Object, voorraadRepository.Object);
            var @event = new BestellingKlaarGemeldEvent { BestellingId = 5 };

            Bestelling bestelling = new Bestelling
            {
                KanKlaarGemeldWorden = true,
                BestelRegels = new List<BestelRegel>
                {
                    new BestelRegel { ArtikelNummer = artikelNummer, Aantal = aantal, Voorraad = new VoorraadMagazijn { Voorraad = voorraad }}
                }
            };

            bestelRepository.Setup(e => e.GetInpakOpdrachtMetId(5))
                .Returns(bestelling);

            // Act
            listener.HandleBestellingKlaargemeld(@event);

            // Assert
            voorraadAgent.Verify(e =>
                e.HaalVoorraadUitMagazijnAsync(It.Is<HaalVoorraadUitMagazijnCommand>(v => v.Aantal == newVoorraad && v.Artikelnummer == artikelNummer)));
        }

        [TestMethod]
        [DataRow(424, "500")]
        [DataRow(4624, "700")]
        public void HandleBetalingGeregistreerd_CallsUpdateOnRepositoryWithKanKlaarGemeldWordenTrue(long bestellingId, string openstaandStr)
        {
            // Arrange
            decimal openstaand = decimal.Parse(openstaandStr);

            var bestelRepository = new Mock<IBestellingRepository>();
            var klantRepository = new Mock<IKlantRepository>();
            var voorraadRepository = new Mock<IVoorraadRepository>();
            var voorraadAgent = new Mock<IVoorraadAgent>();

            var listener = new BestellingEventListeners(bestelRepository.Object, klantRepository.Object, voorraadAgent.Object, voorraadRepository.Object);
            var @event = new BetalingGeregistreerdEvent { BestellingId = bestellingId, OpenstaandBedrag = openstaand};

            Bestelling bestelling = new Bestelling { OpenstaandBedrag = openstaand };

            bestelRepository.Setup(e => e.GetInpakOpdrachtMetId(bestellingId))
                .Returns(bestelling);

            // Act
            listener.HandleBetalingGeregistreerd(@event);

            // Assert
            bestelRepository.Verify(e =>
                e.Update(It.Is<Bestelling>(b => b.OpenstaandBedrag == openstaand )));
        }

        [TestMethod]
        [DataRow(10)]
        [DataRow(50)]
        public void HandleKlantIsWanbetalerGeworden_CallsGetByIdOnRepository(int bestellingId)
        {
            var bestelRepository = new Mock<IBestellingRepository>();
            var klantRepository = new Mock<IKlantRepository>();
            var voorraadRepository = new Mock<IVoorraadRepository>();
            var voorraadAgent = new Mock<IVoorraadAgent>();

            var listener = new BestellingEventListeners(bestelRepository.Object, klantRepository.Object, voorraadAgent.Object, voorraadRepository.Object);
            var @event = new KlantIsWanbetalerGewordenEvent { BestellingId = bestellingId };

            Bestelling bestelling = new Bestelling();

            bestelRepository.Setup(e => e.GetInpakOpdrachtMetId(bestellingId))
                .Returns(bestelling)
                .Verifiable();

            // Act
            listener.HandleKlantIsWanbetalerGeworden(@event);

            // Assert
            bestelRepository.Verify();
        }

        [TestMethod]
        [DataRow(20)]
        [DataRow(592)]
        public void HandleKlantIsWanbetalerGeworden_CalslUpdateOnRepositoryWithWanbetalerTrue(int bestellingId)
        {
            var bestelRepository = new Mock<IBestellingRepository>();
            var klantRepository = new Mock<IKlantRepository>();
            var voorraadRepository = new Mock<IVoorraadRepository>();
            var voorraadAgent = new Mock<IVoorraadAgent>();

            var listener = new BestellingEventListeners(bestelRepository.Object, klantRepository.Object, voorraadAgent.Object, voorraadRepository.Object);
            var @event = new KlantIsWanbetalerGewordenEvent { BestellingId = bestellingId };

            Bestelling bestelling = new Bestelling { Id = bestellingId };

            bestelRepository.Setup(e => e.GetInpakOpdrachtMetId(bestellingId))
                .Returns(bestelling);

            // Act
            listener.HandleKlantIsWanbetalerGeworden(@event);

            // Assert
            bestelRepository.Verify(e =>
                e.Update(It.Is<Bestelling>(b => b.IsKlantWanbetaler && b.Id == bestellingId)));
        }
    }
}
