using FrontendService.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FrontendService.Test.Unit.Models
{
    /// NOTICE: Some price parameters are doubles because of constant expressions in attributes
    [TestClass]
    public class BestelRegelTest
    {
        [TestMethod]
        [DataRow(1, "25.25", "25.25")]
        [DataRow(2, "25.25", "50.50")]
        [DataRow(4, "2.25", "9.00")]
        [DataRow(4, "2.99", "11.96")]
        public void RegelPrijs_IsCalculatedProperly(int aantal, string stukprijsStr, string resultStr)
        {
            // Arrange
            var stukprijs = decimal.Parse(stukprijsStr);
            var result = decimal.Parse(resultStr);
            BestelRegel bestelRegel = new BestelRegel
            {
                Aantal = aantal,
                StukPrijs = stukprijs
            };

            // Assert
            Assert.AreEqual(result, bestelRegel.RegelPrijs);
        }

        [TestMethod]
        [DataRow(1, 25.25, 30.55)]
        [DataRow(2, 25.25, 61.10)]
        [DataRow(4, 2.25, 10.88)]
        [DataRow(4, 2.99, 14.48)]
        public void RegelPrijsInclusiefBtw_IsCalculatedProperly(int aantal, double stukprijsD, double resultD)
        {
            // Arrange
            var stukprijs = (decimal) stukprijsD;
            var result = (decimal) resultD;
            BestelRegel bestelRegel = new BestelRegel
            {
                Aantal = aantal,
                StukPrijs = stukprijs
            };

            // Assert
            Assert.AreEqual(result, bestelRegel.RegelPrijsInclusiefBtw);
        }
    }
}
