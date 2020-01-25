using BestelService.Commands;
using BestelService.Constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BestelService.Test.Unit.Commands
{
    [TestClass]
    public class PakBestelRegelInCommandTest
    {
        [TestMethod]
        public void Constructor_SetsDestinationQueueProperly()
        {
            // Act
            PakBestelRegelInCommand command = new PakBestelRegelInCommand();

            // Assert
            Assert.AreEqual(QueueNames.PakBestelRegelIn, command.DestinationQueue);
        }
    }
}
