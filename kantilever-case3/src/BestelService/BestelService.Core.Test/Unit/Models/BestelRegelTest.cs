using BestelService.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BestelService.Core.Test.Unit.Models
{
    [TestClass]
    public class BestelRegelTest
    {

        [TestMethod]
        public void Constructor_IngepaktIsInitiallyFalse()
        {
            // Act
            BestelRegel bestelRegel = new BestelRegel();

            // Assert
            Assert.AreEqual(false, bestelRegel.Ingepakt);
        }

        [TestMethod]
        public void PakIn_SetsIngepaktToTrue()
        {
            // Arrange
            BestelRegel bestelRegel = new BestelRegel();

            // Act
            bestelRegel.PakIn();

            // Assert
            Assert.AreEqual(true, bestelRegel.Ingepakt);
        }
    }
}
