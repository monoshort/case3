using System;
using System.Collections.Generic;
using System.Linq;
using BestelService.Core.Models;
using BestelService.Core.Repositories;
using BestelService.Infrastructure.DAL;
using BestelService.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BestelService.Infrastructure.Test.Unit.Repositories
{
    [TestClass]
    public class BestelRepositoryTest
    {
        private static SqliteConnection _connection;
        private static DbContextOptions<BestelContext> _options;

        [ClassInitialize]
        public static void ClassInitialize(TestContext tc)
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();
            _options = new DbContextOptionsBuilder<BestelContext>()
                .UseSqlite(_connection).Options;

            using var context = new BestelContext(_options);
            context.Database.EnsureCreated();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            _connection.Close();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            using var context = new BestelContext(_options);
            context.Set<Bestelling>().RemoveRange(context.Set<Bestelling>());
            context.SaveChanges();
        }

        [TestMethod]
        [DataRow("Peter Vrind")]
        [DataRow("Sebas Jackes")]
        public void Add_AddsBestellingToDatabase(string klantNaam)
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                Klant = new Klant { Naam = klantNaam }
            };

            using BestelContext bestelContext = new BestelContext(_options);
            IBestelRepository repository = new BestelRepository(bestelContext);

            // Act
            repository.Add(bestelling);

            // Assert
            using BestelContext resultContext = new BestelContext(_options);
            Assert.AreEqual(1, resultContext.Bestellingen.Count());

            Bestelling resultBestelling = resultContext.Bestellingen.Include(e => e.Klant)
                .First();

            Assert.AreEqual(klantNaam, resultBestelling.Klant.Naam);
        }

        [TestMethod]
        [DataRow("Peter Vrind", "Sebas Jackes")]
        [DataRow("Sebas Jackes", "Peter Vrind")]
        public void Update_UpdatesBestellingData(string klantNaam, string newKlantNaam)
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                Klant = new Klant { Naam = klantNaam }
            };

            TestHelpers.InjectData(_options, bestelling);

            using BestelContext bestelContext = new BestelContext(_options);
            IBestelRepository repository = new BestelRepository(bestelContext);

            Bestelling savedBestelling = bestelContext.Bestellingen.Include(e => e.Klant).First();
            savedBestelling.Klant.Naam = newKlantNaam;

            // Act
            repository.Update(bestelling);

            // Assert
            using BestelContext resultContext = new BestelContext(_options);
            Assert.AreEqual(1, resultContext.Bestellingen.Count());

            Bestelling resultBestelling = resultContext.Bestellingen.Include(e => e.Klant)
                .First();

            Assert.AreEqual(newKlantNaam, resultBestelling.Klant.Naam);
        }

        [TestMethod]
        [DataRow(2, 240.00)]
        [DataRow(3536, 5000.00)]
        public void GetById_GetBestellingById(int id, double subTotaal)
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                Id = id,
                Subtotaal = (decimal)subTotaal
            };

            TestHelpers.InjectData(_options, bestelling);

            using BestelContext context = new BestelContext(_options);
            IBestelRepository repository = new BestelRepository(context);

            // Act
            Bestelling resultBestelling = repository.GetById(id);

            // Assert
            Assert.IsNotNull(resultBestelling);
            Assert.AreEqual((decimal)subTotaal, resultBestelling.Subtotaal);
        }

        [TestMethod]
        [DataRow(1, "10001", 240.00)]
        [DataRow(2, "10002", 5000.00)]
        public void GetByBestellingNummer_GetBestellingByBestellingNummer(int id, string bestellingNummer, double subTotaal)
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                Id = id,
                Subtotaal = (decimal)subTotaal,
                BestellingNummer = bestellingNummer
            };

            TestHelpers.InjectData(_options, bestelling);

            using BestelContext context = new BestelContext(_options);
            IBestelRepository repository = new BestelRepository(context);

            // Act
            Bestelling resultBestelling = repository.GetByBestellingNummer(bestellingNummer);

            // Assert
            Assert.IsNotNull(resultBestelling);
            Assert.AreEqual((decimal)subTotaal, resultBestelling.Subtotaal);
        }

        [TestMethod]
        [DataRow("10001", 240.00)]
        [DataRow("10002", 5000.00)]
        public void GetByBestellingNummer_ThrowInvalidOperationExceptionWhenNotFound(string bestellingNummer, double subTotaal)
        {
            // Arrange
            using BestelContext context = new BestelContext(_options);
            IBestelRepository repository = new BestelRepository(context);

            // Act
            Action action = () => repository.GetByBestellingNummer(bestellingNummer);

            // Assert
            Assert.ThrowsException<InvalidOperationException>(action);
        }

        [TestMethod]
        public void GetMostRecentUnassessedBestelling_ReturnsTheMostRecentBestelling()
        {
            // Arrange
            var bestellingen = new Bestelling[] {
                new Bestelling
                {
                    Id = 1,
                    Subtotaal = 200,
                    BestelDatum = DateTime.Now.AddDays(-2),
                    BestellingNummer = "10001"
                },
                new Bestelling
                {
                    Id = 2,
                    Subtotaal = 200,
                    BestelDatum = DateTime.Now.AddDays(-1),
                    BestellingNummer = "10002"
                },
            };
            TestHelpers.InjectData(_options, bestellingen);

            using BestelContext context = new BestelContext(_options);
            IBestelRepository repository = new BestelRepository(context);

            // Act
            var bestelling = repository.GetMostRecentUnassessedBestelling();

            // Assert
            Assert.AreEqual("10002", bestelling.BestellingNummer);
        }

        [TestMethod]
        public void GetMostRecentUnassessedBestelling_ReturnsTheMostRecentUnassessedBestelling()
        {
            // Arrange
            var bestellingen = new Bestelling[] {
                new Bestelling
                {
                    Id = 1,
                    Subtotaal = 200,
                    BestelDatum = DateTime.Now.AddDays(-3),
                    BestellingNummer = "10001"
                },
                new Bestelling
                {
                    Id = 2,
                    Subtotaal = 200,
                    BestelDatum = DateTime.Now.AddDays(-2),
                    BestellingNummer = "10002"
                },
                new Bestelling
                {
                    Id = 3,
                    Subtotaal = 200,
                    Goedgekeurd = true,
                    BestelDatum = DateTime.Now.AddDays(-1),
                    BestellingNummer = "10003"
                },
                new Bestelling
                {
                    Id = 4,
                    Subtotaal = 200,
                    Goedgekeurd = false,
                    Afgekeurd = true,
                    BestelDatum = DateTime.Now,
                    BestellingNummer = "10004"
                },
            };
            TestHelpers.InjectData(_options, bestellingen);

            using BestelContext context = new BestelContext(_options);
            IBestelRepository repository = new BestelRepository(context);

            // Act
            var bestelling = repository.GetMostRecentUnassessedBestelling();

            // Assert
            Assert.AreEqual("10002", bestelling.BestellingNummer);
        }

        [TestMethod]
        public void GetAll_ReturnsEmptyListOnNoBestellingen()
        {
            // Arrange
            using BestelContext context = new BestelContext(_options);
            IBestelRepository repository = new BestelRepository(context);

            // Act
            IEnumerable<Bestelling> result = repository.GetAll();

            // Assert
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void GetAll_ReturnsAllBestellingen()
        {
            // Arrange
            Bestelling[] bestellingen =
            {
                new Bestelling
                {
                    BestellingNummer = "2323"
                },
                new Bestelling
                {
                    BestellingNummer = "2566"
                },
                new Bestelling
                {
                    BestellingNummer = "7284"
                }
            };
            TestHelpers.InjectData(_options, bestellingen);

            using BestelContext context = new BestelContext(_options);
            IBestelRepository repository = new BestelRepository(context);

            // Act
            IEnumerable<Bestelling> result = repository.GetAll().ToList();

            // Assert
            Assert.AreEqual(3, result.Count());
            Assert.IsNotNull(result.Single(e => e.BestellingNummer == "2323"));
            Assert.IsNotNull(result.Single(e => e.BestellingNummer == "2566"));
            Assert.IsNotNull(result.Single(e => e.BestellingNummer == "7284"));
        }
    }
}
