using System;
using BackOfficeFrontendService.Test.Integration.Constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace BackOfficeFrontendService.Test.Integration.UITests
{
    [TestClass]
    public class IndexTest
    {
        private static string _baseUrl;

        private static ChromeOptions _chromeOptions;

        private IWebDriver _browser;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _baseUrl = Environment.GetEnvironmentVariable("APP_URL")
                        ?? "http:/localhost:3000";

            _chromeOptions = new ChromeOptions();
            _chromeOptions.AddArguments("headless");
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _browser = new ChromeDriver(_chromeOptions) { Url = _baseUrl};
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _browser.Close();
            _browser.Dispose();
        }

        [TestMethod]
        public void IndexPageLoadsWithTitle()
        {
            // Act
            IWebElement result = _browser.FindElement(By.ClassName("navbar-brand"));

            // Assert
            Assert.IsTrue(result.Text.Contains("Kantilever backoffice"));
        }

        [TestMethod]
        public void NavigationOnlyShowsLoginWhenNotLoggedIn()
        {
            // Act
            IWebElement result = _browser.FindElement(By.TagName("nav"));

            // Assert
            Assert.IsTrue(result.Text.Contains("Log in"));
            Assert.IsFalse(result.Text.Contains("Registreer betaling"));
            Assert.IsFalse(result.Text.Contains("Volgende bestelling inpakken"));
            Assert.IsFalse(result.Text.Contains("Voorraad bijbestellen"));
            Assert.IsFalse(result.Text.Contains("Wanbetaler overzicht"));
            Assert.IsFalse(result.Text.Contains("Bestellingen goedkeuren"));
            Assert.IsFalse(result.Text.Contains("Registreer betaling"));
            Assert.IsFalse(result.Text.Contains("Log uit"));
        }
    }
}
