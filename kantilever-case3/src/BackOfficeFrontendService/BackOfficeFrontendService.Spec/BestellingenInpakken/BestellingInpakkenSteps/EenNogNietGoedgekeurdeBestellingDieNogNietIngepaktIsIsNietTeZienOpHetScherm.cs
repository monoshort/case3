using BackOfficeFrontendService.Controllers;
using BackOfficeFrontendService.DAL;
using BackOfficeFrontendService.Models;
using BackOfficeFrontendService.Repositories;
using BackOfficeFrontendService.Repositories.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace BackOfficeFrontendService.Spec.BestellingenInpakken.BestellingInpakkenSteps
{
    [Binding]
    [Scope(Scenario = "Een nog niet goedgekeurde bestelling die nog niet ingepakt is, is niet te zien op het scherm")]
    public class EenNogNietGoedgekeurdeBestellingDieNogNietIngepaktIsIsNietTeZienOpHetScherm
    {
        private static SqliteConnection _connection;
        private static DbContextOptions<BackOfficeContext> _options;
        private static BackOfficeContext _context;

        private static IBestellingRepository _bestellingRepository;
        private static BestellingController _bestellingController;

        private IActionResult _actionResult;

        [BeforeScenario]
        public static void BeforeScenario()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();
            _options = new DbContextOptionsBuilder<BackOfficeContext>()
                .UseSqlite(_connection).Options;

            _context = new BackOfficeContext(_options);
            _context.Database.EnsureCreated();

            _bestellingRepository = new BestellingRepository(_context);
            _bestellingController = new BestellingController(_bestellingRepository, null);
        }

        [AfterScenario]
        public static void AfterScenario()
        {
            _context.Dispose();
            _connection.Close();
        }

        [Given(@"Een niet goedgekeurde bestelling die nog niet ingepakt is")]
        public void GivenEenNietGoedgekeurdeBestellingDieNogNietIngepaktIs()
        {
            Bestelling bestelling = new Bestelling
            {
                BestellingNummer = "10492",
                KlaarGemeld = false,
                Goedgekeurd = false
            };

            TestHelpers.InjectData(_options, bestelling);
        }

        [When(@"Ik de eerstvolgende bestelling pagina open")]
        public void WhenIkDeEerstvolgendeBestellingPaginaOpen()
        {
            _actionResult =  _bestellingController.GetNextInpakBestelling();
        }

        [Then(@"Zie ik een melding dat er geen bestellingen zijn")]
        public void ThenZieIkEenMeldingDatErGeenBestellingenZijn()
        {
            ViewResult page = _actionResult as ViewResult;
            Assert.AreEqual("NoBestellingOmInTePakken", page.ViewName);
        }
    }
}
