using System;
using System.Threading.Tasks;
using BackOfficeFrontendService.Agents.Abstractions;
using BackOfficeFrontendService.Commands;
using BackOfficeFrontendService.Constants;

namespace BackOfficeFrontendService.Agents
{
    public class AuditAgent : IAuditAgent
    {
        private readonly IHttpAgent _httpAgent;
        private readonly string _baseUrl;

        public AuditAgent(IHttpAgent httpAgent)
        {
            _httpAgent = httpAgent;
            _baseUrl = Environment.GetEnvironmentVariable(EnvNames.AuditLoggerUrl)
                       ?? throw new Exception($"Environment variable {EnvNames.AuditLoggerUrl} not set");
        }

        /// <inheritdoc/>
        public async Task<string> ReplayEventsAsync(ReplayEventsCommand command)
        {
            return await _httpAgent.PostAsync<ReplayEventsCommand, string>($"{_baseUrl}/{Endpoints.ReplayEvents}", command);
        }
    }
}
