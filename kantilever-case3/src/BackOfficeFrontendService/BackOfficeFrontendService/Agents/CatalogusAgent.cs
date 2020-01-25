using System;
using System.Threading.Tasks;
using BackOfficeFrontendService.Agents.Abstractions;
using BackOfficeFrontendService.Constants;
using BackOfficeFrontendService.Models;

namespace BackOfficeFrontendService.Agents
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
        public async Task<Artikel[]> GetAlleArtikelenAsync()
        {
            return await _httpAgent.GetAsync<Artikel[]>($"{_baseUrl}/{Endpoints.TotaleCatalogus}");
        }
    }
}
