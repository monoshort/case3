using FrontendService.Commands;
using FrontendService.Constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FrontendService.Test.Unit.Commands
{
    [TestClass]
    public class MaakNieuweBestellingAanCommandTest
    {
        [TestMethod]
        public void Constructor_DestinationQueueIsProperlySet()
        {
            // Act
            MaakNieuweBestellingAanCommand command = new MaakNieuweBestellingAanCommand();

            // Assert
            Assert.AreEqual(QueueNames.MaakNieuweBestellingAanCommand, command.DestinationQueue);
        }
    }
}
