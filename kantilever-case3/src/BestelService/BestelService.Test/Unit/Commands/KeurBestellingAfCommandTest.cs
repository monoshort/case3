using BestelService.Commands;
using BestelService.Constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BestelService.Test.Unit.Commands
{
    [TestClass]
    public class KeurBestellingAfCommandTest
    {
        [TestMethod]
        public void Constructor_QueueIsProperlySet()
        {
            // Act
            KeurBestellingAfCommand command = new KeurBestellingAfCommand();

            // Assert
            Assert.AreEqual(QueueNames.KeurBestellingAf, command.DestinationQueue);
        }
    }
}
