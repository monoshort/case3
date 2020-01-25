using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackOfficeFrontendService.Agents.Abstractions;
using BackOfficeFrontendService.Constants;
using BackOfficeFrontendService.Controllers;
using BackOfficeFrontendService.Exceptions;
using BackOfficeFrontendService.Models;
using BackOfficeFrontendService.Repositories.Abstractions;
using BackOfficeFrontendService.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minor.Miffy;
using Moq;

namespace BackOfficeFrontendService.Test.Unit.Controllers
{
    [TestClass]
    public class BestellingControllerTest
    {
        [TestMethod]
        public void GetNextInpakBestelling_ShouldReturnViewResult()
        {
            // Arrange
            Mock<IBestellingRepository> repositoryMock = new Mock<IBestellingRepository>();
            Mock<IBestellingAgent> bestellingAgentMock = new Mock<IBestellingAgent>();
            BestellingController target = new BestellingController(repositoryMock.Object, bestellingAgentMock.Object);

            // Act
            IActionResult result = target.GetNextInpakBestelling();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void GetNextInpakBestelling_ShouldReturnViewBestellingInpakken()
        {
            // Arrange
            Bestelling bestelling = new Bestelling();
            Mock<IBestellingRepository> repositoryMock = new Mock<IBestellingRepository>();
            Mock<IBestellingAgent> bestellingAgentMock = new Mock<IBestellingAgent>();
            repositoryMock.Setup(e => e.GetVolgendeInpakOpdracht())
                .Returns(bestelling);

            BestellingController target = new BestellingController(repositoryMock.Object, bestellingAgentMock.Object);

            // Act
            IActionResult result = target.GetNextInpakBestelling();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = result as ViewResult;
            Assert.AreEqual("BestellingInpakken", viewResult.ViewName);
        }

        [TestMethod]
        public void GetNextInpakBestelling_ShouldReturnViewNoBestellingOmInTePakken()
        {
            // Arrange
            Mock<IBestellingRepository> repositoryMock = new Mock<IBestellingRepository>();
            Mock<IBestellingAgent> bestellingAgentMock = new Mock<IBestellingAgent>();
            repositoryMock.Setup(x => x.GetVolgendeInpakOpdracht());
            BestellingController target = new BestellingController(repositoryMock.Object, bestellingAgentMock.Object);

            // Act
            IActionResult result = target.GetNextInpakBestelling();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = result as ViewResult;
            Assert.AreEqual("NoBestellingOmInTePakken", viewResult.ViewName);
        }

        [TestMethod]
        public void GetNextInpakBestelling_ShouldReturnBestellingAsModel()
        {
            // Arrange
            Bestelling expected = new Bestelling();
            Mock<IBestellingRepository> repositoryMock = new Mock<IBestellingRepository>();
            Mock<IBestellingAgent> bestellingAgentMock = new Mock<IBestellingAgent>();
            repositoryMock.Setup(e => e.GetVolgendeInpakOpdracht())
                .Returns(expected);

            BestellingController target = new BestellingController(repositoryMock.Object, bestellingAgentMock.Object);

            // Act
            IActionResult result = target.GetNextInpakBestelling();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = result as ViewResult;
            Assert.AreSame(expected, viewResult.Model);
        }

        [TestMethod]
        [DataRow(1022002)]
        [DataRow(3243242)]
        [DataRow(2343804)]
        public void GetFactuur_ReturnsViewResult(long bestellingId)
        {
            // Arrange
            Bestelling expected = new Bestelling();
            Mock<IBestellingRepository> repositoryMock = new Mock<IBestellingRepository>();
            Mock<IBestellingAgent> bestellingAgentMock = new Mock<IBestellingAgent>();
            repositoryMock.Setup(e => e.GetInpakOpdrachtMetId(It.IsAny<long>()))
                .Returns(expected);
            BestellingController target = new BestellingController(repositoryMock.Object, bestellingAgentMock.Object);

            // Act
            IActionResult result = target.GetFactuur(bestellingId).Result;

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        [DataRow(1022002)]
        [DataRow(3243242)]
        [DataRow(2343804)]
        public void GetFactuur_ReturnsViewResultWithNameFactuur(long bestellingId)
        {
            // Arrange
            Bestelling expected = new Bestelling();
            Mock<IBestellingRepository> repositoryMock = new Mock<IBestellingRepository>();
            Mock<IBestellingAgent> bestellingAgentMock = new Mock<IBestellingAgent>();
            repositoryMock.Setup(e => e.GetInpakOpdrachtMetId(It.IsAny<long>()))
                .Returns(expected);
            BestellingController target = new BestellingController(repositoryMock.Object, bestellingAgentMock.Object);

            // Act
            IActionResult result = target.GetFactuur(bestellingId).Result;

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult model = result as ViewResult;
            Assert.AreEqual("Factuur", model.ViewName);
        }

        [TestMethod]
        [DataRow(1022002)]
        [DataRow(3243242)]
        [DataRow(2343804)]
        public void GetFactuur_ReturnsBestellingAsModel(long bestellingId)
        {
            // Arrange
            Bestelling expected = new Bestelling
            {
                Id = bestellingId
            };
            Mock<IBestellingRepository> repositoryMock = new Mock<IBestellingRepository>();
            Mock<IBestellingAgent> bestellingAgentMock = new Mock<IBestellingAgent>();
            repositoryMock.Setup(e => e.GetInpakOpdrachtMetId(bestellingId))
                .Returns(expected);
            BestellingController target = new BestellingController(repositoryMock.Object, bestellingAgentMock.Object);

            // Act
            IActionResult result = target.GetFactuur(bestellingId).Result;

            // Assert
            Bestelling model = (result as ViewResult).Model as Bestelling;
            Assert.AreEqual(bestellingId, model?.Id);
        }

        [TestMethod]
        [DataRow(1022002)]
        [DataRow(3243242)]
        [DataRow(2343804)]
        public void GetFactuur_ReturnsRedirectIfBestellingDoesNotExist(long bestellingId)
        {
            // Arrange
            Mock<IBestellingRepository> repositoryMock = new Mock<IBestellingRepository>();
            Mock<IBestellingAgent> bestellingAgentMock = new Mock<IBestellingAgent>();
            BestellingController target = new BestellingController(repositoryMock.Object, bestellingAgentMock.Object);

            // Act
            IActionResult result = target.GetFactuur(bestellingId).Result;

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }

        [TestMethod]
        [DataRow(1022002)]
        [DataRow(3243242)]
        [DataRow(2343804)]
        public void GetFactuur_ReturnsRedirectToGetNextInpakBestelling(long bestellingId)
        {
            // Arrange
            Mock<IBestellingRepository> repositoryMock = new Mock<IBestellingRepository>();
            Mock<IBestellingAgent> bestellingAgentMock = new Mock<IBestellingAgent>();
            BestellingController target = new BestellingController(repositoryMock.Object, bestellingAgentMock.Object);

            // Act
            IActionResult result = target.GetFactuur(bestellingId).Result;

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirectToActionResult = result as RedirectToActionResult;
            Assert.AreEqual(nameof(BestellingController.GetNextInpakBestelling), redirectToActionResult.ActionName);
        }

        [TestMethod]
        [DataRow(1022002)]
        [DataRow(3243242)]
        [DataRow(2343804)]
        public void GetAdresLabel_ReturnsViewResult(long bestellingId)
        {
            // Arrange
            Bestelling expected = new Bestelling();
            Mock<IBestellingRepository> repositoryMock = new Mock<IBestellingRepository>();
            Mock<IBestellingAgent> bestellingAgentMock = new Mock<IBestellingAgent>();
            repositoryMock.Setup(e => e.GetInpakOpdrachtMetId(It.IsAny<long>()))
                .Returns(expected);
            BestellingController target = new BestellingController(repositoryMock.Object, bestellingAgentMock.Object);

            // Act
            IActionResult result = target.GetAdresLabel(bestellingId).Result;

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        [DataRow(1022002)]
        [DataRow(3243242)]
        [DataRow(2343804)]
        public void GetAdresLabel_ReturnsViewResultWithNameFactuur(long bestellingId)
        {
            // Arrange
            Bestelling expected = new Bestelling();
            Mock<IBestellingRepository> repositoryMock = new Mock<IBestellingRepository>();
            Mock<IBestellingAgent> bestellingAgentMock = new Mock<IBestellingAgent>();
            repositoryMock.Setup(e => e.GetInpakOpdrachtMetId(It.IsAny<long>()))
                .Returns(expected);
            BestellingController target = new BestellingController(repositoryMock.Object, bestellingAgentMock.Object);

            // Act
            IActionResult result = target.GetAdresLabel(bestellingId).Result;

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult model = result as ViewResult;
            Assert.AreEqual("Adreslabel", model.ViewName);
        }

        [TestMethod]
        [DataRow(1022002)]
        [DataRow(3243242)]
        [DataRow(2343804)]
        public void GetAdresLabel_ReturnsBestellingAsModel(long bestellingId)
        {
            // Arrange
            Bestelling expected = new Bestelling
            {
                Id = bestellingId
            };
            Mock<IBestellingRepository> repositoryMock = new Mock<IBestellingRepository>();
            Mock<IBestellingAgent> bestellingAgentMock = new Mock<IBestellingAgent>();
            repositoryMock.Setup(e => e.GetInpakOpdrachtMetId(bestellingId))
                .Returns(expected);
            BestellingController target = new BestellingController(repositoryMock.Object, bestellingAgentMock.Object);

            // Act
            IActionResult result = target.GetAdresLabel(bestellingId).Result;

            // Assert
            Bestelling model = (result as ViewResult).Model as Bestelling;
            Assert.AreEqual(bestellingId, model?.Id);
        }

        [TestMethod]
        [DataRow(1022002)]
        [DataRow(3243242)]
        [DataRow(2343804)]
        public void GetAdresLabel_ReturnsRedirectIfBestellingDoesNotExist(long bestellingId)
        {
            // Arrange
            Mock<IBestellingRepository> repositoryMock = new Mock<IBestellingRepository>();
            Mock<IBestellingAgent> bestellingAgentMock = new Mock<IBestellingAgent>();
            BestellingController target = new BestellingController(repositoryMock.Object, bestellingAgentMock.Object);

            // Act
            IActionResult result = target.GetAdresLabel(bestellingId).Result;

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }

        [TestMethod]
        [DataRow(1022002)]
        [DataRow(3243242)]
        [DataRow(2343804)]
        public void GetAdresLabel_ReturnsRedirectToGetNextInpakBestelling(long bestellingId)
        {
            // Arrange
            Mock<IBestellingRepository> repositoryMock = new Mock<IBestellingRepository>();
            Mock<IBestellingAgent> bestellingAgentMock = new Mock<IBestellingAgent>();
            BestellingController target = new BestellingController(repositoryMock.Object, bestellingAgentMock.Object);

            // Act
            IActionResult result = target.GetAdresLabel(bestellingId).Result;

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirectToActionResult = result as RedirectToActionResult;
            Assert.AreEqual(nameof(BestellingController.GetNextInpakBestelling), redirectToActionResult.ActionName);
        }

        [TestMethod]
        [DataRow(3423442)]
        [DataRow(3976593)]
        public void KeurBestellingGoed_CallsKeurBestellingGoedOnAgent(long bestellingId)
        {
            // Arrange
            Mock<IBestellingRepository> repoMock = new Mock<IBestellingRepository>();
            Mock<IBestellingAgent> bestellingAgentMock = new Mock<IBestellingAgent>();

            BestellingController target = new BestellingController(repoMock.Object, bestellingAgentMock.Object);

            // Act
            target.KeurBestellingGoed(bestellingId).Wait();

            // Arrange
            bestellingAgentMock.Verify(e => e.KeurBestellingGoedAsync(It.IsAny<long>()));
        }

        [TestMethod]
        [DataRow(3423442)]
        [DataRow(3976593)]
        public void KeurBestellingGoed_CallsKeurBestellingGoedOnAgentWithBestellingId(long bestellingId)
        {
            // Arrange
            Mock<IBestellingRepository> repoMock = new Mock<IBestellingRepository>();
            Mock<IBestellingAgent> bestellingAgentMock = new Mock<IBestellingAgent>();

            BestellingController target = new BestellingController(repoMock.Object, bestellingAgentMock.Object);

            // Act
            target.KeurBestellingGoed(bestellingId).Wait();

            // Arrange
            bestellingAgentMock.Verify(e => e.KeurBestellingGoedAsync(It.Is<long>(b => b == bestellingId)));
        }

        [TestMethod]
        [DataRow(3423442)]
        [DataRow(3976593)]
        public void KeurBestellingGoed_ReturnsRedirectToActionResult(long bestellingId)
        {
            // Arrange
            Mock<IBestellingRepository> repoMock = new Mock<IBestellingRepository>();
            Mock<IBestellingAgent> bestellingAgentMock = new Mock<IBestellingAgent>();

            BestellingController target = new BestellingController(repoMock.Object, bestellingAgentMock.Object);

            // Act
            IActionResult result = target.KeurBestellingGoed(bestellingId).Result;

            // Arrange
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }

        [TestMethod]
        [DataRow(3423442)]
        [DataRow(3976593)]
        public void KeurBestellingGoed_ReturnsRedirectToActionResultToGetBestellingenToAccept(long bestellingId)
        {
            // Arrange
            Mock<IBestellingRepository> repoMock = new Mock<IBestellingRepository>();
            Mock<IBestellingAgent> bestellingAgentMock = new Mock<IBestellingAgent>();

            BestellingController target = new BestellingController(repoMock.Object, bestellingAgentMock.Object);

            // Act
            IActionResult result = target.KeurBestellingGoed(bestellingId).Result;

            // Arrange
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirectToActionResult = result as RedirectToActionResult;
            Assert.AreEqual(nameof(BestellingController.GetBestellingenToAccept), redirectToActionResult.ActionName);
        }

        [TestMethod]
        [DataRow(3423442)]
        [DataRow(3976593)]
        public void KeurBestellingGoed_Returns404IfBestellingIdDoesntExist(long bestellingId)
        {
            // Arrange
            Mock<IBestellingRepository> repoMock = new Mock<IBestellingRepository>();
            Mock<IBestellingAgent> bestellingAgentMock = new Mock<IBestellingAgent>();
            bestellingAgentMock.Setup(agent => agent.KeurBestellingGoedAsync(It.IsAny<long>())).Throws(new FunctionalException(FunctionalExceptionMessages.BestellingNotFound));
            BestellingController target = new BestellingController(repoMock.Object, bestellingAgentMock.Object);

            // Act
            IActionResult result = target.KeurBestellingGoed(bestellingId).Result;

            // Arrange
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        [DataRow(3423442)]
        [DataRow(3976593)]
        public void KeurBestellingAf_CallsKeurBestellingAfOnAgent(long bestellingId)
        {
            // Arrange
            Mock<IBestellingRepository> repoMock = new Mock<IBestellingRepository>();
            Mock<IBestellingAgent> bestellingAgentMock = new Mock<IBestellingAgent>();

            BestellingController target = new BestellingController(repoMock.Object, bestellingAgentMock.Object);

            // Act
            target.KeurBestellingAf(bestellingId).Wait();

            // Arrange
            bestellingAgentMock.Verify(e => e.KeurBestellingAfAsync(It.IsAny<long>()));
        }

        [TestMethod]
        [DataRow(3423442)]
        [DataRow(3976593)]
        public void KeurBestellingAf_CallsKeurBestellingAfOnAgentWithBestellingId(long bestellingId)
        {
            // Arrange
            Mock<IBestellingRepository> repoMock = new Mock<IBestellingRepository>();
            Mock<IBestellingAgent> bestellingAgentMock = new Mock<IBestellingAgent>();

            BestellingController target = new BestellingController(repoMock.Object, bestellingAgentMock.Object);

            // Act
            target.KeurBestellingAf(bestellingId).Wait();

            // Arrange
            bestellingAgentMock.Verify(e => e.KeurBestellingAfAsync(It.Is<long>(b => b == bestellingId)));
        }

        [TestMethod]
        [DataRow(3423442)]
        [DataRow(3976593)]
        public void KeurBestellingAf_ReturnsRedirectToActionResult(long bestellingId)
        {
            // Arrange
            Mock<IBestellingRepository> repoMock = new Mock<IBestellingRepository>();
            Mock<IBestellingAgent> bestellingAgentMock = new Mock<IBestellingAgent>();

            BestellingController target = new BestellingController(repoMock.Object, bestellingAgentMock.Object);

            // Act
            IActionResult result = target.KeurBestellingAf(bestellingId).Result;

            // Arrange
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }

        [TestMethod]
        [DataRow(3423442)]
        [DataRow(3976593)]
        public void KeurBestellingAf_ReturnsRedirectToActionResultToGetBestellingenToAccept(long bestellingId)
        {
            // Arrange
            Mock<IBestellingRepository> repoMock = new Mock<IBestellingRepository>();
            Mock<IBestellingAgent> bestellingAgentMock = new Mock<IBestellingAgent>();

            BestellingController target = new BestellingController(repoMock.Object, bestellingAgentMock.Object);

            // Act
            IActionResult result = target.KeurBestellingAf(bestellingId).Result;

            // Arrange
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult redirectToActionResult = result as RedirectToActionResult;
            Assert.AreEqual(nameof(BestellingController.GetBestellingenToAccept), redirectToActionResult.ActionName);
        }

        [TestMethod]
        [DataRow(3423442)]
        [DataRow(3976593)]
        public void KeurBestellingAf_Returns404IfBestellingIdDoesntExist(long bestellingId)
        {
            // Arrange
            Mock<IBestellingRepository> repoMock = new Mock<IBestellingRepository>();
            Mock<IBestellingAgent> bestellingAgentMock = new Mock<IBestellingAgent>();
            bestellingAgentMock.Setup(agent => agent.KeurBestellingAfAsync(It.IsAny<long>())).Throws(new FunctionalException(FunctionalExceptionMessages.BestellingNotFound));
            BestellingController target = new BestellingController(repoMock.Object, bestellingAgentMock.Object);

            // Act
            IActionResult result = target.KeurBestellingAf(bestellingId).Result;

            // Arrange
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void GetBestellingenToAccept_ReturnsViewResult()
        {
            // Arrange
            Mock<IBestellingRepository> repoMock = new Mock<IBestellingRepository>();
            Mock<IBestellingAgent> bestellingAgentMock = new Mock<IBestellingAgent>();

            BestellingController target = new BestellingController(repoMock.Object, bestellingAgentMock.Object);

            // Act
            IActionResult result = target.GetBestellingenToAccept();

            // Arrange
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void GetBestellingenToAccept_ReturnsExpectedData()
        {
            // Arrange
            Mock<IBestellingRepository> repoMock = new Mock<IBestellingRepository>();
            Mock<IBestellingAgent> bestellingAgentMock = new Mock<IBestellingAgent>();

            List<Bestelling> bestellingen = new List<Bestelling>
            {
                new Bestelling(),
                new Bestelling(),
                new Bestelling()
            };

            repoMock.Setup(e => e.GetNietGekeurdeBestellingen())
                .Returns(bestellingen);

            BestellingController target = new BestellingController(repoMock.Object, bestellingAgentMock.Object);

            // Act
            IActionResult result = target.GetBestellingenToAccept();

            // Arrange
            IEnumerable<Bestelling> data = (result as ViewResult).Model as IEnumerable<Bestelling>;
            CollectionAssert.AreEquivalent(bestellingen, data.ToList());
        }

        [TestMethod]
        public void MeldIngepakt_CallsRepositoryGetById()
        {
            // Arrange
            Mock<IBestellingRepository> repoMock = new Mock<IBestellingRepository>();
            Mock<IBestellingAgent> bestellingAgentMock = new Mock<IBestellingAgent>();

            Bestelling bestelling = new Bestelling { KlaarGemeld = false };
            repoMock.Setup(e => e.GetInpakOpdrachtMetId(It.IsAny<long>()))
                .Returns(bestelling).Verifiable();

            // Act
            BestellingController target = new BestellingController(repoMock.Object, bestellingAgentMock.Object);
            target.MeldKlaar(12);
            //Assert
            repoMock.Verify(repo => repo.GetInpakOpdrachtMetId(It.IsAny<long>()));
        }

        [TestMethod]
        public void MeldIngepakt_CallsAgentWhenKanKlaargemeldWorden()
        {
            // Arrange
            Mock<IBestellingRepository> repoMock = new Mock<IBestellingRepository>();
            Mock<IBestellingAgent> bestellingAgentMock = new Mock<IBestellingAgent>();

            BestelRegel regel1 = new BestelRegel { Ingepakt = true };
            Bestelling bestelling = new Bestelling { KanKlaarGemeldWorden = true };
            bestelling.BestelRegels.Add(regel1);

            repoMock.Setup(e => e.GetInpakOpdrachtMetId(It.IsAny<long>()))
                .Returns(bestelling);

            // Act
            BestellingController target = new BestellingController(repoMock.Object, bestellingAgentMock.Object);
            target.MeldKlaar(12);

            //Assert
            bestellingAgentMock.Verify(repo => repo.MeldBestellingKlaarAsync(12));
        }

        [TestMethod]
        public void MeldIngepakt_DoesntCallAgentWhenSomeRijenAreNotIngepaktAndFactuurAndAdreslabelAreGeprint()
        {
            // Arrange
            Mock<IBestellingRepository> repoMock = new Mock<IBestellingRepository>();
            Mock<IBestellingAgent> bestellingAgentMock = new Mock<IBestellingAgent>();

            BestelRegel regel1 = new BestelRegel { Ingepakt = false };
            Bestelling bestelling = new Bestelling { KlaarGemeld = false, AdresLabelGeprint = true, FactuurGeprint = true };
            bestelling.BestelRegels.Add(regel1);

            repoMock.Setup(e => e.GetInpakOpdrachtMetId(It.IsAny<long>()))
                .Returns(bestelling);

            // Act
            BestellingController target = new BestellingController(repoMock.Object, bestellingAgentMock.Object);
            target.MeldKlaar(12);
            //Assert
            bestellingAgentMock.Verify(repo => repo.MeldBestellingKlaarAsync(12), Times.Never);
        }

        [TestMethod]
        public void GetRegistreerBetaling_ReturnsViewResult()
        {
            // Arrange
            Mock<IBestellingRepository> repoMock = new Mock<IBestellingRepository>();
            Mock<IBestellingAgent> bestellingAgentMock = new Mock<IBestellingAgent>();

            BestellingController target = new BestellingController(repoMock.Object, bestellingAgentMock.Object);

            // Act
            IActionResult result = target.GetRegistreerBetaling();

            // Arrange
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        [DataRow(20)]
        [DataRow(45)]
        public void VinkBestelregelAan_CallsGetInpakOpdrachtMetId(long bestellingId)
        {
            // Arrange
            Mock<IBestellingRepository> repoMock = new Mock<IBestellingRepository>();
            Mock<IBestellingAgent> bestellingAgentMock = new Mock<IBestellingAgent>();

            repoMock.Setup(e => e.GetInpakOpdrachtMetId(bestellingId))
                .Returns(new Bestelling { BestelRegels = { new BestelRegel { Id = 0 } } })
                .Verifiable();

            BestellingController target = new BestellingController(repoMock.Object, bestellingAgentMock.Object);

            // Act
            target.VinkBestelregelAan(bestellingId, 0).Wait();

            // Arrange
            repoMock.Verify();
        }

        [TestMethod]
        [DataRow(20, 10)]
        [DataRow(45, 23)]
        public void VinkBestelregelAan_ThrowsExceptionOnInvalidId(long bestellingId, long bestelregelId)
        {
            // Arrange
            Mock<IBestellingRepository> repoMock = new Mock<IBestellingRepository>();
            Mock<IBestellingAgent> bestellingAgentMock = new Mock<IBestellingAgent>();

            repoMock.Setup(e => e.GetInpakOpdrachtMetId(bestellingId))
                .Returns(new Bestelling());

            BestellingController target = new BestellingController(repoMock.Object, bestellingAgentMock.Object);

            // Act
            async Task Act() => await target.VinkBestelregelAan(bestellingId, bestelregelId);

            // Arrange
            var exception = Assert.ThrowsExceptionAsync<InvalidOperationException>(Act).Result;
            Assert.AreEqual($"Bestelrij with id {bestelregelId} not found in bestelling", exception.Message);
        }

        [TestMethod]
        [DataRow(20, 10)]
        [DataRow(45, 23)]
        public void VinkBestelregelAan_CallsPakBestelregelInAsyncOnAgent(long bestellingId, long bestelregelId)
        {
            // Arrange
            Mock<IBestellingRepository> repoMock = new Mock<IBestellingRepository>();
            Mock<IBestellingAgent> bestellingAgentMock = new Mock<IBestellingAgent>();

            repoMock.Setup(e => e.GetInpakOpdrachtMetId(bestellingId))
                .Returns(new Bestelling { BestelRegels = { new BestelRegel { Id = bestelregelId } } });

            BestellingController target = new BestellingController(repoMock.Object, bestellingAgentMock.Object);

            // Act
            target.VinkBestelregelAan(bestellingId, bestelregelId).Wait();

            // Arrange
            bestellingAgentMock.Verify(e => e.PakBestelregelInAsync(bestellingId, bestelregelId));
        }

        [TestMethod]
        [DataRow(20, 10)]
        [DataRow(45, 23)]
        public void VinkBestelregelAan_ReturnsRedirectToPage(long bestellingId, long bestelregelId)
        {
            // Arrange
            Mock<IBestellingRepository> repoMock = new Mock<IBestellingRepository>();
            Mock<IBestellingAgent> bestellingAgentMock = new Mock<IBestellingAgent>();

            repoMock.Setup(e => e.GetInpakOpdrachtMetId(bestellingId))
                .Returns(new Bestelling { BestelRegels = { new BestelRegel { Id = bestelregelId } } });

            BestellingController target = new BestellingController(repoMock.Object, bestellingAgentMock.Object);

            // Act
            IActionResult result = target.VinkBestelregelAan(bestellingId, bestelregelId).Result;

            // Arrange
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));

            RedirectToActionResult actionResult = result as RedirectToActionResult;
            Assert.AreEqual(nameof(BestellingController.GetNextInpakBestelling), actionResult.ActionName);
        }

        [TestMethod]
        [DataRow("2302")]
        [DataRow("69392")]
        public void GetOpenstaandBedrag_CallsGetBestellingByBestellingNummerOnRepository(string bestellingNummer)
        {
            // Arrange
            Mock<IBestellingRepository> repoMock = new Mock<IBestellingRepository>();
            Mock<IBestellingAgent> bestellingAgentMock = new Mock<IBestellingAgent>();

            BestellingController bestellingController = new BestellingController(repoMock.Object, bestellingAgentMock.Object);

            // Act
            bestellingController.GetOpenstaandBedrag(bestellingNummer);

            // Assert
            repoMock.Verify(e => e.GetBestellingByBestellingNummer(bestellingNummer));
        }

        [TestMethod]
        [DataRow("2302")]
        [DataRow("69392")]
        public void GetOpenstaandBedrag_ReturnsNotFoundOnNonExisting(string bestellingNummer)
        {
            // Arrange
            Mock<IBestellingRepository> repoMock = new Mock<IBestellingRepository>();
            Mock<IBestellingAgent> bestellingAgentMock = new Mock<IBestellingAgent>();

            BestellingController bestellingController = new BestellingController(repoMock.Object, bestellingAgentMock.Object);

            // Act
            IActionResult actionResult = bestellingController.GetOpenstaandBedrag(bestellingNummer);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NotFoundResult));
        }

        [TestMethod]
        [DataRow("2302", "23.23")]
        [DataRow("69392", "20.00")]
        public void GetOpenstaandBedrag_ReturnsJsonWithBedrag(string bestellingNummer, string bedragStr)
        {
            // Arrange
            decimal bedrag = decimal.Parse(bedragStr);

            Mock<IBestellingRepository> repoMock = new Mock<IBestellingRepository>();
            Mock<IBestellingAgent> bestellingAgentMock = new Mock<IBestellingAgent>();

            repoMock.Setup(e => e.GetBestellingByBestellingNummer(bestellingNummer))
                .Returns(new Bestelling { OpenstaandBedrag = bedrag });

            BestellingController bestellingController = new BestellingController(repoMock.Object, bestellingAgentMock.Object);

            // Act
            IActionResult actionResult = bestellingController.GetOpenstaandBedrag(bestellingNummer);

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(JsonResult));

            dynamic json = (actionResult as JsonResult).Value;
            Assert.AreEqual(bedrag, json.OpenstaandBedrag);
        }

        [TestMethod]
        [DataRow("28941")]
        [DataRow("24153")]
        [DataRow("51234213")]
        public void GetBestellingDetails_WhenBestellingExistsReturnsViewResult(string bestellingNummer)
        {
            // Arrange
            Mock<IBestellingRepository> repoMock = new Mock<IBestellingRepository>();
            Mock<IBestellingAgent> bestellingAgentMock = new Mock<IBestellingAgent>();
            repoMock.Setup(repo => repo.GetBestellingByBestellingNummer(bestellingNummer)).Returns(new Bestelling { BestellingNummer = bestellingNummer });
            BestellingController target = new BestellingController(repoMock.Object, bestellingAgentMock.Object);

            // Act
            var result = target.GetBestellingDetails(bestellingNummer);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        [DataRow("28941")]
        [DataRow("24153")]
        [DataRow("51234213")]
        public void GetBestellingDetails_WhenBestellingExistsReturnsViewResultWithCorrectViewModel(string bestellingNummer)
        {
            // Arrange
            Mock<IBestellingRepository> repoMock = new Mock<IBestellingRepository>();
            Mock<IBestellingAgent> bestellingAgentMock = new Mock<IBestellingAgent>();
            repoMock.Setup(repo => repo.GetBestellingByBestellingNummer(bestellingNummer)).Returns(new Bestelling { BestellingNummer = bestellingNummer });
            BestellingController target = new BestellingController(repoMock.Object, bestellingAgentMock.Object);

            // Act
            var result = target.GetBestellingDetails(bestellingNummer);

            // Assert
            var viewResult = (ViewResult)result;
            Assert.IsInstanceOfType(viewResult.Model, typeof(BestellingDetailViewModel));
            Assert.AreEqual(bestellingNummer, ((BestellingDetailViewModel)viewResult.Model).Bestelling.BestellingNummer);
        }

        [TestMethod]
        [DataRow("28941")]
        [DataRow("24153")]
        [DataRow("51234213")]
        public void GetBestellingDetails_CallsRepositoryBetBestellingByBestellingNummer(string bestellingNummer)
        {
            // Arrange
            Mock<IBestellingRepository> repoMock = new Mock<IBestellingRepository>();
            Mock<IBestellingAgent> bestellingAgentMock = new Mock<IBestellingAgent>();
            repoMock.Setup(repo => repo.GetBestellingByBestellingNummer(bestellingNummer)).Returns(new Bestelling { BestellingNummer = bestellingNummer }).Verifiable();
            BestellingController target = new BestellingController(repoMock.Object, bestellingAgentMock.Object);

            // Act
            target.GetBestellingDetails(bestellingNummer);

            // Assert
            repoMock.Verify();
        }

        [TestMethod]
        [DataRow("28941")]
        [DataRow("24153")]
        [DataRow("51234213")]
        public void GetBestellingDetails_Returns404IfBestellingDoesntExist(string bestellingNummer)
        {
            // Arrange
            Mock<IBestellingRepository> repoMock = new Mock<IBestellingRepository>();
            Mock<IBestellingAgent> bestellingAgentMock = new Mock<IBestellingAgent>();
            repoMock.Setup(repo => repo.GetBestellingByBestellingNummer(bestellingNummer)).Returns((Bestelling)null);
            BestellingController target = new BestellingController(repoMock.Object, bestellingAgentMock.Object);

            // Act
            var result = target.GetBestellingDetails(bestellingNummer);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void WanBetalersOverzicht_ReturnsViewResultWithView()
        {
            Mock<IBestellingRepository> repoMock = new Mock<IBestellingRepository>();
            Mock<IBestellingAgent> bestellingAgentMock = new Mock<IBestellingAgent>();

            repoMock.Setup(e => e.GetWanbetaalBestellingen())
                .Returns(new List<Bestelling>());
            bestellingAgentMock.Setup(e => e.ControleerOfErWanbetalersZijnAsync())
                .ReturnsAsync(new List<Bestelling>());

            BestellingController target = new BestellingController(repoMock.Object, bestellingAgentMock.Object);

            // Act
            IActionResult result = target.WanbetalersOverzicht().Result;

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));

            ViewResult viewResult = result as ViewResult;
            Assert.AreEqual("WanbetaalOverzicht", viewResult.ViewName);
        }

        [TestMethod]
        public void WanBetalersOverzicht_CallsGetWanBetaalBestellingenOnRepository()
        {
            Mock<IBestellingRepository> repoMock = new Mock<IBestellingRepository>();
            Mock<IBestellingAgent> bestellingAgentMock = new Mock<IBestellingAgent>();

            repoMock.Setup(e => e.GetWanbetaalBestellingen())
                .Returns(new List<Bestelling>())
                .Verifiable();
            bestellingAgentMock.Setup(e => e.ControleerOfErWanbetalersZijnAsync())
                .ReturnsAsync(new List<Bestelling>());

            BestellingController target = new BestellingController(repoMock.Object, bestellingAgentMock.Object);

            // Act
            target.WanbetalersOverzicht().Wait();

            // Assert
            repoMock.Verify();
        }

        [TestMethod]
        public void WanBetalersOverzicht_CallsControleerOfErWanbetalersZijnAsyncOnAgent()
        {
            Mock<IBestellingRepository> repoMock = new Mock<IBestellingRepository>();
            Mock<IBestellingAgent> bestellingAgentMock = new Mock<IBestellingAgent>();

            repoMock.Setup(e => e.GetWanbetaalBestellingen())
                .Returns(new List<Bestelling>());
            bestellingAgentMock.Setup(e => e.ControleerOfErWanbetalersZijnAsync())
                .ReturnsAsync(new List<Bestelling>())
                .Verifiable();

            BestellingController target = new BestellingController(repoMock.Object, bestellingAgentMock.Object);

            // Act
            target.WanbetalersOverzicht().Wait();

            // Assert
            bestellingAgentMock.Verify();
        }

        [TestMethod]
        public void WanBetalersOverzicht_ReturnsCombinedAgentAndRepositoryData()
        {
            Mock<IBestellingRepository> repoMock = new Mock<IBestellingRepository>();
            Mock<IBestellingAgent> bestellingAgentMock = new Mock<IBestellingAgent>();

            Bestelling repoData = new Bestelling { Id = 2 };
            Bestelling agentData = new Bestelling { Id = 1 };

            repoMock.Setup(e => e.GetWanbetaalBestellingen())
                .Returns(new List<Bestelling>{ repoData });
            bestellingAgentMock.Setup(e => e.ControleerOfErWanbetalersZijnAsync())
                .ReturnsAsync(new List<Bestelling>{ agentData });

            BestellingController target = new BestellingController(repoMock.Object, bestellingAgentMock.Object);

            // Act
            IActionResult result = target.WanbetalersOverzicht().Result;

            // Assert
            IEnumerable<Bestelling> resultData = (result as ViewResult).Model as IEnumerable<Bestelling>;
            Assert.AreEqual(2, resultData.Count());

            Assert.IsNotNull(resultData.Single(b => b.Id == 2));
            Assert.IsNotNull(resultData.Single(b => b.Id == 1));
        }

        [TestMethod]
        public void WanBetalersOverzicht_ReturnsCombinedAgentAndRepositoryDataWithoutDuplicates()
        {
            Mock<IBestellingRepository> repoMock = new Mock<IBestellingRepository>();
            Mock<IBestellingAgent> bestellingAgentMock = new Mock<IBestellingAgent>();

            Bestelling repoData = new Bestelling { Id = 1 };
            Bestelling agentData = new Bestelling { Id = 1 };

            repoMock.Setup(e => e.GetWanbetaalBestellingen())
                .Returns(new List<Bestelling>{ repoData });
            bestellingAgentMock.Setup(e => e.ControleerOfErWanbetalersZijnAsync())
                .ReturnsAsync(new List<Bestelling>{ agentData });

            BestellingController target = new BestellingController(repoMock.Object, bestellingAgentMock.Object);

            // Act
            IActionResult result = target.WanbetalersOverzicht().Result;

            // Assert
            IEnumerable<Bestelling> resultData = (result as ViewResult).Model as IEnumerable<Bestelling>;
            Assert.AreEqual(1, resultData.Count());

            Assert.IsNotNull(resultData.Single(b => b.Id == 1));
        }

        [TestMethod]
        public void RegistreerBetaling_FillsViewBageWithErrorIfExceptionIsThrown()
        {
            // Arrange
            Mock<IBestellingRepository> repoMock = new Mock<IBestellingRepository>();
            Mock<IBestellingAgent> bestellingAgentMock = new Mock<IBestellingAgent>();
            bestellingAgentMock.Setup(agent => agent.RegistreerBetalingAsync(It.IsAny<string>(), It.IsAny<decimal>())).Throws(new DestinationQueueException("", new Exception("Sequence contains no elements"), "", "", new Guid()));
            BestellingController target = new BestellingController(repoMock.Object, bestellingAgentMock.Object);

            // Act
            var betaling = new BetalingRegistrerenViewModel { BestellingNummer = "375945", BetaaldBedrag = 10m, OpenstaandBedrag = 12m, Verschil = 2m };
            IActionResult result = target.BetalingRegistreren(betaling).Result;

            // Arrange
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            ViewResult viewResult = result as ViewResult;
            Assert.AreEqual(FunctionalExceptionMessages.BestellingNotFound, viewResult.ViewData["Error"]);
        }
    }
}
