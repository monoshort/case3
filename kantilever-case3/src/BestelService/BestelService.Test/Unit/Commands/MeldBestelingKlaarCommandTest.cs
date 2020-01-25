using BestelService.Commands;
using BestelService.Constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BestelService.Test.Unit.Commands
{
    [TestClass]
    public class MeldBestelingKlaarCommandTest
    {
        [TestMethod]
        public void Constructor_SetsQueueNameProperly()
        {
            // Act
            MeldBestellingKlaarCommand command = new MeldBestellingKlaarCommand();

            // Assert
            Assert.AreEqual(QueueNames.MeldBestellingKlaar, command.DestinationQueue);
        }
    }
}
