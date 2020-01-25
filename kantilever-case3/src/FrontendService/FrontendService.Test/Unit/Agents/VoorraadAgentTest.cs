using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrontendService.Agents;
using FrontendService.Agents.Abstractions;
using FrontendService.Constants;
using FrontendService.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FrontendService.Test.Unit.Agents
{
    [TestClass]
    public class VoorraadAgentTest
    {
        [TestMethod]
        public void Constructor_ThrowsExceptionIfEnvVarIsntSet()
        {
            // Arrange
            Environment.SetEnvironmentVariable(EnvNames.VoorraadServiceUrl, null);
            var mockAgent = new Mock<IHttpAgent>();

            // Act
            Action action = () => _ = new VoorraadAgent(mockAgent.Object);;

            // Assert
            var exception = Assert.ThrowsException<Exception>(action);
            Assert.AreEqual($"Environment variable {EnvNames.VoorraadServiceUrl} not set", exception.Message);
        }

        [TestMethod]
        [DataRow("http://localhost:2020")]
        [DataRow("http://localhost:2060")]
        public void GetAllVoorraadAsync_RetrievesBaseUrlFromEnvironment(string baseUrl)
        {
            // Arrange
            Environment.SetEnvironmentVariable(EnvNames.VoorraadServiceUrl, baseUrl);

            var mockAgent = new Mock<IHttpAgent>();
            var target = new VoorraadAgent(mockAgent.Object);

            // Act
            target.GetAllVoorraadAsync();

            // Assert
            mockAgent.Verify(agent => agent.GetAsync<IEnumerable<VoorraadMagazijn>>($"{baseUrl}/{Endpoints.TotaleVoorraad}"));
        }
        [TestMethod]
        [DataRow(6)]
        [DataRow(2)]
        public void GetAllVoorraadAsync_ReturnsListOfVoorraadMagazijnen(int voorraad)
        {
            // Arrange
            Environment.SetEnvironmentVariable(EnvNames.VoorraadServiceUrl, "http://localhost:2020");
            var mockAgent = new Mock<IHttpAgent>();

            IEnumerable<VoorraadMagazijn> expectedData = new []{ new VoorraadMagazijn { Voorraad = voorraad } };

            mockAgent.Setup(agent => agent.GetAsync<IEnumerable<VoorraadMagazijn>>(It.IsAny<string>()))
                .Returns(Task.FromResult(expectedData));

            var target = new VoorraadAgent(mockAgent.Object);

            // Act
            Task<IEnumerable<VoorraadMagazijn>> result = target.GetAllVoorraadAsync();

            // Assert
            Assert.AreEqual(voorraad, result.Result.First().Voorraad);
        }
    }
}
