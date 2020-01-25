using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using IdentityServer4.Models;
using IdentityService.Constants;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace IdentityService
{
    public class Config
    {
        /// <summary>
        /// Load in the thee json files
        /// </summary>
        public Config(ILoggerFactory loggerFactory)
        {
            ILogger<Config> logger = loggerFactory.CreateLogger<Config>();

            logger.LogDebug($"Retrieving {EnvNames.IdsPath}");
            string idsPath = Environment.GetEnvironmentVariable(EnvNames.IdsPath)
                             ?? throw new Exception($"Environment variable {EnvNames.IdsPath} not set");

            logger.LogDebug($"Retrieving {EnvNames.ClientsPath}");
            string clientsPath = Environment.GetEnvironmentVariable(EnvNames.ClientsPath)
                                 ?? throw new Exception($"Environment variable {EnvNames.ClientsPath} not set");

            logger.LogDebug($"Retrieving {EnvNames.ApisPath}");
            string apisPath = Environment.GetEnvironmentVariable(EnvNames.ApisPath)
                              ?? throw new Exception($"Environment variable {EnvNames.ApisPath} not set");

            logger.LogInformation($"Retrieving data from {idsPath}");
            string idsData = File.ReadAllText(idsPath);
            logger.LogTrace($"Found data {idsPath}: {idsData}");
            Ids = JsonConvert.DeserializeObject<IdentityResource[]>(idsData);
            logger.LogInformation($"Found {Ids.Count()} clients in {idsPath}");

            logger.LogInformation($"Retrieving data from {clientsPath}");
            string clientData = File.ReadAllText(clientsPath);

            logger.LogTrace($"Found data {clientData}: {clientData}");
            Clients = JsonConvert.DeserializeObject<Client[]>(clientData);
            logger.LogInformation($"Found {Clients.Count()} clients in {clientsPath}");

            logger.LogInformation($"Retrieving data from {apisPath}");
            string apisData = File.ReadAllText(apisPath);

            logger.LogTrace($"Found data {apisData}: {apisData}");
            Apis = JsonConvert.DeserializeObject<ApiResource[]>(apisData);
            logger.LogInformation($"Found {Apis.Count()} clients in {apisPath}");
        }

        public IEnumerable<IdentityResource> Ids { get; }
        public IEnumerable<ApiResource> Apis { get; }
        public IEnumerable<Client> Clients { get; }
    }
}
