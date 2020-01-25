using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IdentityServer4.Models;
using IdentityService.Constants;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace IdentityService.Test.Unit
{
    [TestClass]
    public class ConfigTest
    {
        private static readonly string CurrentDirectory = Directory.GetCurrentDirectory();
        private static readonly string IdsPath = Path.Combine(CurrentDirectory, "ids.json");
        private static readonly string ClientsPath = Path.Combine(CurrentDirectory, "clients.json");
        private static readonly string ApisPath = Path.Combine(CurrentDirectory, "apis.json");

        [TestCleanup]
        public void TestCleanup()
        {
            File.Delete(IdsPath);
            File.Delete(ClientsPath);
            File.Delete(ApisPath);
        }

        [TestMethod]
        public void Constructor_ThrowsExceptionOnIdsPathNotSet()
        {
            // Arrange
            Environment.SetEnvironmentVariable(EnvNames.IdsPath, null);
            Environment.SetEnvironmentVariable(EnvNames.ClientsPath, ClientsPath);
            Environment.SetEnvironmentVariable(EnvNames.ApisPath, ApisPath);

            // Act
            void Act() => _ = new Config(new NullLoggerFactory());

            // Assert
            Exception exception = Assert.ThrowsException<Exception>(Act);
            Assert.AreEqual($"Environment variable {EnvNames.IdsPath} not set", exception.Message);
        }

        [TestMethod]
        public void Constructor_ThrowsExceptionOnClientsPathNotSet()
        {
            // Arrange
            Environment.SetEnvironmentVariable(EnvNames.IdsPath, IdsPath);
            Environment.SetEnvironmentVariable(EnvNames.ClientsPath, null);
            Environment.SetEnvironmentVariable(EnvNames.ApisPath, ApisPath);

            // Act
            void Act() => _ = new Config(new NullLoggerFactory());

            // Assert
            Exception exception = Assert.ThrowsException<Exception>(Act);
            Assert.AreEqual($"Environment variable {EnvNames.ClientsPath} not set", exception.Message);
        }

        [TestMethod]
        public void Constructor_ThrowsExceptionOnApisPathNotSet()
        {
            // Arrange
            Environment.SetEnvironmentVariable(EnvNames.IdsPath, IdsPath);
            Environment.SetEnvironmentVariable(EnvNames.ClientsPath, ClientsPath);
            Environment.SetEnvironmentVariable(EnvNames.ApisPath, null);

            // Act
            void Act() => _ = new Config(new LoggerFactory());

            // Assert
            Exception exception = Assert.ThrowsException<Exception>(Act);
            Assert.AreEqual($"Environment variable {EnvNames.ApisPath} not set", exception.Message);
        }

        [TestMethod]
        [DataRow("TestResource")]
        [DataRow("Profile")]
        public void Constructor_ProperlySetsIds(string idName)
        {
            // Arrange
            Environment.SetEnvironmentVariable(EnvNames.IdsPath, IdsPath);
            Environment.SetEnvironmentVariable(EnvNames.ClientsPath, ClientsPath);
            Environment.SetEnvironmentVariable(EnvNames.ApisPath, ApisPath);

            IEnumerable<IdentityResource> ids = new[]
            {
                new IdentityResource
                {
                    DisplayName = idName
                },
            };

            string json = JsonConvert.SerializeObject(ids);

            File.WriteAllText(IdsPath, json);
            File.WriteAllText(ClientsPath, "[]");
            File.WriteAllText(ApisPath, "[]");

            // Act
            Config config = new Config(new NullLoggerFactory());

            // Assert
            Assert.AreEqual(idName, config.Ids.First().DisplayName);
        }

        [TestMethod]
        [DataRow("TestResource")]
        [DataRow("Profile")]
        public void Constructor_ProperlySetsClients(string clientName)
        {
            // Arrange
            Environment.SetEnvironmentVariable(EnvNames.IdsPath, IdsPath);
            Environment.SetEnvironmentVariable(EnvNames.ClientsPath, ClientsPath);
            Environment.SetEnvironmentVariable(EnvNames.ApisPath, ApisPath);

            IEnumerable<Client> ids = new[]
            {
                new Client
                {
                    ClientName = clientName
                }
            };

            string json = JsonConvert.SerializeObject(ids);

            File.WriteAllText(ClientsPath, json);
            File.WriteAllText(IdsPath, "[]");
            File.WriteAllText(ApisPath, "[]");

            // Act
            Config config = new Config(new NullLoggerFactory());

            // Assert
            Assert.AreEqual(clientName, config.Clients.First().ClientName);
        }

        [TestMethod]
        [DataRow("TestResource")]
        [DataRow("Profile")]
        public void Constructor_ProperlySetsApis(string displayName)
        {
            // Arrange
            Environment.SetEnvironmentVariable(EnvNames.IdsPath, IdsPath);
            Environment.SetEnvironmentVariable(EnvNames.ClientsPath, ClientsPath);
            Environment.SetEnvironmentVariable(EnvNames.ApisPath, ApisPath);

            IEnumerable<ApiResource> ids = new[]
            {
                new ApiResource
                {
                    DisplayName = displayName
                },
            };

            string json = JsonConvert.SerializeObject(ids);

            File.WriteAllText(ApisPath, json);
            File.WriteAllText(ClientsPath, "[]");
            File.WriteAllText(IdsPath, "[]");

            // Act
            Config config = new Config(new NullLoggerFactory());

            // Assert
            Assert.AreEqual(displayName, config.Apis.First().DisplayName);
        }
    }
}
