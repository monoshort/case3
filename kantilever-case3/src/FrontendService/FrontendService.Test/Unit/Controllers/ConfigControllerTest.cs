using FrontendService.Constants;
using FrontendService.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FrontendService.Test.Unit.Controllers
{
    [TestClass]
    public class ConfigControllerTest
    {
        [TestMethod]
        [DataRow("auth", "testid", "grant", "http://example.com", "randomScope", "http://test.com")]
        [DataRow("atest", "idtest", "invalid_grant", "http://something.com", "testScope", "http://example.com")]
        public void Get_ReturnsExpectedConfigInOkObjectResult(string authority, string id, string type, string uri, string scope,
            string postUri)
        {
            // Arrange
            Mock<IConfiguration> configMock = new Mock<IConfiguration>();

            Mock<IConfigurationSection> angularAuth = new Mock<IConfigurationSection>();
            angularAuth.SetupGet(e => e.Value).Returns(authority);
            configMock.Setup(e => e.GetSection(EnvNames.AngularAuthority))
                .Returns(angularAuth.Object);

            Mock<IConfigurationSection> angularId = new Mock<IConfigurationSection>();
            angularId.SetupGet(e => e.Value).Returns(id);
            configMock.Setup(e => e.GetSection(EnvNames.AngularClientId))
                .Returns(angularId.Object);

            Mock<IConfigurationSection> angularType = new Mock<IConfigurationSection>();
            angularType.SetupGet(e => e.Value).Returns(type);
            configMock.Setup(e => e.GetSection(EnvNames.AngularReponseType))
                .Returns(angularType.Object);

            Mock<IConfigurationSection> angularUri = new Mock<IConfigurationSection>();
            angularUri.SetupGet(e => e.Value).Returns(uri);
            configMock.Setup(e => e.GetSection(EnvNames.AngularRedirectUri))
                .Returns(angularUri.Object);

            Mock<IConfigurationSection> angularScope = new Mock<IConfigurationSection>();
            angularScope.SetupGet(e => e.Value).Returns(scope);
            configMock.Setup(e => e.GetSection(EnvNames.AngularScope))
                .Returns(angularScope.Object);

            Mock<IConfigurationSection> angularPostLogoutRedirectUriSection = new Mock<IConfigurationSection>();
            angularPostLogoutRedirectUriSection.SetupGet(e => e.Value).Returns(postUri);
            configMock.Setup(e => e.GetSection(EnvNames.AngularPostLogoutRedirectUri))
                .Returns(angularPostLogoutRedirectUriSection.Object);

            ConfigController controller = new ConfigController(configMock.Object);

            // Act
            IActionResult result = controller.Get();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            dynamic resultConfig = (result as OkObjectResult).Value;

            Assert.AreEqual(authority, resultConfig.angular_authority);
            Assert.AreEqual(id, resultConfig.angular_clientid);
            Assert.AreEqual(type, resultConfig.angular_response_type);
            Assert.AreEqual(uri, resultConfig.angular_redirect_uri);
            Assert.AreEqual(scope, resultConfig.angular_scope);
            Assert.AreEqual(postUri, resultConfig.angular_post_logout_redirect_uri);
        }
    }
}
