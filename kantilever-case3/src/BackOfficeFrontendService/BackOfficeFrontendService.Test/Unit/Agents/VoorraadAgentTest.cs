using System;
using System.Collections.Generic;
using System.Linq;
using BackOfficeFrontendService.Agents;
using BackOfficeFrontendService.Agents.Abstractions;
using BackOfficeFrontendService.Commands;
using BackOfficeFrontendService.Constants;
using BackOfficeFrontendService.Events;
using BackOfficeFrontendService.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minor.Miffy.MicroServices.Events;
using Moq;

namespace BackOfficeFrontendService.Test.Unit.Agents
{
    [TestClass]
    public class VoorraadAgentTest
    {
        [TestMethod]
        public void Constructor_ThrowsExceptionIfBaseUrlNotSet()
        {
            // Arrange
            Mock<IHttpAgent> httpAgentMock = new Mock<IHttpAgent>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();
            Environment.SetEnvironmentVariable(EnvNames.VoorraadServiceUrl, null);

            // Act
            void Act() => _ = new VoorraadAgent(httpAgentMock.Object, eventPublisherMock.Object);

            // Assert
            Exception exception = Assert.ThrowsException<Exception>(Act);
            Assert.AreEqual($"Environment variable {EnvNames.VoorraadServiceUrl} is not set!", exception.Message);
        }

        [TestMethod]
        [DataRow("https://example.com")]
        [DataRow("https://test.nl")]
        public void HaalVoorraadUitMagazijnAsync_SendsPostRequestToBaseUrlPlusEndpoint(string baseUrl)
        {
            // Arrange
            Mock<IHttpAgent> httpAgentMock = new Mock<IHttpAgent>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();
            Environment.SetEnvironmentVariable(EnvNames.VoorraadServiceUrl, baseUrl);
            VoorraadAgent voorraadAgent = new VoorraadAgent(httpAgentMock.Object, eventPublisherMock.Object);

            // Act
            voorraadAgent.HaalVoorraadUitMagazijnAsync(new HaalVoorraadUitMagazijnCommand()).Wait();

            // Assert
            httpAgentMock.Verify(e =>
                e.PostAsync<HaalVoorraadUitMagazijnCommand, string>($"{baseUrl}/{Endpoints.HaalVoorraadUitMagazijn}", It.IsAny<HaalVoorraadUitMagazijnCommand>()));
        }

        [TestMethod]
        [DataRow(1392, 10)]
        [DataRow(4392, 42)]
        public void HaalVoorraadUitMagazijnAsync_SendsPostRequestWithCommand(int artikelNummer, int aantal)
        {
            // Arrange
            Mock<IHttpAgent> httpAgentMock = new Mock<IHttpAgent>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();
            Environment.SetEnvironmentVariable(EnvNames.VoorraadServiceUrl, "http://example.com");
            VoorraadAgent voorraadAgent = new VoorraadAgent(httpAgentMock.Object, eventPublisherMock.Object);

            HaalVoorraadUitMagazijnCommand command = new HaalVoorraadUitMagazijnCommand
            {
                Aantal = aantal,
                Artikelnummer = artikelNummer
            };

            // Act
            voorraadAgent.HaalVoorraadUitMagazijnAsync(command).Wait();

            // Assert
            httpAgentMock.Verify(e => e.PostAsync<HaalVoorraadUitMagazijnCommand, string>(It.IsAny<string>(), command));
        }

        [TestMethod]
        [DataRow("https://example.com")]
        [DataRow("https://test.nl")]
        public void GetAllVoorraadAsync_SendsGetRequestToVoorraadEndpoint(string url)
        {
            // Arrange
            Mock<IHttpAgent> httpAgentMock = new Mock<IHttpAgent>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();
            Environment.SetEnvironmentVariable(EnvNames.VoorraadServiceUrl, url);
            VoorraadAgent voorraadAgent = new VoorraadAgent(httpAgentMock.Object, eventPublisherMock.Object);

            // Act
            voorraadAgent.GetAllVoorraadAsync().Wait();

            // Assert
            httpAgentMock.Verify(e => e.GetAsync<VoorraadMagazijn[]>($"{url}/{Endpoints.TotaleVoorraad}"));
        }

        [TestMethod]
        public void GetAllVoorraadAsync_SendsGetRequestToVoorraadEndpoint()
        {
            // Arrange
            Mock<IHttpAgent> httpAgentMock = new Mock<IHttpAgent>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();
            Environment.SetEnvironmentVariable(EnvNames.VoorraadServiceUrl, "http://example.com");
            VoorraadAgent voorraadAgent = new VoorraadAgent(httpAgentMock.Object, eventPublisherMock.Object);

            VoorraadMagazijn[] data = {
                new VoorraadMagazijn {ArtikelNummer = 234},
                new VoorraadMagazijn {ArtikelNummer = 643}
            };

            httpAgentMock.Setup(e => e.GetAsync<VoorraadMagazijn[]>(It.IsAny<string>()))
                .ReturnsAsync(data);

            // Act
            IEnumerable<VoorraadMagazijn> result = voorraadAgent.GetAllVoorraadAsync().Result;

            // Assert
            CollectionAssert.AreEquivalent(data, result.ToArray());
        }

        [TestMethod]
        [DataRow(23928, 20)]
        [DataRow(232140, 4)]
        public void ThrowVoorraadBesteldEventAsync_CallsPublishAsyncOnEventPublisherWithEvent(long artikel, long aantal)
        {
            // Arrange
            Mock<IHttpAgent> httpAgentMock = new Mock<IHttpAgent>();
            Mock<IEventPublisher> eventPublisherMock = new Mock<IEventPublisher>();
            Environment.SetEnvironmentVariable(EnvNames.VoorraadServiceUrl, "http://example.com");
            VoorraadAgent voorraadAgent = new VoorraadAgent(httpAgentMock.Object, eventPublisherMock.Object);

            // Act
            voorraadAgent.ThrowVoorraadBesteldEventAsync(artikel, aantal).Wait();

            // Assert
            eventPublisherMock.Verify(e =>
                e.PublishAsync(It.Is<VoorraadBesteldEvent>(ev => ev.Artikelnummer == artikel && ev.BesteldeVoorraad == aantal)));
        }
    }
}
