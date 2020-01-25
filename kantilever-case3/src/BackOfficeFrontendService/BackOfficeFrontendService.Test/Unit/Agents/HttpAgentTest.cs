using BackOfficeFrontendService.Agents;
using Flurl.Http.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackOfficeFrontendService.Test.Unit.Agents
{
    [TestClass]
    public class HttpAgentTest
    {
        private HttpTest _httpTest;

        [TestInitialize]
        public void TestInitialize()
        {
            _httpTest = new HttpTest();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _httpTest.Dispose();
        }

        [TestMethod]
        [DataRow("http://example.com/test/test")]
        [DataRow("http://test.com/api/apples")]
        public void Post_SendsPostRequestToSpecifiedEndpoint(string url)
        {
            // Arrange
            HttpAgent httpAgent = new HttpAgent();
            object data = new object();

            // Act
            httpAgent.PostAsync<object, object>(url, data).Wait();

            // Assert
            _httpTest.ShouldHaveCalled(url);
        }

        [TestMethod]
        [DataRow("Apples")]
        [DataRow("Pears")]
        public void Post_ReturnsExpectedData(string returnData)
        {
            // Arrange
            HttpAgent httpAgent = new HttpAgent();
            string data = "Post Data";

            _httpTest.RespondWithJson(returnData);

            // Act
            string response = httpAgent.PostAsync<string, string>("http://example.com", data).Result;

            // Assert
            Assert.AreEqual(returnData, response);
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

        [TestMethod]
        [DataRow("Apples")]
        [DataRow("Pears")]
        public void Get_ReturnsExpectedData(string returnData)
        {
            // Arrange
            HttpAgent httpAgent = new HttpAgent();

            _httpTest.RespondWithJson(returnData);

            // Act
            string response = httpAgent.GetAsync<string>("http://example.com").Result;

            // Assert
            Assert.AreEqual(returnData, response);
        }
    }
}
