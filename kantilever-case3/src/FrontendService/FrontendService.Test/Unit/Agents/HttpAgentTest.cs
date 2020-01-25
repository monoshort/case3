using Flurl.Http.Testing;
using FrontendService.Agents;
using FrontendService.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FrontendService.Test.Unit.Agents
{
    [TestClass]
    public class HttpAgentTest
    {
        private static HttpTest _httpTest;

        [ClassInitialize]
        public static void ClassCleanup(TestContext c)
        {
            _httpTest = new HttpTest();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            _httpTest.Dispose();
        }

        [TestMethod]
        [DataRow("https://test.com/catalogus")]
        [DataRow("https://infosupport.com/admin")]
        public void Get_SendsRequestToEndpoint(string url)
        {
            // Arrange
            _httpTest.RespondWithJson(new object());

            HttpAgent agent = new HttpAgent();

            // Act
            agent.GetAsync<VoorraadMagazijn>(url);

            // Assert
            _httpTest.ShouldHaveCalled(url);
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(5)]
        [DataRow(104)]
        public void Get_ReturnsExpectedVoorraad(int voorraad)
        {
            // Arrange
            VoorraadMagazijn voorraadMagazijn = new VoorraadMagazijn
            {
                Voorraad = voorraad
            };

            _httpTest.RespondWithJson(voorraadMagazijn);

            HttpAgent agent = new HttpAgent();

            // Act
            VoorraadMagazijn result = agent.GetAsync<VoorraadMagazijn>("https://example.com").Result;

            // Assert
            Assert.AreEqual(voorraad, result.Voorraad);
        }

        [TestMethod]
        [DataRow("http://example.com/test/test")]
        [DataRow("http://test.com/api/apples")]
        public void Get_SendsGetRequestToSpecifiedEndpoint(string url)
        {
            // Arrange
            HttpAgent httpAgent = new HttpAgent();

            // Act
            httpAgent.GetAsync<object>(url).Wait();

            // Assert
            _httpTest.ShouldHaveCalled(url);
        }

        private class TestFile
        {
            public string test { get; set; }
        }

        [TestMethod]
        [DataRow("Apples")]
        [DataRow("Pears")]
        public void Get_ReturnsExpectedData(string returnData)
        {
            // Arrange
            HttpAgent httpAgent = new HttpAgent();

            _httpTest.RespondWithJson(new TestFile {test = returnData});

            // Act
            TestFile response = httpAgent.GetAsync<TestFile>("http://example.com").Result;

            // Assert
            Assert.AreEqual(returnData, response.test);
        }

    }
}

