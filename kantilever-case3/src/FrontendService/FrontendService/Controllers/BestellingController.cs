using System;
using System.Collections.Generic;
using FrontendService.Agents.Abstractions;
using FrontendService.Constants;
using FrontendService.Exceptions;
using FrontendService.Models;
using FrontendService.Repositories.Abstractions;
using FrontendService.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FrontendService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BestellingController : Controller
    {
        private readonly IBestellingAgent _bestellingAgent;
        private readonly IArtikelRepository _artikelRepository;
        private readonly IKlantRepository _klantRepository;

        public BestellingController(IArtikelRepository artikelRepository, IBestellingAgent bestellingAgent, IKlantRepository klantRepository)
        {
            _bestellingAgent = bestellingAgent;
            _artikelRepository = artikelRepository;
            _klantRepository = klantRepository;
        }

        [HttpPost]
        [Authorize(Policy = AuthPolicies.KanBestellenPolicy)]
        public IActionResult Post(BestellingViewModel model)
        {
            try
            {
                var klant = _klantRepository.GetById(model.Klant.Id);
                
                Bestelling bestelling = new Bestelling
                {
                    Klant = klant,
                    BestelDatum = DateTime.Now,
                    AfleverAdres = new Adres
                    {
                        StraatnaamHuisnummer = model.AfleverAdres?.StraatnaamHuisnummer,
                        Postcode = model.AfleverAdres?.Postcode,
                        Woonplaats = model.AfleverAdres?.Woonplaats
                    },
                    BestelRegels = ConvertArtikelViewModelsToBestelRegels(model.Winkelwagen.Artikelen)
                };


                _bestellingAgent.Bestel(bestelling);
                return Ok(new BestellingResult { Message = "Bestelling is successvol geplaatst" });
            }
            catch (ArtikelDoesNotExistException exception)
            {
                return BadRequest(new BestellingResult { Message = $"Artikel met id {exception.ArtikelId} bestaat niet." });
            }
        }

        /// <summary>
        /// Convert viewmodels to bestelregels
        /// </summary>
        private ICollection<BestelRegel> ConvertArtikelViewModelsToBestelRegels(WinkelwagenRijViewModel[] artikelen)
        {
            IList<BestelRegel> bestelRegels = new List<BestelRegel>();

            foreach (WinkelwagenRijViewModel winkelwagenRij in artikelen)
            {
                Artikel artikel = _artikelRepository.GetById(winkelwagenRij.Artikel.Id);

                if (artikel == null)
                {
                    throw new ArtikelDoesNotExistException(winkelwagenRij.Artikel.Id);
                }

                BestelRegel regel = new BestelRegel
                {
                    Aantal = winkelwagenRij.Aantal,
                    Leverancierscode = artikel.Leveranciercode,
                    StukPrijs = artikel.Prijs,
                    Naam = artikel.Naam,
                    ArtikelNummer = artikel.Artikelnummer,
                    AfbeeldingUrl = artikel.AfbeeldingUrl
                };
                bestelRegels.Add(regel);
            }
            return bestelRegels;
        }
    }
}
