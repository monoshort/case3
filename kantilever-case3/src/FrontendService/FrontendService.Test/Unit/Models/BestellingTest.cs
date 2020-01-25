using System.Collections.Generic;
using FrontendService.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FrontendService.Test.Unit.Models
{
    [TestClass]
    public class BestellingTest
    {
        [TestMethod]
        public void Subtotaal_IsZeroOnNoBestelRegels()
        {
            // Act
            Bestelling bestelling = new Bestelling();

            // Assert
            Assert.AreEqual(0M, bestelling.Subtotaal);
        }

        [TestMethod]
        [DataRow(5.25)]
        [DataRow(52.35)]
        public void Subtotaal_IsEqualToRegelTotaalIfItHasOneRegel(double stukPrijs)
        {
            // Arrange
            BestelRegel bestelRegel = new BestelRegel { Aantal = 1, StukPrijs = (decimal)stukPrijs };

            // Act
            Bestelling bestelling = new Bestelling
            {
                BestelRegels = { bestelRegel }
            };

            // Assert
            Assert.AreEqual(bestelRegel.RegelPrijs, bestelling.Subtotaal);
        }

        [TestMethod]
        public void Subtotaal_IsEqualToTheSumOfBestelRegels()
        {
            // Act
            Bestelling bestelling = new Bestelling
            {
                BestelRegels =
                {
                    new BestelRegel {Aantal = 4, StukPrijs = 15M},
                    new BestelRegel {Aantal = 2, StukPrijs = 2M}
                }
            };

            // Assert
            Assert.AreEqual(64.00M, bestelling.Subtotaal);
        }

        [TestMethod]
        public void SubtotaalInclusiefBtw_IsEqualToTheSumOfBestelRegelsInclusiefBtw()
        {
            // Act
            Bestelling bestelling = new Bestelling
            {
                BestelRegels =
                {
                    new BestelRegel {Aantal = 4, StukPrijs = 15M},
                    new BestelRegel {Aantal = 2, StukPrijs = 2M}
                }
            };

            // Assert
            Assert.AreEqual(77.44M, bestelling.SubtotaalInclusiefBtw);
        }

        [TestMethod]
        [DataRow(1.00, 5.96)]
        [DataRow(1.50, 6.46)]
        [DataRow(2.00, 6.96)]
        [DataRow(6.00, 10.96)]
        [DataRow(32.00, 36.96)]
        public void SubtotaalMetVerzendKosten_AddsVerzendKostenToSubtotaal(double subTotaalD, double expectedD)
        {
            // Arrange
            decimal subTotaal = (decimal) subTotaalD;
            decimal expected = (decimal) expectedD;

            // Act
            Bestelling bestelling = new Bestelling
            {
                BestelRegels = new List<BestelRegel>
                {
                    new BestelRegel {Aantal = 1, StukPrijs = subTotaal}
                }
            };

            // Assert
            Assert.AreEqual(expected, bestelling.SubtotaalMetVerzendKosten);
        }

        [TestMethod]
        [DataRow(2.00, 8.42)]
        [DataRow(16.00, 25.36)]
        [DataRow(32.00, 44.72)]
        public void SubtotaalInclusiefBtwMetVerzendKosten_AddsVerzendKostenToSubtotaal(double subTotaal, double expected)
        {
            // Act
            Bestelling bestelling = new Bestelling
            {
                BestelRegels = new List<BestelRegel>
                {
                    new BestelRegel {Aantal = 1, StukPrijs = (decimal) subTotaal}
                }
            };

            // Assert
            Assert.AreEqual((decimal) expected, bestelling.SubtotaalInclusiefBtwMetVerzendKosten);
        }

        [TestMethod]
        [DataRow(10)]
        [DataRow(50)]
        public void OpenstaandBedrag_EqualsSubtotaalInclusiefBtwMetVerzendKosten(double subTotaal)
        {
            // Act
            Bestelling bestelling = new Bestelling
            {
                BestelRegels = new List<BestelRegel>
                {
                    new BestelRegel {Aantal = 1, StukPrijs = (decimal) subTotaal}
                }
            };

            // Assert
            Assert.AreEqual(bestelling.OpenstaandBedrag, bestelling.SubtotaalInclusiefBtwMetVerzendKosten);
        }
    }
}
