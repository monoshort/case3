using System.Linq;
using FrontendService.Models;
using FrontendService.Repositories.Abstractions;
using FrontendService.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FrontendService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtikelController : Controller
    {
        private readonly IArtikelRepository _artikelRepository;

        public ArtikelController(IArtikelRepository artikelRepository)
        {
            _artikelRepository = artikelRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var artikelen = _artikelRepository.GetAll();
            var result = artikelen.Select(artikel =>
                new ArtikelViewModel
                {
                    Id = artikel.Id,
                    BeschikbaarAantal = artikel.Voorraad,
                    AfbeeldingUrl = artikel.AfbeeldingUrl,
                    Naam = artikel.Naam,
                    Prijs = artikel.Prijs,
                    PrijsInclBtw = artikel.PrijsInclBtw,
                    Beschrijving = artikel.Beschrijving,
                    Categorie = artikel.Categorie,
            }).ToList();

            return Json(result);
        }

        [HttpGet]
        [Route("{artikelId}")]
        public IActionResult Get(long artikelId)
        {
            Artikel artikel =  _artikelRepository.GetById(artikelId);
            if (artikel == null)
            {
                return NotFound();
            }
            return Json(new ArtikelDetailViewModel 
            { 
                AfbeeldingUrl = artikel.AfbeeldingUrl,
                Artikelnummer = artikel.Artikelnummer,
                Beschrijving = artikel.Beschrijving,
                Categorie = artikel.Categorie,
                Id = artikel.Id,
                Leverancier = artikel.Leverancier,
                Leveranciercode = artikel.Leveranciercode,
                LeverbaarTot = artikel.LeverbaarTot,
                LeverbaarVanaf = artikel.LeverbaarVanaf,
                Naam = artikel.Naam,
                Prijs = artikel.Prijs,
                PrijsInclBtw = artikel.PrijsInclBtw,
                SubCategorie = artikel.SubCategorie,
                Voorraad = artikel.Voorraad
            });
        }
    }
}
