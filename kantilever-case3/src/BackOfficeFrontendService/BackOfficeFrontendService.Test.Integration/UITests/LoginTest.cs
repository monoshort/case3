using System;
using BackOfficeFrontendService.Test.Integration.Constants;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace BackOfficeFrontendService.Test.Integration.UITests
{
    [TestClass]
    public class LoginTest
    {
        private static string _baseUrl;

        private static ChromeOptions _chromeOptions;

        private IWebDriver _browser;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _baseUrl = Environment.GetEnvironmentVariable(EnvNames.AppUrl)
                       ?? "http://localhost:3000";
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _browser = new ChromeDriver
            {
                Url = _baseUrl
            };
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _browser.Close();
            _browser.Dispose();
        }

        [TestMethod]
        public void LogIn_OpensLoginWebPage()
        {
            // Arrange
            IWebElement loginButton = _browser.FindElement(By.CssSelector("[href=\"/Account/Login\"]"));

            // Act
            loginButton.Click();

            // Assert
            IWebElement result = _browser.FindElement(By.TagName("h1"));
            Assert.AreEqual("Log in", result.Text);
        }

        [TestMethod]
        public void LogInFlow_WorksWithTestDataAndUnlocksNavigation()
        {
            // Arrange
            IWebElement loginButton = _browser.FindElement(By.CssSelector("[href=\"/Account/Login\"]"));

            loginButton.Click();

            IWebElement usernameField = _browser.FindElement(By.Id("Username"));
            IWebElement passwordField = _browser.FindElement(By.Id("Password"));
            IWebElement submitButton = _browser.FindElement(By.CssSelector("button[value=\"login\"]"));

            // Act
            usernameField.SendKeys(TestData.Username);
            passwordField.SendKeys(TestData.Password);
            submitButton.Click();

            // Assert
            Console.WriteLine(_browser.PageSource);
            IWebElement result = _browser.FindElement(By.TagName("nav"));

            Assert.IsFalse(result.Text.Contains("Log in"));
            Assert.IsTrue(result.Text.Contains("Volgende bestelling inpakken"));
            Assert.IsTrue(result.Text.Contains("Voorraad bijbestellen"));
            Assert.IsTrue(result.Text.Contains("Wanbetaler overzicht"));
            Assert.IsTrue(result.Text.Contains("Bestellingen goedkeuren"));
            Assert.IsTrue(result.Text.Contains("Registreer betaling"));
            Assert.IsTrue(result.Text.Contains("Log uit"));
        }
    }
}
