using System.Linq;
using FrontendService.Agents.Abstractions;
using FrontendService.Controllers;
using FrontendService.Models;
using FrontendService.Repositories.Abstractions;
using FrontendService.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FrontendService.Test.Unit.Controllers
{
    [TestClass]
    public class BestellingControllerTest
    {
        [TestMethod]
        public void Post_ShouldReturnOkOnSuccess()
        {
            // Arrange
            var articleRepositoryMock = new Mock<IArtikelRepository>();
            var bestellingAgentMock = new Mock<IBestellingAgent>();
            var klantRepositoryMock = new Mock<IKlantRepository>();

            var controller = new BestellingController(articleRepositoryMock.Object, bestellingAgentMock.Object, klantRepositoryMock.Object);

            BestellingViewModel bestellingViewModel = new BestellingViewModel
            {
                Klant = new KlantViewModel
                {
                    Factuuradres = new Adres()
                },
                Winkelwagen = new WinkelwagenViewModel
                {
                    Artikelen = new WinkelwagenRijViewModel[0]
                },
                AfleverAdres = new Adres()
            };

            // Act
            IActionResult result = controller.Post(bestellingViewModel);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            BestellingResult objectResult = (result as OkObjectResult).Value as BestellingResult;
            Assert.AreEqual("Bestelling is successvol geplaatst", objectResult.Message);
        }

        [TestMethod]
        [DataRow(10)]
        [DataRow(467)]
        public void Post_ShouldReturnBadRequestOnNonExistingArtikelId(int artikelId)
        {
            // Arrange
            var articleRepositoryMock = new Mock<IArtikelRepository>();
            var bestellingAgentMock = new Mock<IBestellingAgent>();
            var klantRepositoryMock = new Mock<IKlantRepository>();

            var controller = new BestellingController(articleRepositoryMock.Object, bestellingAgentMock.Object, klantRepositoryMock.Object);

            BestellingViewModel bestellingViewModel = new BestellingViewModel
            {
                Klant = new KlantViewModel
                {
                    Factuuradres = new Adres()
                },
                Winkelwagen = new WinkelwagenViewModel
                {
                    Artikelen = new []
                    {
                        new WinkelwagenRijViewModel { Artikel = new ArtikelViewModel { Id = artikelId } }
                    }
                },
                AfleverAdres = new Adres()
            };

            // Act
            IActionResult result = controller.Post(bestellingViewModel);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            BestellingResult objectResult = (result as BadRequestObjectResult).Value as BestellingResult;

            Assert.AreEqual($"Artikel met id {artikelId} bestaat niet.", objectResult.Message);

        }

        [TestMethod]
        [DataRow(10, "Jan Peter", "Bergen op Zoom")]
        [DataRow(467, "Bertha Maan", "Amsterdam")]
        public void Post_ShouldCallBestelOnAgent(int artikelId, string klantNaam, string klantWoonplaats)
        {
            // Arrange
            var articleRepositoryMock = new Mock<IArtikelRepository>();
            var bestellingAgentMock = new Mock<IBestellingAgent>();
            var klantRepositoryMock = new Mock<IKlantRepository>();

            articleRepositoryMock.Setup(e => e.GetById(artikelId))
                .Returns(new Artikel());

            var controller = new BestellingController(articleRepositoryMock.Object, bestellingAgentMock.Object, klantRepositoryMock.Object);

            KlantViewModel klant = new KlantViewModel
            {
                Factuuradres = new Adres
                {
                    Woonplaats = klantWoonplaats
                },
                Naam = klantNaam
            };

            BestellingViewModel bestellingViewModel = new BestellingViewModel
            {
                Klant = klant,
                Winkelwagen = new WinkelwagenViewModel
                {
                    Artikelen = new []
                    {
                        new WinkelwagenRijViewModel { Artikel = new ArtikelViewModel { Id = artikelId } }
                    }
                },
                AfleverAdres = new Adres{
                    Woonplaats = klantWoonplaats
                },
            };

            // Act
            controller.Post(bestellingViewModel);

            // Assert
            bestellingAgentMock.Verify(e => e.Bestel(It.IsAny<Bestelling>()));
        }

        [TestMethod]
        public void Post_ShouldCallBestelOnAgentWithExpectedData()
        {
            // Arrange
            var articleRepositoryMock = new Mock<IArtikelRepository>();
            var bestellingAgentMock = new Mock<IBestellingAgent>();
            var klantRepositoryMock = new Mock<IKlantRepository>();

            Klant klant1 = new Klant
            {
                Id = 1,
                Factuuradres = new Adres
                {
                    Woonplaats = "Utrecht",
                    Postcode = "2452 ED",
                    StraatnaamHuisnummer = "Generaal spoorstraat 23"
                },
                Naam = "Jan van Weden",
                Telefoonnummer = "068294729"
            };

            Artikel artikel1 = new Artikel
            {
                Naam = "Fietsband",
                Leveranciercode = "FB-23",
                AfbeeldingUrl = "test.jpg",
                Id = 24
            };

            Artikel artikel2 = new Artikel
            {
                Naam = "Bel",
                Leveranciercode = "ED-23",
                AfbeeldingUrl = "test2.jpg",
                Id = 592
            };

            articleRepositoryMock.Setup(e => e.GetById(24)).Returns(artikel1);
            articleRepositoryMock.Setup(e => e.GetById(592)).Returns(artikel2);
            klantRepositoryMock.Setup(e => e.GetById(1)).Returns(klant1);

            var controller = new BestellingController(articleRepositoryMock.Object, bestellingAgentMock.Object, klantRepositoryMock.Object);

            BestellingViewModel bestellingViewModel = new BestellingViewModel
            {
                Klant = new KlantViewModel
                {
                    Id = 1
                },
                Winkelwagen = new WinkelwagenViewModel
                {
                    Artikelen = new []
                    {
                        new WinkelwagenRijViewModel { Artikel = new ArtikelViewModel { Id = artikel1.Id }, Aantal = 23 },
                        new WinkelwagenRijViewModel { Artikel = new ArtikelViewModel { Id = artikel2.Id }, Aantal = 52 }
                    }
                },
                AfleverAdres = new Adres
                {
                    Woonplaats = "Utrecht",
                    Postcode = "2452 ED",
                    StraatnaamHuisnummer = "Generaal spoorstraat 23"
                }
            };

            Bestelling resultBestelling = null;
            bestellingAgentMock.Setup(e => e.Bestel(It.IsAny<Bestelling>()))
                .Callback<Bestelling>(e => resultBestelling = e);

            // Act
            controller.Post(bestellingViewModel);

            // Assert
            Assert.AreEqual(klant1.Factuuradres, resultBestelling.Klant.Factuuradres);
            Assert.AreEqual(klant1.Naam, resultBestelling.Klant.Naam);
            Assert.AreEqual(klant1.Telefoonnummer, resultBestelling.Klant.Telefoonnummer);
            Assert.AreEqual(klant1.Factuuradres, resultBestelling.Klant.Factuuradres);
            Assert.AreEqual(bestellingViewModel.AfleverAdres.StraatnaamHuisnummer, resultBestelling.AfleverAdres.StraatnaamHuisnummer);
            Assert.AreEqual(bestellingViewModel.AfleverAdres.Postcode, resultBestelling.AfleverAdres.Postcode);
            Assert.AreEqual(bestellingViewModel.AfleverAdres.Woonplaats, resultBestelling.AfleverAdres.Woonplaats);

            BestelRegel bestelRegel1 = resultBestelling.BestelRegels.Single(e => e.Naam == "Fietsband");
            Assert.AreEqual(23, bestelRegel1.Aantal);
            Assert.AreEqual("FB-23", bestelRegel1.Leverancierscode);
            Assert.AreEqual("test.jpg", bestelRegel1.AfbeeldingUrl);

            BestelRegel bestelRegel2 = resultBestelling.BestelRegels.Single(e => e.Naam == "Bel");
            Assert.AreEqual(52, bestelRegel2.Aantal);
            Assert.AreEqual("ED-23", bestelRegel2.Leverancierscode);
            Assert.AreEqual("test2.jpg", bestelRegel2.AfbeeldingUrl);
        }
    }
}
