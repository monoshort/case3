using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using FrontendService.Agents;
using FrontendService.Agents.Abstractions;
using FrontendService.Constants;
using FrontendService.Controllers;
using FrontendService.Models;
using FrontendService.Repositories.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FrontendService.Test.Unit.Controllers
{
    [TestClass]
    public class KlantControllerTest
    {
        [TestMethod]
        [DataRow("hans.k@mail.com")]
        [DataRow("freek.k@mail.com")]
        [DataRow("jan.dj@mail.com")]
        [DataRow("piet.vr@mail.com")]
        [DataRow("pim.d@mail.com")]
        public void BestellingenVanKlant_ShouldReturnAJsonResultWithExpectedData(string username)
        {
            // Arrange
            Mock<IKlantRepository> klantRepoMock = new Mock<IKlantRepository>();
            Mock<IBestellingRepository> bestellingRepoMock = new Mock<IBestellingRepository>();

            IEnumerable<Bestelling> expectedData = new[]
            {
                new Bestelling {Goedgekeurd = false, BestellingNummer = "56201659"},
                new Bestelling {Goedgekeurd = false, BestellingNummer = "50284924"},
            };

            bestellingRepoMock.Setup(e => e.GetByKlantUsername(username))
                .Returns(expectedData);

            KlantController target = new KlantController(klantRepoMock.Object, bestellingRepoMock.Object);

            // Act
            IActionResult result = target.BestellingenVanKlant(username);

            // Assert
            Assert.IsInstanceOfType(result, typeof(JsonResult));
            IEnumerable<Bestelling> resultData = (result as JsonResult).Value as IEnumerable<Bestelling>;

            CollectionAssert.AreEquivalent(expectedData.ToList(), resultData.ToList());
        }

        [TestMethod]
        [DataRow("hans.k@mail.com")]
        [DataRow("freek.k@mail.com")]
        [DataRow("jan.dj@mail.com")]
        [DataRow("piet.vr@mail.com")]
        [DataRow("pim.d@mail.com")]
        public void BestellingenVanKlant_ShouldCallGetBestellingenVanKlantOnRepository(string username)
        {
            // Arrange
            Mock<IKlantRepository> klantRepoMock = new Mock<IKlantRepository>();
            Mock<IBestellingRepository> bestelRepoMock = new Mock<IBestellingRepository>();

            KlantController target = new KlantController(klantRepoMock.Object, bestelRepoMock.Object);

            // Act
            target.BestellingenVanKlant(username);

            // Assert
            bestelRepoMock.Verify(e => e.GetByKlantUsername(username));
        }

        [TestMethod]
        [DataRow("jan@piet.com")]
        [DataRow("mo@ma.nl")]
        public void GetKlant_ReturnsForbidOnWrongCustomer(string username)
        {
            // Arrange
            Mock<IKlantRepository> klantRepository = new Mock<IKlantRepository>();
            Mock<IBestellingRepository> bestellingRepository = new Mock<IBestellingRepository>();
            KlantController controller = new KlantController(klantRepository.Object, bestellingRepository.Object);

            Mock<ClaimsPrincipal> mockPrincipal = new Mock<ClaimsPrincipal>();
            Mock<ClaimsIdentity> mockIdentity = new Mock<ClaimsIdentity>();

            mockPrincipal.SetupGet(e => e.Identities)
                .Returns(new ClaimsIdentity[0]);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = mockPrincipal.Object
                }
            };

            // Act
            IActionResult result = controller.GetKlant(username);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ForbidResult));

            ForbidResult forbidResult = result as ForbidResult;
            Assert.AreEqual(KlantController.ForbiddenKlantMessage, forbidResult.AuthenticationSchemes.First());
        }

        [TestMethod]
        [DataRow("jan@piet.com")]
        [DataRow("mo@ma.nl")]
        public void GetKlant_CallsGetByUsernameOnRepository(string username)
        {
            // Arrange
            Mock<IKlantRepository> klantRepository = new Mock<IKlantRepository>();
            Mock<IBestellingRepository> bestellingRepository = new Mock<IBestellingRepository>();
            KlantController controller = new KlantController(klantRepository.Object, bestellingRepository.Object);

            Mock<ClaimsPrincipal> mockPrincipal = new Mock<ClaimsPrincipal>();

            Mock<ClaimsIdentity> claimsIdentity = new Mock<ClaimsIdentity>();
            claimsIdentity.Setup(e => e.Claims)
                .Returns(new List<Claim>
                {
                    new Claim("name", username),
                });
            claimsIdentity.SetupGet(e => e.AuthenticationType)
                .Returns("Bearer");

            mockPrincipal.SetupGet(e => e.Identities)
                .Returns(new[] { claimsIdentity.Object });

            mockPrincipal.SetupGet(e => e.Identities)
                .Returns(new[] {claimsIdentity.Object});

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = mockPrincipal.Object
                }
            };


            // Act
            controller.GetKlant(username);

            // Assert
            klantRepository.Verify(e => e.GetByUsername(username));
        }

        [TestMethod]
        [DataRow("jan@piet.com")]
        [DataRow("mo@ma.nl")]
        public void GetKlant_Returns404OnNotFound(string username)
        {
            // Arrange
            Mock<IKlantRepository> klantRepository = new Mock<IKlantRepository>();
            Mock<IBestellingRepository> bestellingRepository = new Mock<IBestellingRepository>();
            KlantController controller = new KlantController(klantRepository.Object, bestellingRepository.Object);

            Mock<ClaimsPrincipal> mockPrincipal = new Mock<ClaimsPrincipal>();

            Mock<ClaimsIdentity> claimsIdentity = new Mock<ClaimsIdentity>();
            claimsIdentity.Setup(e => e.Claims)
                .Returns(new List<Claim>
                {
                    new Claim("name", username),
                });
            claimsIdentity.SetupGet(e => e.AuthenticationType)
                .Returns("Bearer");

            mockPrincipal.SetupGet(e => e.Identities)
                .Returns(new[] { claimsIdentity.Object });

            mockPrincipal.SetupGet(e => e.Identities)
                .Returns(new[] {claimsIdentity.Object});

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = mockPrincipal.Object
                }
            };


            // Act
            IActionResult result = controller.GetKlant(username);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        [DataRow("jan@piet.com")]
        [DataRow("mo@ma.nl")]
        public void GetKlant_ReturnsJsonWithKlant(string username)
        {
            // Arrange
            Mock<IKlantRepository> klantRepository = new Mock<IKlantRepository>();
            Mock<IBestellingRepository> bestellingRepository = new Mock<IBestellingRepository>();
            KlantController controller = new KlantController(klantRepository.Object, bestellingRepository.Object);

            Mock<ClaimsPrincipal> mockPrincipal = new Mock<ClaimsPrincipal>();

            Mock<ClaimsIdentity> claimsIdentity = new Mock<ClaimsIdentity>();
            claimsIdentity.Setup(e => e.Claims)
                .Returns(new List<Claim>
                {
                    new Claim("name", username),
                });
            claimsIdentity.SetupGet(e => e.AuthenticationType)
                .Returns("Bearer");

            mockPrincipal.SetupGet(e => e.Identities)
                .Returns(new[] { claimsIdentity.Object });

            mockPrincipal.SetupGet(e => e.Identities)
                .Returns(new[] {claimsIdentity.Object});

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = mockPrincipal.Object
                }
            };

            Klant klant = new Klant
            {
                Naam = username,
            };

            klantRepository.Setup(e => e.GetByUsername(username))
                .Returns(klant);

            // Act
            IActionResult result = controller.GetKlant(username);

            // Assert
            Assert.IsInstanceOfType(result, typeof(JsonResult));

            JsonResult jsonResult = result as JsonResult;
            Assert.AreEqual(klant, jsonResult.Value);
        }
    }
}
