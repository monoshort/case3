using BackOfficeFrontendService.Commands;
using BackOfficeFrontendService.Constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackOfficeFrontendService.Test.Unit.Commands
{
    [TestClass]
    public class ControleerOfErWanbetalingenZijnCommandTest
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
