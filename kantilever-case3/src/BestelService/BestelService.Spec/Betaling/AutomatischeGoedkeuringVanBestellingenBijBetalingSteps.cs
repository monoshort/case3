using BestelService.Core.Models;
using BestelService.Infrastructure.DAL;
using BestelService.Infrastructure.Repositories;
using BestelService.Services.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minor.Miffy.MicroServices.Events;
using Moq;
using TechTalk.SpecFlow;

namespace BestelService.Spec.Betaling
{
    [Binding]
    public class AutomatischeGoedkeuringVanBestellingenBijBetalingSteps
    {
        private Core.Models.Bestelling _ongekeurdeBestelling;
        private Core.Models.Bestelling _openstaandeBestelling;
        private Klant _klant = new Klant { Id = 1 };
        private static BestellingService _bestellingService;
        private static SqliteConnection _connection;
        private static DbContextOptions<BestelContext> _options;
        private static BestelContext _context;
        private static BestelRepository _repository;

        [Given(@"Er een goedgekeurde bestelling is met een openstaand bedrag van:  (.*)")]
        public void GivenErEenGoedgekeurdeBestellingIsMetEenOpenstaandBedragVan(decimal p0)
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();
            _options = new DbContextOptionsBuilder<BestelContext>()
                .UseSqlite(_connection).Options;

            _context = new BestelContext(_options);
            _repository = new BestelRepository(_context);
            _bestellingService = new BestellingService(_repository, new Mock<IEventPublisher>().Object);
            _context.Database.EnsureCreated();

            _context.Set<Core.Models.Bestelling>().RemoveRange(_context.Set<Core.Models.Bestelling>());
            _context.SaveChanges();

            _openstaandeBestelling = new Core.Models.Bestelling
            {
                Id = 1,
                BestellingNummer = "10001",
                SubtotaalInclusiefBtw = p0,
                OpenstaandBedrag = p0,
                Goedgekeurd = true,
                Klant = _klant
            };
            _repository.Add(_openstaandeBestelling);
        }

        [Given(@"En een bestaande ongekeurde bestelling is met een openstaand bedrag van: (.*)")]
        public void GivenEnEenBestaandeOngekeurdeBestellingIsMetEenOpenstaandBedragVan(decimal p0)
        {
            _ongekeurdeBestelling = new Core.Models.Bestelling
            {
                Id = 2,
                BestellingNummer = "10002",
                SubtotaalInclusiefBtw = p0,
                OpenstaandBedrag = p0,
                Goedgekeurd = false,
                Klant = _klant
            };
            _repository.Add(_ongekeurdeBestelling);
        }

        [When(@"Ik een betaling doorvoor van (.*) voor deze bestelling")]
        public void WhenIkEenBetalingDoorvoorVanVoorDezeBestelling(decimal p0)
        {
            _bestellingService.RegistreerBetaling(_openstaandeBestelling.BestellingNummer, p0);
        }

        [Then(@"Moet de ongekeurde bestaande bestelling '(.*)' goedgekeurd worden")]
        public void ThenMoetDeOngekeurdeBestaandeBestellingGoedgekeurdWorden(string p0)
        {
            var result = p0 == "wel";
            var bestelling = _repository.GetById(2);
            Assert.AreEqual(result, bestelling.Goedgekeurd);
            _context.Dispose();
            _connection.Close();
        }

    }
}
