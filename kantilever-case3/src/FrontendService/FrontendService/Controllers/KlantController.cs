using System.Collections.Generic;
using System.Linq;
using FrontendService.Models;
using FrontendService.Repositories.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace FrontendService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KlantController : Controller
    {
        internal const string ForbiddenKlantMessage = "Meegegeven 'username' en 'name' in claim komen niet overeen";

        private readonly IKlantRepository _klantRepository;
        private readonly IBestellingRepository _bestellingRepository;

        public KlantController(IKlantRepository klantRepository, IBestellingRepository bestellingRepository)
        {
            _klantRepository = klantRepository;
            _bestellingRepository = bestellingRepository;
        }

        [HttpGet("bestellingen/{username}")]
        public IActionResult BestellingenVanKlant([FromRoute] string username)
        {
            IEnumerable<Bestelling> data = _bestellingRepository.GetByKlantUsername(username);
            return Json(data);
        }

        [HttpGet("{username}")]
        public IActionResult GetKlant([FromRoute] string username)
        {
            var identity = HttpContext.User.Identities.FirstOrDefault(i => i.AuthenticationType == "Bearer");
            if (identity?.Claims == null || !identity.Claims.Any(c => c.Type == "name" && c.Value == username))
            {
                return Forbid(ForbiddenKlantMessage);
            }

            Klant klant = _klantRepository.GetByUsername(username);
            if (klant == null)
            {
                return NotFound();
            }

            return Json(klant);
        }

    }
}
