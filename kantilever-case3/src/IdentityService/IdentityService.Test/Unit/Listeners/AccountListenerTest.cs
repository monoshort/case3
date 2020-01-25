using System;
using System.Collections.Generic;
using System.Security.Claims;
using IdentityService.Commands;
using IdentityService.Listeners;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace IdentityService.Test.Unit.Listeners
{
    [TestClass]
    public class AccountListenerTest
    {
        [TestMethod]
        [DataRow("test@test.nl", "testPassword", 54)]
        [DataRow("TheMan@test.nl", "1234", 65)]
        public void HandleMaakNieuweGebruikerAan_CallsCreateAsyncOnUserManagerWithUser(string username, string password, long klantId)
        {
            // Arrange
            Mock<IUserStore<IdentityUser>> userStoreMock = new Mock<IUserStore<IdentityUser>>();
            Mock<UserManager<IdentityUser>> userManagerMock = new Mock<UserManager<IdentityUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            IdentityResult result = IdentityResult.Success;

            userManagerMock.Setup(e =>
                e.CreateAsync(It.Is<IdentityUser>(e => e.UserName == username), password))
                .ReturnsAsync(result)
                .Verifiable();

            userManagerMock.Setup(e => e.AddClaimsAsync(It.IsAny<IdentityUser>(), It.IsAny<IEnumerable<Claim>>()))
                .ReturnsAsync(result);

            MaakAccountAanCommand command = new MaakAccountAanCommand
            {
                Username = username,
                Password = password
            };

            AccountListener accountListener = new AccountListener(userManagerMock.Object, new NullLoggerFactory());

            // Act
            accountListener.HandleMaakKlantAccountAan(command);

            // Assert
            userManagerMock.Verify();
        }

        [TestMethod]
        [DataRow("That is catastrophic!")]
        [DataRow("Some error")]
        public void HandleMaakNieuweGebruikerAan_ThrowsExceptionOnCreateError(string errorDescription)
        {
            // Arrange
            Mock<IUserStore<IdentityUser>> userStoreMock = new Mock<IUserStore<IdentityUser>>();
            Mock<UserManager<IdentityUser>> userManagerMock = new Mock<UserManager<IdentityUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            IdentityError error = new IdentityError { Description = errorDescription };
            IdentityResult result = IdentityResult.Failed(error);

            userManagerMock.Setup(e => e.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(result);

            MaakAccountAanCommand command = new MaakAccountAanCommand
            {
                Username = "user",
                Password = "password"
            };

            AccountListener accountListener = new AccountListener(userManagerMock.Object, new NullLoggerFactory());

            // Act
            void Act() => accountListener.HandleMaakKlantAccountAan(command);

            // Assert
            Exception exception = Assert.ThrowsException<Exception>(Act);
            Assert.AreEqual(errorDescription, exception.Message);
        }

        [TestMethod]
        [DataRow("test@test.nl", "testPassword")]
        [DataRow("TheMan@test.nl", "1234")]
        public void HandleMaakNieuweGebruikerAan_CallsAddClaimsAsyncOnUserManagerWithUserAndClaims(string username, string password)
        {
            // Arrange
            Mock<IUserStore<IdentityUser>> userStoreMock = new Mock<IUserStore<IdentityUser>>();
            Mock<UserManager<IdentityUser>> userManagerMock = new Mock<UserManager<IdentityUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            IdentityResult result = IdentityResult.Success;

            userManagerMock.Setup(e => e.AddClaimsAsync(It.IsAny<IdentityUser>(), It.IsAny<IEnumerable<Claim>>()))
                .ReturnsAsync(result);
            userManagerMock.Setup(e => e.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(result);

            MaakAccountAanCommand command = new MaakAccountAanCommand
            {
                Username = username,
                Password = password
            };

            AccountListener accountListener = new AccountListener(userManagerMock.Object, new NullLoggerFactory());

            // Act
            accountListener.HandleMaakKlantAccountAan(command);

            // Assert
            userManagerMock.Verify();
        }

        [TestMethod]
        [DataRow("That is catastrophic!")]
        [DataRow("Some error")]
        public void HandleMaakNieuweGebruikerAan_ThrowsExceptionOnAddClaimError(string errorDescription)
        {
            // Arrange
            Mock<IUserStore<IdentityUser>> userStoreMock = new Mock<IUserStore<IdentityUser>>();
            Mock<UserManager<IdentityUser>> userManagerMock = new Mock<UserManager<IdentityUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            IdentityError error = new IdentityError { Description = errorDescription };
            IdentityResult result = IdentityResult.Failed(error);

            userManagerMock.Setup(e => e.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            userManagerMock.Setup(e => e.AddClaimsAsync(It.IsAny<IdentityUser>(), It.IsAny<IEnumerable<Claim>>()))
                .ReturnsAsync(result);

            MaakAccountAanCommand command = new MaakAccountAanCommand
            {
                Username = "user",
                Password = "password"
            };

            AccountListener accountListener = new AccountListener(userManagerMock.Object, new NullLoggerFactory());

            // Act
            void Act() => accountListener.HandleMaakKlantAccountAan(command);

            // Assert
            Exception exception = Assert.ThrowsException<Exception>(Act);
            Assert.AreEqual(errorDescription, exception.Message);
        }

        [TestMethod]
        [DataRow("PietBerg@gmail.com")]
        [DataRow("janvandingenen@outlook.com")]
        public void HandleVerwijderAccount_CallsFindyByNameOnUsermanager(string username)
        {
            // Arrange
            Mock<IUserStore<IdentityUser>> userStoreMock = new Mock<IUserStore<IdentityUser>>();
            Mock<UserManager<IdentityUser>> userManagerMock = new Mock<UserManager<IdentityUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            AccountListener target = new AccountListener(userManagerMock.Object, new NullLoggerFactory());
            
            userManagerMock.Setup(e => e.FindByNameAsync(username))
                .ReturnsAsync(new IdentityUser())
                .Verifiable();
            userManagerMock.Setup(e => e.DeleteAsync(It.IsAny<IdentityUser>()))
                .ReturnsAsync(IdentityResult.Success);
            
            // Act
            target.HandleVerwijderAccount(new VerwijderAccountCommand {Username = username});
            
            // Arrange
            userManagerMock.Verify();
        }

        [TestMethod]
        [DataRow("PietBerg@gmail.com")]
        [DataRow("janvandingenen@outlook.com")]
        public void HandleVerwijderAccount_CallsDeleteAsyncWithUser(string username)
        {
            // Arrange
            Mock<IUserStore<IdentityUser>> userStoreMock = new Mock<IUserStore<IdentityUser>>();
            Mock<UserManager<IdentityUser>> userManagerMock = new Mock<UserManager<IdentityUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            AccountListener target = new AccountListener(userManagerMock.Object, new NullLoggerFactory());

            IdentityUser identityUser = new IdentityUser
            {
                UserName = username
            };
            userManagerMock.Setup(e => e.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(identityUser);
            userManagerMock.Setup(e => e.DeleteAsync(It.Is<IdentityUser>(x => x.UserName == username)))
                .ReturnsAsync(IdentityResult.Success)
                .Verifiable();

            // Act
            target.HandleVerwijderAccount(new VerwijderAccountCommand {Username = username});
            
            // Arrange
            userManagerMock.Verify();
        }
        
        [TestMethod]
        [DataRow("PietBerg@gmail.com")]
        [DataRow("janvandingenen@outlook.com")]
        public void HandleVerwijderAccount_ReturnsCommand(string username)
        {
            // Arrange
            Mock<IUserStore<IdentityUser>> userStoreMock = new Mock<IUserStore<IdentityUser>>();
            Mock<UserManager<IdentityUser>> userManagerMock = new Mock<UserManager<IdentityUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            AccountListener target = new AccountListener(userManagerMock.Object, new NullLoggerFactory());

            IdentityUser identityUser = new IdentityUser
            {
                UserName = username
            };
            userManagerMock.Setup(e => e.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(identityUser);
            userManagerMock.Setup(e => e.DeleteAsync(It.Is<IdentityUser>(x => x.UserName == username)))
                .ReturnsAsync(IdentityResult.Success);

            VerwijderAccountCommand command = new VerwijderAccountCommand {Username = username};

            // Act
            VerwijderAccountCommand result = target.HandleVerwijderAccount(command);
            
            // Arrange
            Assert.AreEqual(command, result);
        }
        
        [TestMethod]
        [DataRow("Gebruiker niet gevonden")]
        [DataRow("Geen toegang")]
        public void HandleVerwijderAccount_ThrowsException(string error)
        {
            // Arrange
            Mock<IUserStore<IdentityUser>> userStoreMock = new Mock<IUserStore<IdentityUser>>();
            Mock<UserManager<IdentityUser>> userManagerMock = new Mock<UserManager<IdentityUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            AccountListener target = new AccountListener(userManagerMock.Object, new NullLoggerFactory());

            string username = "jan@jan.nl";
            IdentityUser identityUser = new IdentityUser
            {
                UserName = username
            };

            string errorDescription = "There was an error ooh nee";
            IdentityResult identityResult = IdentityResult.Failed(new IdentityError
            {
                Description = errorDescription
            });
                
            userManagerMock.Setup(e => e.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(identityUser);
            userManagerMock.Setup(e => e.DeleteAsync(It.Is<IdentityUser>(x => x.UserName == username)))
                .ReturnsAsync(identityResult);

            VerwijderAccountCommand command = new VerwijderAccountCommand {Username = username};

            // Act
            Action act = () => target.HandleVerwijderAccount(command);
            
            // Arrange
            Exception exception = Assert.ThrowsException<Exception>(act);
            Assert.AreEqual(errorDescription, exception.Message);
        }
    }
}
