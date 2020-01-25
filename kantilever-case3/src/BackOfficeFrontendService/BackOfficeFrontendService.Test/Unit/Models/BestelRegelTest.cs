using BackOfficeFrontendService.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackOfficeFrontendService.Test.Unit.Models
{
    [TestClass]
    public class BestelRegelTest
    {
        [TestMethod]
        [DataRow(10, 5, true)]
        [DataRow(7, 7, true)]
        [DataRow(5, 7, false)]
        [DataRow(2, 7, false)]
        public void VoorraadBeschikbaar_IsProperlyDecided(int opVoorraad, int aantal, bool expected)
        {
            // Arrange
            BestelRegel bestelRegel = new BestelRegel
            {
                Aantal = aantal,
                Voorraad = new VoorraadMagazijn
                {
                    Voorraad = opVoorraad
                }
            };

            // Act
            bool result = bestelRegel.VoorraadBeschikbaar;

            // Assert
            Assert.AreEqual(expected, result);
        }
    }
}
