using BestelService.Core.Exceptions;
using BestelService.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BestelService.Core.Test.Unit.Exceptions
{
    [TestClass]
    public class BestellingKanNietKlaargemeldWordenExceptionTest
    {
        [TestMethod]
        public void Constructor_SetsBestellingProperly()
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                BestellingNummer = "12941"
            };

            // Act
            var exception = new BestellingKanNietKlaargemeldWordenException(bestelling, "test");

            // Assert
            Assert.AreSame(bestelling, exception.Bestelling);
        }

        [TestMethod]
        [DataRow("Hello World")]
        [DataRow("Test")]
        public void Constructor_SetsMessageProperly(string message)
        {
            // Act
            var exception = new BestellingKanNietKlaargemeldWordenException(new Bestelling(), message);

            // Assert
            Assert.AreEqual(message, exception.Message);
        }
    }
}
