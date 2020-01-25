using System.Collections.Generic;
using System.Linq;
using FrontendService.DAL;
using FrontendService.Models;
using FrontendService.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FrontendService.Test.Unit.Repositories
{
    [TestClass]
    public class BestellingRepositoryTest
    {
        private static SqliteConnection _connection;
        private static DbContextOptions<FrontendContext> _options;

        [ClassInitialize]
        public static void ClassInitialize(TestContext ctx)
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            _options = new DbContextOptionsBuilder<FrontendContext>()
                .UseSqlite(_connection)
                .Options;

            using FrontendContext context = new FrontendContext(_options);
            context.Database.EnsureCreated();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            _connection.Close();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            using FrontendContext context = new FrontendContext(_options);
            context.Artikelen.RemoveRange(context.Artikelen);
            context.Bestellingen.RemoveRange(context.Bestellingen);
            context.Klanten.RemoveRange(context.Klanten);
            context.SaveChanges();
        }

        [TestMethod]
        public void Add_AddsBestellingToDatabase()
        {
            using FrontendContext context = new FrontendContext(_options);
            BestellingRepository target = new BestellingRepository(context);

            // Arrange
            Klant k = new Klant() { Id = 1234, Naam = "Harry Slinger", Telefoonnummer = "4179561237"};
            Bestelling b = new Bestelling() {Afgekeurd = false, BestellingNummer = "4732651820", Ingepakt = false, Goedgekeurd = false, KlantId = 1234, Klant = k };

            // Act
            target.Add(b);


            // Assert
            using FrontendContext checkContext = new FrontendContext(_options);
            var result = checkContext.Bestellingen.Where(best => best.BestellingNummer == "4732651820");
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(1234, result.First().KlantId);
        }

        [TestMethod]
        public void GetById_GetsBestellingById()
        {
            using FrontendContext context = new FrontendContext(_options);
            BestellingRepository target = new BestellingRepository(context);

            // Arrange
            Klant k = new Klant { Id = 1234, Naam = "Harry Slinger", Telefoonnummer = "4179561237" };
            TestHelpers.InjectData(_options, k);
            Bestelling b = new Bestelling { Id = 2850, Afgekeurd = false, BestellingNummer = "4732651820", Ingepakt = false, Goedgekeurd = false, KlantId = 1234 };

            TestHelpers.InjectData(_options, b);

            // Act
            using FrontendContext checkContext = new FrontendContext(_options);
            var result = target.GetById(2850);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("4732651820", result.BestellingNummer);
        }

        [TestMethod]
        public void Update_UpdatesBestellingAccordingly()
        {
            using FrontendContext context = new FrontendContext(_options);
            BestellingRepository target = new BestellingRepository(context);

            // Arrange
            Klant k = new Klant { Id = 1234, Naam = "Harry Slinger", Telefoonnummer = "4179561237" };
            TestHelpers.InjectData(_options, k);
            Bestelling b = new Bestelling() { Id = 2850, Afgekeurd = false, BestellingNummer = "4732651820", Ingepakt = false, Goedgekeurd = false, KlantId = 1234 };

            TestHelpers.InjectData(_options, b);

            // Act
            Bestelling newBestelling = new Bestelling() { Id = 2850, Afgekeurd = false, BestellingNummer = "4732651820", Ingepakt = true, Goedgekeurd = true, KlantId = 1234 };
            target.Update(newBestelling);

            // Assert
            using FrontendContext checkContext = new FrontendContext(_options);
            var result = checkContext.Bestellingen.Find(2850L);
            Assert.IsNotNull(result);
            Assert.AreEqual("4732651820", result.BestellingNummer);
            Assert.AreEqual(true, result.Goedgekeurd);
            Assert.AreEqual(true, result.Ingepakt);
        }

        [TestMethod]
        public void IsEmpty_ReturnsFalseIfItemsExist()
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                Klant = new Klant(),
                AfleverAdres = new Adres()
            };

            TestHelpers.InjectData(_options, bestelling);

            using FrontendContext context = new FrontendContext(_options);
            BestellingRepository target = new BestellingRepository(context);

            // Act
            bool result = target.IsEmpty();

            // Assert
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void IsEmpty_ReturnsTrueIfEmpty()
        {
            // Arrange
            using FrontendContext context = new FrontendContext(_options);
            BestellingRepository target = new BestellingRepository(context);

            // Act
            bool result = target.IsEmpty();

            // Assert
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        [DataRow("fredje")]
        [DataRow("jandeman")]
        public void GetByKlantUsername_ReturnsBestellingenByKlant(string username)
        {
            // Arrange
            Bestelling[] testData =
            {
                new Bestelling
                {
                    Id = 1,
                    Klant = new Klant {Username = username}
                },
                new Bestelling
                {
                    Id = 2,
                    Klant = new Klant {Username = "PeterJan"}
                },
                new Bestelling
                {
                    Id = 3,
                    Klant = new Klant {Username = username}
                }
            };
            TestHelpers.InjectData(_options, testData);

            using FrontendContext context = new FrontendContext(_options);
            BestellingRepository target = new BestellingRepository(context);

            // Act
            IEnumerable<Bestelling> bestellingen = target.GetByKlantUsername(username).ToList();

            // Assert
            Assert.AreEqual(2, bestellingen.Count());
            Assert.IsNotNull(bestellingen.SingleOrDefault(e => e.Id == 1));
            Assert.IsNotNull(bestellingen.SingleOrDefault(e => e.Id == 3));

            Assert.IsNull(bestellingen.SingleOrDefault(e => e.Id == 2));
        }
    }
}
