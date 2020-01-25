using System;
using System.Threading.Tasks;
using BackOfficeFrontendService.Agents.Abstractions;
using BackOfficeFrontendService.Commands;
using BackOfficeFrontendService.Constants;
using BackOfficeFrontendService.Events;
using BackOfficeFrontendService.Models;
using Minor.Miffy.MicroServices.Events;

namespace BackOfficeFrontendService.Agents
{
    public class VoorraadAgent : IVoorraadAgent
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IHttpAgent _httpAgent;
        private readonly string _voorraadUrl;

        public VoorraadAgent(IHttpAgent httpAgent, IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
            _httpAgent = httpAgent;
            _voorraadUrl = Environment.GetEnvironmentVariable(EnvNames.VoorraadServiceUrl)
                           ?? throw new Exception($"Environment variable {EnvNames.VoorraadServiceUrl} is not set!");
        }

        /// <inheritdoc/>
        public async Task<VoorraadMagazijn[]> GetAllVoorraadAsync()
        {
            return await _httpAgent.GetAsync<VoorraadMagazijn[]>($"{_voorraadUrl}/{Endpoints.TotaleVoorraad}");
        }

        /// <inheritdoc/>
        public async Task HaalVoorraadUitMagazijnAsync(HaalVoorraadUitMagazijnCommand command)
        {
            await _httpAgent.PostAsync<HaalVoorraadUitMagazijnCommand, string>($"{_voorraadUrl}/{Endpoints.HaalVoorraadUitMagazijn}", command);
        }

        /// <inheritdoc/>
        public async Task ThrowVoorraadBesteldEventAsync(long artikelNummer, long aantal)
        {
            var evt = new VoorraadBesteldEvent
            {
                Artikelnummer = artikelNummer,
                BesteldeVoorraad = aantal
            };

            await _eventPublisher.PublishAsync(evt);
        }
    }
}
