using FrontendService.Commands;
using FrontendService.Constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FrontendService.Test.Unit.Commands
{
    [TestClass]
    public class MaakNieuweKlantAanCommandTest
    {
        [TestMethod]
        public void Constructor_SetsQueueNameProperly()
        {
            // Act
            MaakNieuweKlantAanCommand command = new MaakNieuweKlantAanCommand();

            // Assert
            Assert.AreEqual(QueueNames.MaakNieuweKlantAanCommand, command.DestinationQueue);
        }
    }
}
