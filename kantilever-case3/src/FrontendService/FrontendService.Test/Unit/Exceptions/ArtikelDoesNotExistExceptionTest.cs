using FrontendService.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FrontendService.Test.Unit.Exceptions
{
    [TestClass]
    public class ArtikelDoesNotExistExceptionTest
    {
        [TestMethod]
        [DataRow(24)]
        [DataRow(228)]
        public void Constructor_SetsArtikelProperly(int artikelId)
        {
            // Act
            ArtikelDoesNotExistException exception = new ArtikelDoesNotExistException(artikelId);

            // Assert
            Assert.AreEqual(artikelId, exception.ArtikelId);
        }

        [TestMethod]
        [DataRow(24)]
        [DataRow(228)]
        public void Constructor_SetsMessageProperly(long artikelId)
        {
            // Act
            ArtikelDoesNotExistException exception = new ArtikelDoesNotExistException(artikelId);

            // Assert
            Assert.AreEqual($"Artikel {artikelId} does not exist", exception.Message);
        }
    }
}
