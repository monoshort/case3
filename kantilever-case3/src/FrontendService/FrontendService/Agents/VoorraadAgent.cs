using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FrontendService.Agents.Abstractions;
using FrontendService.Constants;
using FrontendService.Models;

namespace FrontendService.Agents
{
    public class VoorraadAgent : IVoorraadAgent
    {
        private readonly IHttpAgent _agent;
        private readonly string _baseUrl;

        public VoorraadAgent(IHttpAgent agent)
        {
            _agent = agent;
            _baseUrl = Environment.GetEnvironmentVariable(EnvNames.VoorraadServiceUrl)
                ?? throw new Exception($"Environment variable {EnvNames.VoorraadServiceUrl} not set");
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<VoorraadMagazijn>> GetAllVoorraadAsync()
        {
            return await _agent.GetAsync<IEnumerable<VoorraadMagazijn>>($"{_baseUrl}/{Endpoints.TotaleVoorraad}");
        }
    }
}
