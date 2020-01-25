using System;
using System.Collections.Generic;
using System.Linq;
using BackOfficeFrontendService.Agents;
using BackOfficeFrontendService.Agents.Abstractions;
using BackOfficeFrontendService.Constants;
using BackOfficeFrontendService.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BackOfficeFrontendService.Test.Unit.Agents
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
        [DataRow(20)]
        [DataRow(592)]
        public void GetAlleArtikelenAsync_ReturnsListOfArtikelen(int artikelNummer)
        {
            // Arrange
            Environment.SetEnvironmentVariable(EnvNames.CatalogusServiceUrl, "http://localhost:2020");
            var mockAgent = new Mock<IHttpAgent>();

            Artikel[] expectedResult = { new Artikel {Artikelnummer = artikelNummer} };

            mockAgent.Setup(agent => agent.GetAsync<Artikel[]>(It.IsAny<string>()))
                .ReturnsAsync(expectedResult);

            var target = new CatalogusAgent(mockAgent.Object);

            // Act
            IEnumerable<Artikel> result = target.GetAlleArtikelenAsync().Result;

            // Assert
            Assert.AreEqual(artikelNummer, result.First().Artikelnummer);
        }
    }
}
