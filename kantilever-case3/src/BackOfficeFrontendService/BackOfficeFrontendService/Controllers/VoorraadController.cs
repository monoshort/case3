using System.Collections.Generic;
using System.Threading.Tasks;
using BackOfficeFrontendService.Agents.Abstractions;
using BackOfficeFrontendService.Constants;
using BackOfficeFrontendService.Models;
using BackOfficeFrontendService.Repositories.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackOfficeFrontendService.Controllers
{
    public class VoorraadController : Controller
    {
        private readonly IVoorraadRepository _voorraadRepository;
        private readonly IVoorraadAgent _voorraadAgent;

        public VoorraadController(IVoorraadRepository voorraadRepository, IVoorraadAgent voorraadAgent)
        {
            _voorraadRepository = voorraadRepository;
            _voorraadAgent = voorraadAgent;
        }

        [Authorize(Policy = AuthPolicies.KanArtikelenBijbestellenPolicy)]
        public IActionResult BijbestelOverzicht()
        {
            IEnumerable<VoorraadMagazijn> voorraadMagazijns = _voorraadRepository.GetArtikelenNietOpVoorraad();
            return View(voorraadMagazijns);
        }

        [Authorize(Policy = AuthPolicies.KanArtikelenBijbestellenPolicy)]
        public async Task<IActionResult> BijBesteld(long artikelNummer)
        {
            VoorraadMagazijn voorraadMagazijn = _voorraadRepository.GetByArtikelNummer(artikelNummer);

            if (voorraadMagazijn == null)
            {
                return NotFound();
            }

            await _voorraadAgent.ThrowVoorraadBesteldEventAsync(artikelNummer, voorraadMagazijn.BijTeBestellen);

            return RedirectToAction(nameof(BijbestelOverzicht));
        }
    }
}
