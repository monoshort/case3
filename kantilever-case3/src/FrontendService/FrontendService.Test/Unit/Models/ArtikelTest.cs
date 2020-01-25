using FrontendService.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FrontendService.Test.Unit.Models
{
    [TestClass]
    public class ArtikelTest
    {
        [TestMethod]
        [DataRow(20, 24.2)]
        [DataRow(25.25, 30.55)]
        [DataRow(193.23, 233.81)]
        public void PrijsInclBtw_ReturnsPrijsMultipliedByMultiplier(double prijsD, double resultD)
        {
            // Arrange
            var prijs = (decimal) prijsD;
            var expectedResult = (decimal) resultD;
            Artikel artikel = new Artikel { Prijs = prijs };

            // Act
            decimal result = artikel.PrijsInclBtw;

            // Assert
            Assert.AreEqual(expectedResult, result);
        }
    }
}
