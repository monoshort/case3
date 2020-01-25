using BackOfficeFrontendService.Commands;
using BackOfficeFrontendService.Constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackOfficeFrontendService.Test.Unit.Commands
{
    [TestClass]
    public class PakBestelRegelInCommandTest
    {
        [TestMethod]
        public void Constructor_SetsQueueNameProperly()
        {
            // Act
            PakBestelRegelInCommand command = new PakBestelRegelInCommand();

            // Assert
            Assert.AreEqual(QueueNames.PakBestelRegelIn, command.DestinationQueue);
        }
    }
}
