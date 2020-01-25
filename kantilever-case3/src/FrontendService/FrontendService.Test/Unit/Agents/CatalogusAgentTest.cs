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
    public class CatalogusAgentTest
    {
        [TestMethod]
        public void Constructor_ThrowsExceptionIfEnvVarIsNotSet()
        {
            // Arrange
            Environment.SetEnvironmentVariable(EnvNames.CatalogusServiceUrl, null);
            var mockAgent = new Mock<IHttpAgent>();

            // Act
            Action action = () => _ = new CatalogusAgent(mockAgent.Object);

            // Assert
            var exception = Assert.ThrowsException<Exception>(action);
            Assert.AreEqual($"Environment variable {EnvNames.CatalogusServiceUrl} not set", exception.Message);
        }

        [TestMethod]
        [DataRow("http://localhost:2020")]
        [DataRow("http://localhost:2060")]
        public void GetAlleArtikelenAsync_RetrievesBaseUrlFromEnvironment(string baseUrl)
        {
            // Arrange
            Environment.SetEnvironmentVariable(EnvNames.CatalogusServiceUrl, baseUrl);

            var mockAgent = new Mock<IHttpAgent>();
            var target = new CatalogusAgent(mockAgent.Object);

            // Act
            target.GetAlleArtikelenAsync();

            // Assert
            mockAgent.Verify(agent => agent.GetAsync<IEnumerable<Artikel>>($"{baseUrl}/{Endpoints.TotaleCatalogus}"));
        }

        [TestMethod]
        [DataRow("testArtikel")]
        [DataRow("testArtikel2")]
        public void GetAlleArtikelenAsync_ReturnsListOfArtikelen(string artikelNaam)
        {
            // Arrange
            Environment.SetEnvironmentVariable(EnvNames.CatalogusServiceUrl, "http://localhost:2020");
            var mockAgent = new Mock<IHttpAgent>();

            IEnumerable<Artikel> expectedResult = new [] { new Artikel {Naam = artikelNaam} };

            mockAgent.Setup(agent => agent.GetAsync<IEnumerable<Artikel>>(It.IsAny<string>()))
                .Returns(Task.FromResult(expectedResult));

            var target = new CatalogusAgent(mockAgent.Object);

            // Act
            Task<IEnumerable<Artikel>> result = target.GetAlleArtikelenAsync();

            // Assert
            Assert.AreEqual(artikelNaam, result.Result.First().Naam);
        }
    }
}
