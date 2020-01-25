using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FrontendService.Agents.Abstractions;
using FrontendService.Constants;
using FrontendService.Models;

namespace FrontendService.Agents
{
    public class CatalogusAgent : ICatalogusAgent
    {
        private readonly IHttpAgent _httpAgent;
        private readonly string _baseUrl;

        public CatalogusAgent(IHttpAgent httpAgent)
        {
            _httpAgent = httpAgent;
            _baseUrl = Environment.GetEnvironmentVariable(EnvNames.CatalogusServiceUrl)
                ?? throw new Exception($"Environment variable {EnvNames.CatalogusServiceUrl} not set");
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Artikel>> GetAlleArtikelenAsync()
        {
            return await _httpAgent.GetAsync<IEnumerable<Artikel>>($"{_baseUrl}/{Endpoints.TotaleCatalogus}");
        }
    }
}
