using BackOfficeFrontendService.Controllers;
using BackOfficeFrontendService.DAL;
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
    [Scope(Scenario = "Er zijn geen goedgekeurde nog-in-te-pakken bestellingen")]
    public class ErZijnGeenGoedgekeurdeNogInTePakkenBestellingen
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

        [Given(@"Geen goedgekeurde nog-in-te-pakken bestellingen")]
        public void Given()
        {
        }

        [When(@"Ik de eerstvolgende bestelling pagina open")]
        public void When()
        {
            _actionResult =  _bestellingController.GetNextInpakBestelling();
        }

        [Then(@"Zie ik een melding dat er geen bestellingen zijn")]
        public void Then()
        {
            ViewResult page = _actionResult as ViewResult;
            Assert.AreEqual("NoBestellingOmInTePakken", page.ViewName);
        }
    }
}
