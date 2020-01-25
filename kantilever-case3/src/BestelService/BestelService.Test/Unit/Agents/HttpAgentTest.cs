using BestelService.Agents;
using Flurl.Http.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BestelService.Test.Unit.Agents
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
        [DataRow("http://example.com")]
        [DataRow("http://test.nl")]
        public void Post_SendsRequestToSpecifiedEndpoint(string url)
        {
            // Arrange
            HttpAgent httpAgent = new HttpAgent();

            // Act
            httpAgent.PostAsync<object, string>(url, new object());

            // Assert
            _httpTest.ShouldHaveCalled(url);
        }

        [TestMethod]
        [DataRow("test")]
        [DataRow("hello world")]
        public void Post_ReturnsExpectedData(string expectedData)
        {
            // Arrange
            HttpAgent httpAgent = new HttpAgent();

            _httpTest.RespondWithJson(expectedData);

            // Act
            string result = httpAgent.PostAsync<object, string>("http://e.nl", new object()).Result;

            // Assert
            Assert.AreEqual(expectedData, result);
        }

        [TestMethod]
        [DataRow("test")]
        [DataRow("hello world")]
        public void Post_SendsRequestToSpecifiedEndpointWithData(string expectedData)
        {
            // Arrange
            HttpAgent httpAgent = new HttpAgent();

            // Act
            httpAgent.PostAsync<string, string>("http://e.nl", expectedData);

            // Assert
            _httpTest.ShouldHaveCalled("http://e.nl")
                .WithRequestJson(expectedData);
        }
    }
}
