using BackOfficeFrontendService.Commands;
using BackOfficeFrontendService.Constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackOfficeFrontendService.Test.Unit.Commands
{
    [TestClass]
    public class KeurBestellingAfCommandTest
    {
        [TestMethod]
        public void Constructor_QueueIsProperlySet()
        {
            // Act
            KeurBestellingAfCommand command = new KeurBestellingAfCommand();

            // Assert
            Assert.AreEqual(QueueNames.KeurBestellingAf, command.DestinationQueue);
        }
    }
}
