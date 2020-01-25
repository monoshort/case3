using System;
using System.Collections.Generic;
using BestelService.Core.Exceptions;
using BestelService.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BestelService.Core.Test.Unit.Models
{
    [TestClass]
    public class BestellingTest
    {
        [TestMethod]
        public void KeurAf_SetsIsAfgekeurdToTrue()
        {
            // Arrange
            Bestelling bestelling = new Bestelling();

            // Act
            bestelling.KeurAf();

            // Assert
            Assert.AreEqual(true, bestelling.Afgekeurd);
        }

        [TestMethod]
        public void Constructor_AfgekeurdIsInitiallyFalse()
        {
            // Act
            Bestelling bestelling = new Bestelling();

            // Assert
            Assert.AreEqual(false, bestelling.Afgekeurd);
        }

        [TestMethod]
        public void KeurGoed_SetsIsGoedgekeurdToTrue()
        {
            // Arrange
            Bestelling bestelling = new Bestelling();

            // Act
            bestelling.KeurGoed();

            // Assert
            Assert.AreEqual(true, bestelling.Goedgekeurd);
        }

        [TestMethod]
        public void MeldKlaar_SetsIsKlaarGemeldToTrueIAllRegelsAreIngepaktAndGeprint()
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                Goedgekeurd = true,
                AdresLabelGeprint = true,
                FactuurGeprint = true,
                BestelRegels = new List<BestelRegel>
                {
                    new BestelRegel { Ingepakt = true }
                }
            };

            // Act
            bestelling.MeldKlaar();

            // Assert
            Assert.AreEqual(true, bestelling.KlaarGemeld);
        }

        [TestMethod]
        public void MeldKlaar_ThrowsExceptionIfFactuurIsNotPrinted()
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                Id = 2,
                AdresLabelGeprint = true,
                FactuurGeprint = false
            };

            // Act
            void Act() => bestelling.MeldKlaar();

            // Assert
            var exception = Assert.ThrowsException<BestellingKanNietKlaargemeldWordenException>(Act);
            Assert.AreEqual(Bestelling.FactuurIsNietUigeprintMessage, exception.Message);
        }

        [TestMethod]
        public void MeldKlaar_ThrowsExceptionIfAdresLabelIsNotPrinted()
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                Id = 2,
                AdresLabelGeprint = false,
                FactuurGeprint = true
            };

            // Act
            void Act() => bestelling.MeldKlaar();

            // Assert
            var exception = Assert.ThrowsException<BestellingKanNietKlaargemeldWordenException>(Act);
            Assert.AreEqual(Bestelling.AdresLabelIsNietUigeprintMessage, exception.Message);
        }

        [TestMethod]
        public void MeldKlaar_ThrowsExceptionIfNotAllBestelRegelsAreIngepakt()
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                Id = 2,
                AdresLabelGeprint = true,
                FactuurGeprint = true,
                BestelRegels = new List<BestelRegel>
                {
                    new BestelRegel { Ingepakt = true },
                    new BestelRegel { Ingepakt = true },
                    new BestelRegel { Ingepakt = false },
                }
            };

            // Act
            void Act() => bestelling.MeldKlaar();

            // Assert
            var exception = Assert.ThrowsException<BestellingKanNietKlaargemeldWordenException>(Act);
            Assert.AreEqual(Bestelling.BestelRegelsZijnNogNietIngepaktMessage, exception.Message);
        }

        [TestMethod]
        public void Constructor_KlaarGemeldIsInitiallyFalse()
        {
            // Act
            Bestelling bestelling = new Bestelling();

            // Assert
            Assert.AreEqual(false, bestelling.KlaarGemeld);
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(204929)]
        public void VoegBestelNummerToe_AddsBestelNummerToItemBasedOnId(long id)
        {
            // Arrange
            Bestelling bestelling = new Bestelling { Id = id };

            // Act
            bestelling.VoegBestelnummerToe();

            // Assert
            string resultBestelnummer = (id + Bestelling.BestelNummerStartNumber).ToString();
            Assert.AreEqual(resultBestelnummer, bestelling.BestellingNummer);
        }

        [TestMethod]
        [DataRow(200, true)]
        [DataRow(400, true)]
        [DataRow(499, true)]
        [DataRow(500, true)]
        [DataRow(501, false)]
        [DataRow(800, false)]
        public void ControleerOfBestellingAutomatischGoedgekeurdIs_ProperlySetsGoedgekeurd(double subtotaalInclusiefBtw, bool isGoedgekeurd)
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                OpenstaandBedrag = (decimal) subtotaalInclusiefBtw
            };

            bestelling.Klant = new Klant();
            bestelling.Klant.Bestellingen.Add(bestelling);

            // Act
            bestelling.ControleerOfBestellingAutomatischGoedgekeurdKanWorden();

            // Assert
            Assert.AreEqual(isGoedgekeurd, bestelling.Goedgekeurd);
        }

        [TestMethod]
        public void Constructor_GoedgekeurdIsInitiallyFalse()
        {
            // Act
            Bestelling bestelling = new Bestelling();

            // Assert
            Assert.AreEqual(false, bestelling.Goedgekeurd);
        }

        [TestMethod]
        public void Constructor_FactuurGeprintIsInitiallyFalse()
        {
            // Act
            Bestelling bestelling = new Bestelling();

            // Assert
            Assert.AreEqual(false, bestelling.FactuurGeprint);
        }

        [TestMethod]
        public void PrintFactuur_SetsFactuurGeprintToTrue()
        {
            // Arrange
            Bestelling bestelling = new Bestelling();

            // Act
            bestelling.PrintFactuur();

            // Assert
            Assert.AreEqual(true, bestelling.FactuurGeprint);
        }

        [TestMethod]
        public void Constructor_AdresLabelGeprintIsInitiallyFalse()
        {
            // Act
            Bestelling bestelling = new Bestelling();

            // Assert
            Assert.AreEqual(false, bestelling.AdresLabelGeprint);
        }

        [TestMethod]
        public void PrintAdresLabel_SetsAdresLabelGeprintToTrue()
        {
            // Arrange
            Bestelling bestelling = new Bestelling();

            // Act
            bestelling.PrintAdresLabel();

            // Assert
            Assert.AreEqual(true, bestelling.AdresLabelGeprint);
        }

        [TestMethod]
        [DataRow("100.00", "50.00")]
        [DataRow("100.99", "100.98")]
        public void BoekBedragAf_PaysPartOfTheOpenstaandBedrag(string openstaandStr, string bedragStr)
        {
            // Arrange
            var openstaand = decimal.Parse(openstaandStr);
            var bedrag = decimal.Parse(bedragStr);
            Bestelling bestelling = new Bestelling
            {
                OpenstaandBedrag = openstaand
            };

            // Act
            bestelling.BoekBedragAf(bedrag);

            // Assert
            var expected = openstaand - bedrag;
            Assert.AreEqual(expected, bestelling.OpenstaandBedrag);
        }
        [TestMethod]
        [DataRow("100.00", "50.00")]
        [DataRow("100.99", "100.98")]
        public void BoekBedragAf_CannotBoekAfMoreThanIsOpenstaand(string bedragStr, string openstaandStr)
        {
            // Arrange
            var bedrag = decimal.Parse(bedragStr);
            var openstaand = decimal.Parse(openstaandStr);
            Bestelling bestelling = new Bestelling
            {
                OpenstaandBedrag = openstaand
            };

            // Act
            Action action = () => bestelling.BoekBedragAf(bedrag);

            // Assert
            Assert.ThrowsException<BedragKanNietWordenAfgeboektWordenException>(action);
        }

        [TestMethod]
        public void PrintFactuur_CallsKanIngepaktWordenEventIFReady()
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                Klant = new Klant(),
                AdresLabelGeprint = true
            };

            bool eventCalled = false;
            bestelling.BestellingKanKlaargemeldWorden += (bestelling1, args) => eventCalled = true;

            // Act
            bestelling.PrintFactuur();

            // Assert
            Assert.AreEqual(true, eventCalled);
        }

        [TestMethod]
        public void PrintFactuur_DoesNotCallKanIngepaktWordenEventIfNotReady()
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                Klant = new Klant()
            };

            bool eventCalled = false;
            bestelling.BestellingKanKlaargemeldWorden += (bestelling1, args) => eventCalled = true;

            // Act
            bestelling.PrintFactuur();

            // Assert
            Assert.AreEqual(false, eventCalled);
        }

        [TestMethod]
        public void PrintAdresLabel_CallsKanIngepaktWordenEventIFReady()
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                Klant = new Klant(),
                FactuurGeprint = true
            };

            bool eventCalled = false;
            bestelling.BestellingKanKlaargemeldWorden += (bestelling1, args) => eventCalled = true;

            // Act
            bestelling.PrintAdresLabel();

            // Assert
            Assert.AreEqual(true, eventCalled);
        }

        [TestMethod]
        public void PrintAdresLabel_DoesNotCallKanIngepaktWordenEventIfNotReady()
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                Klant = new Klant(),
            };

            bool eventCalled = false;
            bestelling.BestellingKanKlaargemeldWorden += (bestelling1, args) => eventCalled = true;

            // Act
            bestelling.PrintAdresLabel();

            // Assert
            Assert.AreEqual(false, eventCalled);
        }

        [TestMethod]
        [DataRow(10)]
        [DataRow(500)]
        public void PakIn_CallsKanIngepaktWordenEventIfReady(long id)
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                Klant = new Klant(),
                FactuurGeprint = true,
                AdresLabelGeprint = true,
                BestelRegels = new List<BestelRegel> {new BestelRegel { Id = id }}
            };

            bool eventCalled = false;
            bestelling.BestellingKanKlaargemeldWorden += (bestelling1, args) => eventCalled = true;

            // Act
            bestelling.PakIn(id);

            // Assert
            Assert.AreEqual(true, eventCalled);
        }

        [TestMethod]
        [DataRow(10)]
        [DataRow(500)]
        public void PakIn_DoesNotCallKanIngepaktWordenEventIfNotReady(long id)
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                Klant = new Klant(),
                BestelRegels = new List<BestelRegel> {new BestelRegel { Id = id }}
            };

            bool eventCalled = false;
            bestelling.BestellingKanKlaargemeldWorden += (bestelling1, args) => eventCalled = true;

            // Act
            bestelling.PakIn(id);

            // Assert
            Assert.AreEqual(false, eventCalled);
        }

        [TestMethod]
        [DataRow(0, false)]
        [DataRow(Bestelling.DagenOmTeBetalen - 1, false)]
        [DataRow(Bestelling.DagenOmTeBetalen, true)]
        [DataRow(Bestelling.DagenOmTeBetalen + 1, true)]
        public void ControleerOfKlantWanbetalerIs_SetsIsKlantWanbetalerExpectedValueOnBestelDatum(int daysAgo, bool expected)
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                BestelDatum = DateTime.Now.AddDays(-daysAgo)
            };

            // Act
            bestelling.ControleerOfKlantWanbetalerIs();

            // Assert
            Assert.AreEqual(expected, bestelling.IsKlantWanbetaler);
        }

        [TestMethod]
        public void ControleerOfKlantWanbetalerIs_SetsIsKlantWanbetalerFalseIfGoedgekeurd()
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                BestelDatum = DateTime.Now.AddDays(-50),
                Goedgekeurd = true
            };

            // Act
            bestelling.ControleerOfKlantWanbetalerIs();

            // Assert
            Assert.AreEqual(false, bestelling.IsKlantWanbetaler);
        }

        [TestMethod]
        public void ControleerOfKlantWanbetalerIs_SetsIsKlantWanbetalerFalseIfAfgekeurd()
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                BestelDatum = DateTime.Now.AddDays(-50),
                Afgekeurd = true
            };

            // Act
            bestelling.ControleerOfKlantWanbetalerIs();

            // Assert
            Assert.AreEqual(false, bestelling.IsKlantWanbetaler);
        }

        [TestMethod]
        public void ControleerOfKlantWanbetalerIs_CallsBestellingKlantIsWanbetalerGewordenOnWanbetaler()
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                BestelDatum = DateTime.Now.AddDays(-50),
                Afgekeurd = true
            };


            bool eventCalled = false;
            bestelling.BestellingKlantIsWanbetalerGeworden += (bestelling, args) => { eventCalled = true; };

            // Act
            bestelling.ControleerOfKlantWanbetalerIs();

            // Assert
            Assert.AreEqual(false, eventCalled);
        }

        [TestMethod]
        public void ControleerOfKlantWanbetalerIs_DoesNotCallBestellingKlantIsWanbetalerGewordenOnNietWanbetaler()
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                BestelDatum = DateTime.Now,
                Afgekeurd = true
            };


            bool eventCalled = false;
            bestelling.BestellingKlantIsWanbetalerGeworden += (bestelling, args) => { eventCalled = true; };

            // Act
            bestelling.ControleerOfKlantWanbetalerIs();

            // Assert
            Assert.AreEqual(false, eventCalled);
        }

        [TestMethod]
        public void ControleerOfKlantWanbetalerIs_DoesNotCallBestellingKlantIsWanbetalerGewordenIfKlantIsAlreadyWanbetaler()
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                BestelDatum = DateTime.Now,
                Afgekeurd = true,
                IsKlantWanbetaler = true
            };

            bool eventCalled = false;
            bestelling.BestellingKlantIsWanbetalerGeworden += (bestelling, args) => { eventCalled = true; };

            // Act
            bestelling.ControleerOfKlantWanbetalerIs();

            // Assert
            Assert.AreEqual(false, eventCalled);
        }
    }
}
