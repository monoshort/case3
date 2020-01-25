using BestelService.Commands;
using BestelService.Constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BestelService.Test.Unit.Commands
{
    [TestClass]
    public class MaakNieuweBestellingAanCommandTest
    {
        [TestMethod]
        public void Constructor_SetsProperDestinationQueue()
        {
            // Act
            var command = new MaakNieuweBestellingAanCommand();

            // Assert
            Assert.AreEqual(QueueNames.MaakNieuweBestellingAan, command.DestinationQueue);
        }
    }
}
