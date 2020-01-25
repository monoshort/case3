using BackOfficeFrontendService.Agents.Abstractions;
using BackOfficeFrontendService.Controllers;
using BackOfficeFrontendService.Repositories.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BackOfficeFrontendService.Test.Unit.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void Index_ShouldReturnViewResult()
        {
            // Arrange
            HomeController target = new HomeController();

            // Act
            IActionResult result = target.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
    }
}