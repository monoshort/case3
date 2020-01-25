using System;
using System.Threading.Tasks;
using FrontendService.Agents.Abstractions;
using FrontendService.Models;
using FrontendService.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FrontendService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IAccountAgent _accountAgent;
        private readonly IKlantAgent _klantAgent;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountAgent accountAgent, IKlantAgent klantAgent, ILoggerFactory loggerFactory)
        {
            _accountAgent = accountAgent;
            _klantAgent = klantAgent;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }

        [HttpPost("Registreer")]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            var klant = new Klant
            {
                Naam = registerViewModel.Naam,
                Telefoonnummer = registerViewModel.Telefoonnummer,
                Factuuradres = new Adres
                {
                    StraatnaamHuisnummer = registerViewModel.Adres.StraatnaamHuisnummer,
                    Postcode = registerViewModel.Adres.Postcode,
                    Woonplaats = registerViewModel.Adres.Woonplaats
                },
                Username = registerViewModel.Username
            };

            _logger.LogDebug($"Attempting to create account {registerViewModel.Username}.");
            await _accountAgent.MaakAccountAanAsync(registerViewModel.Username, registerViewModel.Password);

            try
            {
                _logger.LogDebug($"Attempting to create klant {klant.Username}.");
                await _klantAgent.MaakKlantAanAsync(klant);
            }
            catch (Exception)
            {
                _logger.LogCritical($"Failed to create klant, attempting to delete account {klant.Username}");
                await _accountAgent.VerwijderAccountAsync(klant.Username);

                _logger.LogInformation($"Deleted account {klant.Username}");
                throw;
            }

            return Ok();
        }
    }
}