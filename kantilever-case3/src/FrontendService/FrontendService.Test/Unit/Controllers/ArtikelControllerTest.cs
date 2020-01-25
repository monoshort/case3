using System.Collections.Generic;
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
    public class ArtikelControllerTest
    {
        [TestMethod]
        public void Get_ShouldReturnJsonResult()
        {
            // Arrange
            var articleRepositoryMock = new Mock<IArtikelRepository>();
            var bestellingAgentMock = new Mock<IBestellingAgent>();
            articleRepositoryMock.Setup(a => a.GetAll()).Returns(new List<Artikel>());
            var target = new ArtikelController(articleRepositoryMock.Object);

            // Act
            IActionResult result = target.Get();

            // Assert
            Assert.IsInstanceOfType(result, typeof(JsonResult));
        }

        [TestMethod]
        public void Get_ShouldReturnJsonListOfAllArtikelen()
        {
            // Arrange
            var articleRepositoryMock = new Mock<IArtikelRepository>();
            var bestellingAgentMock = new Mock<IBestellingAgent>();
            var artikelList = new List<Artikel>
            {
                new Artikel {Id = 1},
                new Artikel {Id = 2},
                new Artikel {Id = 3},
                new Artikel {Id = 4},
            };
            var artikelViewmodelList = new List<ArtikelViewModel>
            {
                new ArtikelViewModel {Id = 1},
                new ArtikelViewModel {Id = 2},
                new ArtikelViewModel {Id = 3},
                new ArtikelViewModel {Id = 4},
            };

            articleRepositoryMock.Setup(a => a.GetAll()).Returns(artikelList);
            var target = new ArtikelController(articleRepositoryMock.Object);

            // Act
            IActionResult result = target.Get();

            // Assert
            Assert.IsInstanceOfType(result, typeof(JsonResult));
            var data = (result as JsonResult).Value as List<ArtikelViewModel>;

            foreach (ArtikelViewModel item in artikelViewmodelList)
            {
                Assert.IsTrue(data.Any(a => a.Id == item.Id));
            }
        }

        [TestMethod]
        [DataRow(41357861L)]
        [DataRow(24124214L)]
        [DataRow(83592355L)]
        [DataRow(37498237L)]
        [DataRow(93535729L)]
        public void Get_IndividualArtikel_ShouldReturnJson(long artikelId)
        {
            // Arrange
            var articleRepositoryMock = new Mock<IArtikelRepository>();
            var bestellingAgentMock = new Mock<IBestellingAgent>();
            articleRepositoryMock.Setup(a => a.GetById(artikelId)).Returns(new Artikel { Id = artikelId, Beschrijving = "", Naam = "naam", Prijs = 123m});
            var target = new ArtikelController(articleRepositoryMock.Object);

            // Act
            IActionResult result = target.Get(artikelId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(JsonResult));
        }

        [TestMethod]
        [DataRow(41357861L)]
        [DataRow(24124214L)]
        [DataRow(83592355L)]
        [DataRow(37498237L)]
        [DataRow(93535729L)]
        public void Get_IndividualArtikel_ShouldReturnJsonResultOfArtikelDetailViewModel(long artikelId)
        {
            // Arrange
            var articleRepositoryMock = new Mock<IArtikelRepository>();
            articleRepositoryMock.Setup(a => a.GetById(artikelId)).Returns(new Artikel { Id = artikelId, Beschrijving = "", Naam = "naam", Prijs = 123m });
            var target = new ArtikelController(articleRepositoryMock.Object);

            // Act
            IActionResult result = target.Get(artikelId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(JsonResult));
            Assert.IsInstanceOfType((result as JsonResult).Value, typeof(ArtikelDetailViewModel));
        }

        [TestMethod]
        [DataRow(41357861L)]
        [DataRow(24124214L)]
        [DataRow(83592355L)]
        [DataRow(37498237L)]
        [DataRow(93535729L)]
        public void Get_IndividualArtikel_ShouldCallRepositoryGetByIdMethodWithCorrectId(long artikelId)
        {
            // Arrange
            var articleRepositoryMock = new Mock<IArtikelRepository>();
            articleRepositoryMock.Setup(a => a.GetById(artikelId)).Returns(new Artikel { Id = artikelId, Beschrijving = "", Naam = "naam", Prijs = 123m }).Verifiable();
            var target = new ArtikelController(articleRepositoryMock.Object);

            // Act
            IActionResult result = target.Get(artikelId);

            // Assert
            articleRepositoryMock.Verify();
        }

        [TestMethod]
        [DataRow(41357861L)]
        [DataRow(24124214L)]
        [DataRow(83592355L)]
        [DataRow(37498237L)]
        [DataRow(93535729L)]
        public void Get_IndividualArtikel_ShouldReturn404WhenArtikelIsNotFound(long artikelId)
        {
            // Arrange
            var articleRepositoryMock = new Mock<IArtikelRepository>();
            articleRepositoryMock.Setup(a => a.GetById(artikelId)).Returns((Artikel)null);
            var target = new ArtikelController(articleRepositoryMock.Object);

            // Act
            IActionResult result = target.Get(artikelId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}
