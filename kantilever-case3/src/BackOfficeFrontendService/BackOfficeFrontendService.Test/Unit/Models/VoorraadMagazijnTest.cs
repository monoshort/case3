using System.Collections.Generic;
using BackOfficeFrontendService.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackOfficeFrontendService.Test.Unit.Models
{
    [TestClass]
    public class VoorraadMagazijnTest
    {
        [TestMethod]
        [DataRow(10, 20, true)]
        [DataRow(4, 4, false)]
        [DataRow(10, 14, true)]
        [DataRow(14, 10, false)]
        public void IsBijbestellenNodig_ShouldReturnExpectedValue(int voorraad, int besteld, bool expected)
        {
            // Arrange
            VoorraadMagazijn voorraadMagazijn = new VoorraadMagazijn
            {
                Voorraad = voorraad,
                BestelRegels = new List<BestelRegel>
                {
                    new BestelRegel { Aantal = besteld, Bestelling = new Bestelling()}
                }
            };

            // Act
            bool result = voorraadMagazijn.IsBijbestellenNodig;

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void IsBijbestellenNodig_DoesNotCountKlaarGemeldeBestelRegels()
        {
            // Arrange
            VoorraadMagazijn voorraadMagazijn = new VoorraadMagazijn
            {
                Voorraad = 4,
                BestelRegels = new List<BestelRegel>
                {
                    new BestelRegel
                    {
                        Aantal = 6,
                        Bestelling = new Bestelling
                        {
                            KlaarGemeld = true
                        }
                    }
                }
            };

            // Act
            bool result = voorraadMagazijn.IsBijbestellenNodig;

            // Assert
            Assert.AreEqual(false, result);
        }
    }
}
