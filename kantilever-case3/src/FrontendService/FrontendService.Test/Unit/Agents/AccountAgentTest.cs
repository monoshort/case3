using FrontendService.Agents;
using FrontendService.Agents.Abstractions;
using FrontendService.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minor.Miffy.MicroServices.Commands;
using Moq;

namespace FrontendService.Test.Unit.Agents
{
    [TestClass]
    public class AccountAgentTest
    {
        [TestMethod]
        [DataRow("jan", "test123")]
        [DataRow("peter", "123")]
        public void MaakAccountAanAsync_CallsPublishAsyncOnPublisherWithCommand(string username, string password)
        {
            // Arrange
            Mock<ICommandPublisher> publisherMock = new Mock<ICommandPublisher>();
            IAccountAgent accountAgent = new AccountAgent(publisherMock.Object);

            // Act
            accountAgent.MaakAccountAanAsync(username, password).Wait();

            // Assert
            publisherMock.Verify(e =>
                e.PublishAsync<MaakAccountAanCommand>(
                    It.Is<MaakAccountAanCommand>(c => c.Username == username && c.Password == password)));
        }

        [TestMethod]
        [DataRow("testUser")]
        [DataRow("JanDan")]
        public void VerwijderAccountAsync_CallsPublishAsyncOnPublisherWithCommand(string username)
        {
            // Arrange
            Mock<ICommandPublisher> publisherMock = new Mock<ICommandPublisher>();
            IAccountAgent accountAgent = new AccountAgent(publisherMock.Object);

            // Act
            accountAgent.VerwijderAccountAsync(username).Wait();

            // Assert
            publisherMock.Verify(e =>
                e.PublishAsync<VerwijderAccountCommand>(
                    It.Is<VerwijderAccountCommand>(c => c.Username == username)));
        }
    }
}
