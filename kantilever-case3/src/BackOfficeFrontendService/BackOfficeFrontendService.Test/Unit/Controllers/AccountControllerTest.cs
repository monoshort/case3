using System.Security.Claims;
using BackOfficeFrontendService.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BackOfficeFrontendService.Test.Unit.Controllers
{
    [TestClass]
    public class AccountControllerTest
    {
        [TestMethod]
        public void Login_ReturnsRedirectToIndexPage()
        {
            // Arrange
            AccountController accountController = new AccountController();

            Mock<ClaimsPrincipal> mockPrincipal = new Mock<ClaimsPrincipal>();
            Mock<ClaimsIdentity> mockIdentity = new Mock<ClaimsIdentity>();

            mockPrincipal.SetupGet(e => e.Identity)
                .Returns(mockIdentity.Object);

            mockIdentity.SetupGet(e => e.IsAuthenticated)
                .Returns(true);

            accountController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = mockPrincipal.Object
                }
            };

            // Act
            IActionResult result = accountController.Login();

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            RedirectToActionResult actionResult = result as RedirectToActionResult;

            Assert.AreEqual("Index", actionResult?.ActionName);
            Assert.AreEqual("Home", actionResult?.ControllerName);
        }

        [TestMethod]
        public void Login_ReturnsChallengeOnNotAuthorized()
        {
            // Arrange
            AccountController accountController = new AccountController();

            Mock<ClaimsPrincipal> mockPrincipal = new Mock<ClaimsPrincipal>();
            Mock<ClaimsIdentity> mockIdentity = new Mock<ClaimsIdentity>();

            mockPrincipal.SetupGet(e => e.Identity)
                .Returns(mockIdentity.Object);

            mockIdentity.SetupGet(e => e.IsAuthenticated)
                .Returns(false);

            accountController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = mockPrincipal.Object
                }
            };

            // Act
            IActionResult result = accountController.Login();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ChallengeResult));
        }

        [TestMethod]
        public void Logout_ReturnsRedirectToActionResult()
        {
            // Arrange
            AccountController accountController = new AccountController();

            Mock<ClaimsPrincipal> mockPrincipal = new Mock<ClaimsPrincipal>();
            Mock<ClaimsIdentity> mockIdentity = new Mock<ClaimsIdentity>();

            mockPrincipal.SetupGet(e => e.Identity)
                .Returns(mockIdentity.Object);

            mockIdentity.SetupGet(e => e.IsAuthenticated)
                .Returns(true);

            accountController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = mockPrincipal.Object
                }
            };

            // Act
            IActionResult result = accountController.Login();

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }

        [TestMethod]
        [DataRow("http://example.com/test")]
        [DataRow("http://hello.com/hello")]
        public void AccessDenied_ReturnsViewResultAndReturnUrl(string url)
        {
            // Arrange
            AccountController accountController = new AccountController();

            // Act
            IActionResult result = accountController.AccessDenied(url);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));

            ViewResult viewResult = result as ViewResult;
            Assert.AreEqual(url, viewResult.Model);
        }
    }
}
