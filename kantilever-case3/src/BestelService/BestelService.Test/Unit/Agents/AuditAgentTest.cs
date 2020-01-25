using System;
using BestelService.Agents;
using BestelService.Agents.Abstractions;
using BestelService.Commands;
using BestelService.Constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BestelService.Test.Unit.Agents
{
    [TestClass]
    public class AuditAgentTest
    {
        [TestMethod]
        public void Constructor_ThrowsExceptionIfEnvVarIsNotSet()
        {
            // Arrange
            Environment.SetEnvironmentVariable(EnvNames.AuditLoggerUrl, null);
            var mockAgent = new Mock<IHttpAgent>();

            // Act
            Action action = () => _ = new AuditAgent(mockAgent.Object);

            // Assert
            var exception = Assert.ThrowsException<Exception>(action);
            Assert.AreEqual($"Environment variable {EnvNames.AuditLoggerUrl} not set", exception.Message);
        }

        [TestMethod]
        [DataRow("http://example.com")]
        [DataRow("http://test.com")]
        public void ReplayEventsAsync_CallsPostAsyncWithProperUrl(string url)
        {
            // Arrange
            Mock<IHttpAgent> httpAgentMock = new Mock<IHttpAgent>();
            Environment.SetEnvironmentVariable(EnvNames.AuditLoggerUrl, url);
            AuditAgent auditAgent = new AuditAgent(httpAgentMock.Object);

            ReplayEventsCommand command = new ReplayEventsCommand();

            // Act
            auditAgent.ReplayEventsAsync(command).Wait();

            // Assert
            httpAgentMock.Verify(e => e.PostAsync<ReplayEventsCommand, string>($"{url}/{Endpoints.ReplayEvents}", command));
        }

        [TestMethod]
        [DataRow("3535")]
        [DataRow("10000")]
        public void ReplayEventsAsync_ReturnsStringProperly(string response)
        {
            // Arrange
            Mock<IHttpAgent> httpAgentMock = new Mock<IHttpAgent>();
            Environment.SetEnvironmentVariable(EnvNames.AuditLoggerUrl, "http://example.com");
            AuditAgent auditAgent = new AuditAgent(httpAgentMock.Object);

            ReplayEventsCommand command = new ReplayEventsCommand();

            httpAgentMock.Setup(e => e.PostAsync<ReplayEventsCommand, string>(It.IsAny<string>(), command))
                .ReturnsAsync(response);

            // Act
            string result = auditAgent.ReplayEventsAsync(command).Result;

            // Assert
            Assert.AreEqual(response, result);
        }
    }
}
