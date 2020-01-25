using FrontendService.EventListeners;
using FrontendService.Events;
using FrontendService.Models;
using FrontendService.Repositories.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FrontendService.Test.Unit.Listeners
{
    [TestClass]
    public class KlantEventListenerTests
    {
        [TestMethod]
        public void HandleNieuweKlantAangemaakt_CallsRepositoryWithNieuweKlant()
        {
            // Arrange
            var repoMock = new Mock<IKlantRepository>();
            var target = new KlantEventListeners(repoMock.Object);
            Klant klant = new Klant()
            {
                Naam = "Jeroen",
                Telefoonnummer = "8392569214",
            };
            NieuweKlantAangemaaktEvent @event = new NieuweKlantAangemaaktEvent()
            {
                Klant = klant
            };

            // Act
            target.HandleNieuweKlantAangemaaktEvent(@event);

            // Assert
            repoMock.Verify(mock => mock.Add(It.Is<Klant>(klant => klant.Naam == "Jeroen" && klant.Telefoonnummer == "8392569214")), Times.Once);
        }
    }
}
