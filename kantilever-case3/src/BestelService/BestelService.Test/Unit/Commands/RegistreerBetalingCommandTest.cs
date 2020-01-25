using BestelService.Commands;
using BestelService.Constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BestelService.Test.Unit.Commands
{
    [TestClass]
    public class RegistreerBetalingCommandTest
    {
        [TestMethod]
        public void Constructor_SetsDestinationQueueProperly()
        {
            // Act
            RegistreerBetalingCommand command = new RegistreerBetalingCommand();

            // Assert
            Assert.AreEqual(QueueNames.RegistreerBetaling, command.DestinationQueue);
        }
    }
}
