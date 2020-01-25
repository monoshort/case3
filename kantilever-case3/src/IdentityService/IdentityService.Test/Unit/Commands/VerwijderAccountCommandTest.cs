using IdentityService.Commands;
using IdentityService.Constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IdentityService.Test.Unit.Commands
{
    [TestClass]
    public class VerwijderAccountCommandTest
    {
        [TestMethod]
        public void Constructor_SetsQueueNameProperly()
        {
            // Act
            var command = new VerwijderAccountCommand();

            // Assert
            Assert.AreEqual(QueueNames.VerwijderAccount, command.DestinationQueue);
        }
    }
}