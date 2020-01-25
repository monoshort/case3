using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackOfficeFrontendService.Agents.Abstractions;
using BackOfficeFrontendService.Constants;
using BackOfficeFrontendService.Exceptions;
using BackOfficeFrontendService.Models;
using BackOfficeFrontendService.Repositories.Abstractions;
using BackOfficeFrontendService.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Minor.Miffy;

namespace BackOfficeFrontendService.Controllers
{
    public class BestellingController : Controller
    {
        /// <summary>
        /// Bestelling agent to communicate with the network
        /// </summary>
        private readonly IBestellingAgent _bestellingAgent;

        /// <summary>
        /// Repository to persist bestellingen
        /// </summary>
        private readonly IBestellingRepository _repository;

        /// <summary>
        /// Instantiate the controller with a repository and an agent
        /// </summary>
        public BestellingController(IBestellingRepository repository, IBestellingAgent bestellingAgent)
        {
            _repository = repository;
            _bestellingAgent = bestellingAgent;
        }

        /// <summary>
        /// Get a list of unaccepted bestellingen
        /// </summary>
        [Authorize(Policy = AuthPolicies.KanBestellingKeurenPolicy)]
        public IActionResult GetBestellingenToAccept()
        {
            IEnumerable<Bestelling> bestellingen = _repository.GetNietGekeurdeBestellingen();
            return View("NietGekeurdeBestellingenOverzicht", bestellingen);
        }

        [Authorize(Policy = AuthPolicies.KanBestellingInpakkenPolicy)]
        public IActionResult GetNextInpakBestelling()
        {
            Bestelling volgendeInpakOpdracht = _repository.GetVolgendeInpakOpdracht();

            return volgendeInpakOpdracht == null
                ? View("NoBestellingOmInTePakken")
                : View("BestellingInpakken", volgendeInpakOpdracht);
        }

        [Authorize(Policy = AuthPolicies.KanBestellingInpakkenPolicy)]
        public async Task<IActionResult> GetFactuur(long bestellingId)
        {
            Bestelling bestelling = _repository.GetInpakOpdrachtMetId(bestellingId);

            if (bestelling == null)
            {
                return RedirectToAction(nameof(GetNextInpakBestelling));
            }

            await _bestellingAgent.BestellingPrintFactuurAsync(bestelling.Id);
            return View("Factuur", bestelling);
        }

        [Authorize(Policy = AuthPolicies.KanBestellingKeurenPolicy)]
        public async Task<IActionResult> KeurBestellingGoed(long bestellingId)
        {
            try
            {
                await _bestellingAgent.KeurBestellingGoedAsync(bestellingId);
            }
            catch (FunctionalException e)
            {
                if (e.Message == FunctionalExceptionMessages.BestellingNotFound)
                {
                    return NotFound();
                }
            }

            return RedirectToAction(nameof(GetBestellingenToAccept));
        }

        [Authorize(Policy = AuthPolicies.KanBestellingKeurenPolicy)]
        public async Task<IActionResult> KeurBestellingAf(long bestellingId)
        {
            try
            {
                await _bestellingAgent.KeurBestellingAfAsync(bestellingId);
            }
            catch (FunctionalException e)
            {
                if (e.Message == FunctionalExceptionMessages.BestellingNotFound)
                {
                    return NotFound();
                }
            }

            return RedirectToAction(nameof(GetBestellingenToAccept));
        }

        [Authorize(Policy = AuthPolicies.KanBestellingInpakkenPolicy)]
        public async Task<IActionResult> GetAdresLabel(long bestellingId)
        {
            Bestelling bestelling = _repository.GetInpakOpdrachtMetId(bestellingId);

            if (bestelling == null)
            {
                return RedirectToAction(nameof(GetNextInpakBestelling));
            }

            await _bestellingAgent.BestellingPrintAdresLabelAsync(bestelling.Id);
            return View("Adreslabel", bestelling);
        }

        [Authorize(Policy = AuthPolicies.KanBestellingInpakkenPolicy)]
        public async Task<IActionResult> MeldKlaar(long bestellingId)
        {
            Bestelling bestelling = _repository.GetInpakOpdrachtMetId(bestellingId);

            if (bestelling.KanKlaarGemeldWorden)
            {
                await _bestellingAgent.MeldBestellingKlaarAsync(bestellingId);
                return RedirectToAction(nameof(GetNextInpakBestelling));
            }
            
            return RedirectToAction(nameof(GetNextInpakBestelling));
        }

        [Authorize(Policy = AuthPolicies.KanBestellingInpakkenPolicy)]
        public async Task<IActionResult> VinkBestelregelAan(long bestellingId, long bestelregelId)
        {
            Bestelling bestelling = _repository.GetInpakOpdrachtMetId(bestellingId);
            BestelRegel rij = bestelling.BestelRegels.SingleOrDefault(bestelrij => bestelrij.Id == bestelregelId);

            if (rij == null)
            {
                throw new InvalidOperationException($"Bestelrij with id {bestelregelId} not found in bestelling");
            }

            await _bestellingAgent.PakBestelregelInAsync(bestellingId, bestelregelId);
            return RedirectToAction(nameof(GetNextInpakBestelling));
        }

        [Authorize(Policy = AuthPolicies.KanBetalingRegistrerenPolicy)]
        public IActionResult GetRegistreerBetaling()
        {
            return View("RegistreerBetaling", new BetalingRegistrerenViewModel());
        }

        [Authorize(Policy = AuthPolicies.KanBetalingRegistrerenPolicy)]
        [Route("Bestelling/GetOpenstaandBedrag/{bestellingNummer}")]
        public IActionResult GetOpenstaandBedrag(string bestellingNummer)
        {
            Bestelling bestelling = _repository.GetBestellingByBestellingNummer(bestellingNummer);

            if (bestelling != null)
            {
                return Json(new { bestelling.OpenstaandBedrag });
            }

            return NotFound();
        }

        [Authorize(Policy = AuthPolicies.KanBestellingKeurenPolicy)]
        [Route("Bestelling/GetBestellingDetails/{bestellingNummer}")]
        public IActionResult GetBestellingDetails(string bestellingNummer)
        {
            Bestelling bestelling = _repository.GetBestellingByBestellingNummer(bestellingNummer);

            if (bestelling != null)
            {
                return View("BestellingDetailView", new BestellingDetailViewModel { Bestelling = bestelling });
            }
            return NotFound();
        }

        [Authorize(Policy = AuthPolicies.KanBetalingRegistrerenPolicy)]
        public async Task<IActionResult> BetalingRegistreren(BetalingRegistrerenViewModel betaling)
        {
            try
            {
                await _bestellingAgent.RegistreerBetalingAsync(betaling.BestellingNummer, betaling.BetaaldBedrag);
            }
            catch (DestinationQueueException e)
            {
                if (e.InnerException?.Message == "Sequence contains no elements")
                {
                    ViewBag.Error = "Bestellingnummer bestaat niet.";
                }
            }
            return View("RegistreerBetaling", new BetalingRegistrerenViewModel());
        }

        [Authorize(Policy = AuthPolicies.KanWanBetalersBekijken)]
        public async Task<IActionResult> WanbetalersOverzicht()
        {
            IEnumerable<Bestelling> newWanBetalers = await _bestellingAgent.ControleerOfErWanbetalersZijnAsync();
            IEnumerable<Bestelling> oldWanBetalers = _repository.GetWanbetaalBestellingen();

            IEnumerable<Bestelling> result = newWanBetalers.Concat(oldWanBetalers)
                .Distinct(new BestellingEqualityComparer());

            return View("WanbetaalOverzicht", result);
        }
    }
}
