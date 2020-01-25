using BackOfficeFrontendService.Constants;
using BackOfficeFrontendService.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackOfficeFrontendService.Test.Unit.Exceptions
{
    [TestClass]
    public class FunctionalExceptionTest
    {
        [TestMethod]
        [DataRow("You got mail")]
        [DataRow(FunctionalExceptionMessages.BestellingNotFound)]
        public void Constructor_MessageIsProperlySet(string message)
        {
            // Act
            FunctionalException exception = new FunctionalException(message);

            // Assert
            Assert.AreEqual(message, exception.Message);
        }
    }
}
