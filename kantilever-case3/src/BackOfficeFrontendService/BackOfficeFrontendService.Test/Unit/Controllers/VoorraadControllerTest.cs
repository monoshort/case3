using System.Collections.Generic;
using System.Linq;
using BackOfficeFrontendService.Agents.Abstractions;
using BackOfficeFrontendService.Controllers;
using BackOfficeFrontendService.Models;
using BackOfficeFrontendService.Repositories.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BackOfficeFrontendService.Test.Unit.Controllers
{
    [TestClass]
    public class VoorraadControllerTest
    {
        [TestMethod]
        public void BijBestelOverzicht_ReturnsViewResult()
        {
            // Arrange
            Mock<IVoorraadRepository> voorraadRepositoryMock = new Mock<IVoorraadRepository>();
            Mock<IVoorraadAgent> voorraadAgentMock = new Mock<IVoorraadAgent>();
            VoorraadController voorraadController = new VoorraadController(voorraadRepositoryMock.Object, voorraadAgentMock.Object);

            // Act
            IActionResult result = voorraadController.BijbestelOverzicht();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(5)]
        [DataRow(40)]
        public void BijBestelOverzicht_ReturnsViewResultWithData(int amount)
        {
            // Arrange
            Mock<IVoorraadRepository> voorraadRepositoryMock = new Mock<IVoorraadRepository>();
            Mock<IVoorraadAgent> voorraadAgentMock = new Mock<IVoorraadAgent>();

            IEnumerable<VoorraadMagazijn> data = Enumerable.Repeat(new VoorraadMagazijn(), amount);

            voorraadRepositoryMock.Setup(e => e.GetArtikelenNietOpVoorraad())
                .Returns(data);

            VoorraadController voorraadController = new VoorraadController(voorraadRepositoryMock.Object, voorraadAgentMock.Object);

            // Act
            IActionResult result = voorraadController.BijbestelOverzicht();

            // Assert
            IEnumerable<VoorraadMagazijn> model = (result as ViewResult).Model as IEnumerable<VoorraadMagazijn>;
            Assert.AreEqual(amount, model.Count());
        }

        [TestMethod]
        [DataRow(10)]
        [DataRow(53)]
        public void BestelBij_CallsGetByArtikelNummerOnRepository(long artikelNummer)
        {
            // Arrange
            Mock<IVoorraadRepository> voorraadRepositoryMock = new Mock<IVoorraadRepository>();
            Mock<IVoorraadAgent> voorraadAgentMock = new Mock<IVoorraadAgent>();
            VoorraadController voorraadController = new VoorraadController(voorraadRepositoryMock.Object, voorraadAgentMock.Object);

            // Act
            voorraadController.BijBesteld(artikelNummer).Wait();

            // Assert
            voorraadRepositoryMock.Verify(e => e.GetByArtikelNummer(artikelNummer));
        }

        [TestMethod]
        [DataRow(10)]
        [DataRow(53)]
        public void BestelBij_Returns404OnNotFound(long artikelNummer)
        {
            // Arrange
            Mock<IVoorraadRepository> voorraadRepositoryMock = new Mock<IVoorraadRepository>();
            Mock<IVoorraadAgent> voorraadAgentMock = new Mock<IVoorraadAgent>();
            VoorraadController voorraadController = new VoorraadController(voorraadRepositoryMock.Object, voorraadAgentMock.Object);

            voorraadRepositoryMock.Setup(e => e.GetByArtikelNummer(artikelNummer))
                .Returns((VoorraadMagazijn) null);

            // Act
            IActionResult actionResult = voorraadController.BijBesteld(artikelNummer).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod]
        [DataRow(10, 10)]
        [DataRow(53, 23)]
        public void BestelBij_CallsVoorraadBesteldAsyncOnAgent(long artikelNummer, int aantal)
        {
            // Arrange
            Mock<IVoorraadRepository> voorraadRepositoryMock = new Mock<IVoorraadRepository>();
            Mock<IVoorraadAgent> voorraadAgentMock = new Mock<IVoorraadAgent>();
            VoorraadController voorraadController = new VoorraadController(voorraadRepositoryMock.Object, voorraadAgentMock.Object);

            VoorraadMagazijn voorraadMagazijn = new VoorraadMagazijn
            {
                ArtikelNummer = artikelNummer,
                Voorraad = 0,
                BestelRegels = new List<BestelRegel>
                {
                    new BestelRegel { Aantal = aantal, Bestelling = new Bestelling() }
                }
            };

            voorraadRepositoryMock.Setup(e => e.GetByArtikelNummer(artikelNummer))
                .Returns(voorraadMagazijn);

            // Act
            voorraadController.BijBesteld(artikelNummer).Wait();

            // Assert
            voorraadAgentMock.Verify(e =>
                e.ThrowVoorraadBesteldEventAsync(artikelNummer, aantal));
        }

        [TestMethod]
        [DataRow(10, 10)]
        [DataRow(53, 23)]
        public void BestelBij_ReturnsRedirectToActionBijBestelOverzicht(long artikelNummer, int aantal)
        {
            // Arrange
            Mock<IVoorraadRepository> voorraadRepositoryMock = new Mock<IVoorraadRepository>();
            Mock<IVoorraadAgent> voorraadAgentMock = new Mock<IVoorraadAgent>();
            VoorraadController voorraadController = new VoorraadController(voorraadRepositoryMock.Object, voorraadAgentMock.Object);

            VoorraadMagazijn voorraadMagazijn = new VoorraadMagazijn
            {
                ArtikelNummer = artikelNummer,
                Voorraad = 0,
                BestelRegels = new List<BestelRegel>
                {
                    new BestelRegel { Aantal = aantal, Bestelling = new Bestelling()}
                }
            };

            voorraadRepositoryMock.Setup(e => e.GetByArtikelNummer(artikelNummer))
                .Returns(voorraadMagazijn);

            voorraadAgentMock.Setup(e => e.ThrowVoorraadBesteldEventAsync(artikelNummer, aantal));

            // Act
            IActionResult result = voorraadController.BijBesteld(artikelNummer).Result;

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));

            RedirectToActionResult redirectResult = result as RedirectToActionResult;
            Assert.AreEqual(nameof(VoorraadController.BijbestelOverzicht), redirectResult?.ActionName);
        }
    }
}
