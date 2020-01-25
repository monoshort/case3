using BackOfficeFrontendService.Commands;
using BackOfficeFrontendService.Constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackOfficeFrontendService.Test.Unit.Commands
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
