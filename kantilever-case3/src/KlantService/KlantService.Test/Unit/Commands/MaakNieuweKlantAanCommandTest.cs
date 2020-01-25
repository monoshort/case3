using KlantService.Commands;
using KlantService.Constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KlantService.Test.Unit.Commands
{
    [TestClass]
    public class MaakNieuweKlantAanCommandTest
    {
        [TestMethod]
        public void Constructor_ProperlySetsQueueName()
        {
            // Act
            MaakNieuweKlantAanCommand command = new MaakNieuweKlantAanCommand();

            // Assert
            Assert.AreEqual(QueueNames.MaakNieuweKlantAan, command.DestinationQueue);
        }
    }
}
