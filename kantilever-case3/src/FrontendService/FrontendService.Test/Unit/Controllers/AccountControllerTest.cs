using System;
using FrontendService.Agents.Abstractions;
using FrontendService.Controllers;
using FrontendService.Models;
using FrontendService.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FrontendService.Test.Unit.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {
        [TestMethod]
        [DataRow("Jan", "1249184", "testuser", "test123")]
        [DataRow("Piet", "683923", "janpeter", "test63")]
        public void Register_CallsMaakAccountAanAsyncOnAccountAgent(string naam, string telefoon, string username, string password)
        {
            // Arrange
            Mock<IAccountAgent> accountAgent = new Mock<IAccountAgent>();
            Mock<IKlantAgent> klantAgent = new Mock<IKlantAgent>();
            AccountController controller = new AccountController(accountAgent.Object, klantAgent.Object, new NullLoggerFactory());

            accountAgent.Setup(e => e.MaakAccountAanAsync(username, password))
                .Verifiable();

            klantAgent.Setup(e => e.MaakKlantAanAsync(It.IsAny<Klant>()));

            RegisterViewModel registerViewModel = new RegisterViewModel
            {
                Naam = naam,
                Telefoonnummer = telefoon,
                Username = username,
                Password = password,
                Adres = new Adres
                {
                    Woonplaats = "Breda",
                    Postcode = "11111",
                    StraatnaamHuisnummer = "JacobStraat"
                }
            };

            // Act
            controller.Register(registerViewModel).Wait();

            // Assert
            accountAgent.Verify();
        }

        [TestMethod]
        [DataRow("Jan", "1249184", "testuser", "test123")]
        [DataRow("Piet", "683923", "janpeter", "test63")]
        public void Register_CallsMaakKlantAanAsync(string naam, string telefoon, string username, string password)
        {
            // Arrange
            Mock<IAccountAgent> accountAgent = new Mock<IAccountAgent>();
            Mock<IKlantAgent> klantAgent = new Mock<IKlantAgent>();
            AccountController controller = new AccountController(accountAgent.Object, klantAgent.Object, new NullLoggerFactory());

            accountAgent.Setup(e => e.MaakAccountAanAsync(username, password));

            klantAgent.Setup(e =>
                e.MaakKlantAanAsync(It.Is<Klant>(k => k.Naam == naam && k.Username == username && k.Telefoonnummer == telefoon)))
                .Verifiable();

            RegisterViewModel registerViewModel = new RegisterViewModel
            {
                Naam = naam,
                Telefoonnummer = telefoon,
                Username = username,
                Password = password,
                Adres = new Adres
                {
                    Woonplaats = "Breda",
                    Postcode = "11111",
                    StraatnaamHuisnummer = "JacobStraat"
                }
            };

            // Act
            controller.Register(registerViewModel).Wait();

            // Assert
            klantAgent.Verify();
        }

        [TestMethod]
        [DataRow("Jan", "1249184", "testuser", "test123")]
        [DataRow("Piet", "683923", "janpeter", "test63")]
        public void Register_CallsVerwijderAccountAsyncWhenKlantFails(string naam, string telefoon, string username, string password)
        {
            // Arrange
            Mock<IAccountAgent> accountAgent = new Mock<IAccountAgent>();
            Mock<IKlantAgent> klantAgent = new Mock<IKlantAgent>();
            AccountController controller = new AccountController(accountAgent.Object, klantAgent.Object, new NullLoggerFactory());

            accountAgent.Setup(e => e.MaakAccountAanAsync(username, password));

            klantAgent.Setup(e => e.MaakKlantAanAsync(It.IsAny<Klant>())).Throws<AggregateException>();

            RegisterViewModel registerViewModel = new RegisterViewModel
            {
                Naam = naam,
                Telefoonnummer = telefoon,
                Username = username,
                Password = password,
                Adres = new Adres
                {
                    Woonplaats = "Breda",
                    Postcode = "11111",
                    StraatnaamHuisnummer = "JacobStraat"
                }
            };

            // Act
            try
            {
                controller.Register(registerViewModel).Wait();
            }
            catch
            {
                // We're not testing this right now
            }

            // Assert
            accountAgent.Verify(e => e.VerwijderAccountAsync(username));
        }

        [TestMethod]
        [DataRow("Jan", "1249184", "testuser", "test123")]
        [DataRow("Piet", "683923", "janpeter", "test63")]
        public void Register_RethrowsExceptionWhenKlantFails(string naam, string telefoon, string username, string password)
        {
            // Arrange
            Mock<IAccountAgent> accountAgent = new Mock<IAccountAgent>();
            Mock<IKlantAgent> klantAgent = new Mock<IKlantAgent>();
            AccountController controller = new AccountController(accountAgent.Object, klantAgent.Object, new NullLoggerFactory());

            accountAgent.Setup(e => e.MaakAccountAanAsync(username, password));

            klantAgent.Setup(e => e.MaakKlantAanAsync(It.IsAny<Klant>())).Throws<AggregateException>();

            RegisterViewModel registerViewModel = new RegisterViewModel
            {
                Naam = naam,
                Telefoonnummer = telefoon,
                Username = username,
                Password = password,
                Adres = new Adres
                {
                    Woonplaats = "Breda",
                    Postcode = "11111",
                    StraatnaamHuisnummer = "JacobStraat"
                }
            };

            // Act
            void Act() => controller.Register(registerViewModel).Wait();

            // Assert
            Assert.ThrowsException<AggregateException>(Act);
        }

        [TestMethod]
        [DataRow("Jan", "1249184", "testuser", "test123")]
        [DataRow("Piet", "683923", "janpeter", "test63")]
        public void Register_ReturnsOk(string naam, string telefoon, string username, string password)
        {
            // Arrange
            Mock<IAccountAgent> accountAgent = new Mock<IAccountAgent>();
            Mock<IKlantAgent> klantAgent = new Mock<IKlantAgent>();
            AccountController controller = new AccountController(accountAgent.Object, klantAgent.Object, new NullLoggerFactory());

            RegisterViewModel registerViewModel = new RegisterViewModel
            {
                Naam = naam,
                Telefoonnummer = telefoon,
                Username = username,
                Password = password,
                Adres = new Adres
                {
                    Woonplaats = "Breda",
                    Postcode = "11111",
                    StraatnaamHuisnummer = "JacobStraat"
                }
            };

            // Act
            IActionResult result = controller.Register(registerViewModel).Result;

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }
    }
}
