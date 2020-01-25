using IdentityService.Commands;
using IdentityService.Constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IdentityService.Test.Unit.Commands
{
    [TestClass]
    public class MaakAccountAanCommandTest
    {
        [TestMethod]
        public void Constructor_SetsQueueNameProperly()
        {
            // Act
            MaakAccountAanCommand command = new MaakAccountAanCommand();

            // Assert
            Assert.AreEqual(QueueNames.MaakAccountAan, command.DestinationQueue);
        }
    }
}
