using BestelService.Commands;
using BestelService.Constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BestelService.Test.Unit.Commands
{
    [TestClass]
    public class PrintFactuurCommandTest
    {
        [TestMethod]
        public void Constructor_SetsDestinationQueueProperly()
        {
            // Act
            PrintFactuurCommand command = new PrintFactuurCommand();

            // Assert
            Assert.AreEqual(QueueNames.PrintFactuur, command.DestinationQueue);
        }
    }
}
