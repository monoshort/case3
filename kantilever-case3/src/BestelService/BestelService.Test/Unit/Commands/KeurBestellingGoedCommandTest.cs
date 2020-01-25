using BestelService.Commands;
using BestelService.Constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BestelService.Test.Unit.Commands
{
    [TestClass]
    public class KeurBestellingGoedCommandTest
    {
        [TestMethod]
        public void Constructor_QueueIsProperlySet()
        {
            // Act
            KeurBestellingGoedCommand command = new KeurBestellingGoedCommand();

            // Assert
            Assert.AreEqual(QueueNames.KeurBestellingGoed, command.DestinationQueue);
        }
    }
}
