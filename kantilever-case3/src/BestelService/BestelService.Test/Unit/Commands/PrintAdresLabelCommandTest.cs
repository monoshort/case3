using BestelService.Commands;
using BestelService.Constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BestelService.Test.Unit.Commands
{
    [TestClass]
    public class PrintAdresLabelCommandTest
    {
        [TestMethod]
        public void Constructor_SetsDestinationQueueProperly()
        {
            // Act
            PrintAdresLabelCommand command = new PrintAdresLabelCommand();

            // Assert
            Assert.AreEqual(QueueNames.PrintAdresLabel, command.DestinationQueue);
        }
    }
}
