using BackOfficeFrontendService.Commands;
using BackOfficeFrontendService.Constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackOfficeFrontendService.Test.Unit.Commands
{
    [TestClass]
    public class MeldBestellingKlaarCommandTest
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
