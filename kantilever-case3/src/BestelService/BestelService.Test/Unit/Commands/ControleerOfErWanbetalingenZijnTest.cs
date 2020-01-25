using BestelService.Commands;
using BestelService.Constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BestelService.Test.Unit.Commands
{
    [TestClass]
    public class ControleerOfErWanbetalingenZijnTest
    {
        [TestMethod]
        public void Constructor_SetsQueueNameProperly()
        {
            // Act
            ControleerOfErWanbetalingenZijnCommand command = new ControleerOfErWanbetalingenZijnCommand();

            // Assert
            Assert.AreEqual(QueueNames.ControleerOfErWanbetalingenZijn, command.DestinationQueue);
        }
    }
}
