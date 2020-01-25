using BackOfficeFrontendService.EventListeners;
using BackOfficeFrontendService.Events;
using BackOfficeFrontendService.Models;
using BackOfficeFrontendService.Repositories.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BackOfficeFrontendService.Test.Unit.EventListeners
{
    [TestClass]
    public class KlantEventListenersTest
    {
        [TestMethod]
        [DataRow("Sint Ansfridusstraat 121", "Amersfoort", "3817 BG")]
        [DataRow("Donkerstraat 134", "Ravenswaaij", "4119 LX")]
        public void HandleKlantAangemaakt_CallsAddOnRepositoryWithKlant(string straat, string plaats, string postcode)
        {
            // Arrange
            Klant klant = new Klant
            {
                Factuuradres = new Adres
                {
                    StraatnaamHuisnummer = straat,
                    Postcode = postcode,
                    Woonplaats = plaats
                }
            };


            var klantRepositoryMock = new Mock<IKlantRepository>();
            var listener = new KlantEventListeners(klantRepositoryMock.Object);
            var @event = new NieuweKlantAangemaaktEvent { Klant = klant };

            // Act
            listener.HandleNieuweKlant(@event);

            // Assert
            klantRepositoryMock.Verify(e => e.Add(klant));
        }
    }
}
