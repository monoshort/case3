using BestelService.Core.Models;
using BestelService.Core.Repositories;
using BestelService.Events;
using BestelService.Listeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BestelService.Test.Unit.Listeners
{
    [TestClass]
    public class KlantEventListenerTest
    {
        [TestMethod]
        [DataRow("Jan-Frederik")]
        [DataRow("Peter")]
        public void HandleNieuweKlantAangemaakt_CallsAddOnRepository(string naam)
        {
            // Arrange
            Mock<IKlantRepository> repoMock = new Mock<IKlantRepository>();
            KlantEventListener listener = new KlantEventListener(repoMock.Object);

            NieuweKlantAangemaaktEvent @event = new NieuweKlantAangemaaktEvent
            {
                Klant = new Klant { Naam = naam }
            };

            // Act
            listener.HandleNieuweKlantAangemaakt(@event);

            // Assert
            repoMock.Verify(e => e.Add(@event.Klant));
        }
    }
}
