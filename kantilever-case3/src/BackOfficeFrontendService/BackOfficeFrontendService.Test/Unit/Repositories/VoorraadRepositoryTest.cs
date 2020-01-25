using System.Collections.Generic;
using System.Linq;
using BackOfficeFrontendService.DAL;
using BackOfficeFrontendService.Models;
using BackOfficeFrontendService.Repositories;
using BackOfficeFrontendService.Repositories.Abstractions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BackOfficeFrontendService.Test.Unit.Repositories
{
    [TestClass]
    public class VoorraadRepositoryTest
    {
        private static SqliteConnection _connection;
        private static DbContextOptions<BackOfficeContext> _options;

        [TestInitialize]
        public void TestInitialize()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();
            _options = new DbContextOptionsBuilder<BackOfficeContext>()
                .UseSqlite(_connection).Options;

            using BackOfficeContext context = new BackOfficeContext(_options);
            context.Database.EnsureCreated();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _connection.Close();
        }

        [TestMethod]
        [DataRow(10, 23)]
        [DataRow(402, 40)]
        public void Add_AddsVoorraadToDatabase(long artikelNummer, int amount)
        {
            // Arrange
            VoorraadMagazijn voorraadMagazijn = new VoorraadMagazijn
            {
                ArtikelNummer = artikelNummer,
                Voorraad = amount
            };

            using var context = new BackOfficeContext(_options);
            IVoorraadRepository repository = new VoorraadRepository(context);

            // Act
            repository.Add(voorraadMagazijn);

            // Assert
            using var resultContext = new BackOfficeContext(_options);
            VoorraadMagazijn resultVoorraadMagazijn = resultContext.VoorraadMagazijn.SingleOrDefault();

            Assert.IsNotNull(resultVoorraadMagazijn);
            Assert.AreEqual(artikelNummer, resultVoorraadMagazijn.ArtikelNummer);
            Assert.AreEqual(amount, resultVoorraadMagazijn.Voorraad);
        }

        [TestMethod]
        [DataRow(24524, 23, 22)]
        [DataRow(7673434, 40, 60)]
        public void Update_UpdatesVoorraadInDatabase(long artikelNummer, int amount, int newAmount)
        {
            // Arrange
            VoorraadMagazijn voorraadMagazijn = new VoorraadMagazijn
            {
                ArtikelNummer = artikelNummer,
                Voorraad = amount
            };
            TestHelpers.InjectData(_options, voorraadMagazijn);

            using var context = new BackOfficeContext(_options);
            IVoorraadRepository repository = new VoorraadRepository(context);

            VoorraadMagazijn newVoorraadMagazijn = context.VoorraadMagazijn.First();
            newVoorraadMagazijn.Voorraad = newAmount;

            // Act
            repository.Update(newVoorraadMagazijn);

            // Assert
            using var resultContext = new BackOfficeContext(_options);
            VoorraadMagazijn resultVoorraadMagazijn = resultContext.VoorraadMagazijn.SingleOrDefault();

            Assert.IsNotNull(resultVoorraadMagazijn);
            Assert.AreEqual(artikelNummer, resultVoorraadMagazijn.ArtikelNummer);
            Assert.AreEqual(newAmount, resultVoorraadMagazijn.Voorraad);
        }

        [TestMethod]
        public void IsEmpty_IsFalseIfDatabaseContainsVoorraad()
        {
            // Arrange
            VoorraadMagazijn voorraadMagazijn = new VoorraadMagazijn();
            TestHelpers.InjectData(_options, voorraadMagazijn);

            using var context = new BackOfficeContext(_options);
            IVoorraadRepository repository = new VoorraadRepository(context);

            // Act
            bool result = repository.IsEmpty();

            // Assert
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void IsEmpty_IsTrueIfDatabaseIsEmpty()
        {
            // Arrange
            using var context = new BackOfficeContext(_options);
            IVoorraadRepository repository = new VoorraadRepository(context);

            // Act
            bool result = repository.IsEmpty();

            // Assert
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        [DataRow(20)]
        [DataRow(502)]
        public void GetByArtikelNummer_GetsVoorraadByArtikelNummer(long artikelNummer)
        {
            // Arrange
            VoorraadMagazijn voorraadMagazijn =  new VoorraadMagazijn
            {
                ArtikelNummer = artikelNummer,
                Voorraad = 5
            };
            TestHelpers.InjectData(_options, voorraadMagazijn);

            using BackOfficeContext context = new BackOfficeContext(_options);
            IVoorraadRepository voorraadRepository = new VoorraadRepository(context);

            // Act
            VoorraadMagazijn result = voorraadRepository.GetByArtikelNummer(artikelNummer);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.Voorraad);
            Assert.AreEqual(artikelNummer, result.ArtikelNummer);
        }

        [TestMethod]
        [DataRow(20)]
        [DataRow(502)]
        public void GetByArtikelNummer_ReturnsNullOnNotFound(long artikelNummer)
        {
            // Arrange
            using BackOfficeContext context = new BackOfficeContext(_options);
            IVoorraadRepository voorraadRepository = new VoorraadRepository(context);

            // Act
            VoorraadMagazijn result = voorraadRepository.GetByArtikelNummer(artikelNummer);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetArtikelenNietOpVoorraad_ReturnsArtikelenNietOpVoorraad()
        {
            // Arrange
            VoorraadMagazijn[] voorraadData = {
                new VoorraadMagazijn {
                    ArtikelNummer = 1,
                    Voorraad = 5,
                    BestelRegels =
                    {
                        new BestelRegel { Aantal = 5, ArtikelNummer = 1, Bestelling = new Bestelling() },
                        new BestelRegel { Aantal = 5, ArtikelNummer = 1, Bestelling = new Bestelling() }
                    }
                },
                new VoorraadMagazijn
                {
                    ArtikelNummer = 2,
                    Voorraad = 2,
                    BestelRegels =
                    {
                        new BestelRegel { Aantal = 3, ArtikelNummer = 2, Bestelling = new Bestelling() }
                    }
                },
                new VoorraadMagazijn
                {
                    ArtikelNummer = 3,
                    Voorraad = 12,
                    BestelRegels =
                    {
                        new BestelRegel { Aantal = 4, ArtikelNummer = 3, Bestelling = new Bestelling() },
                        new BestelRegel { Aantal = 4, ArtikelNummer = 3, Bestelling = new Bestelling() }
                    }
                }
            };

            TestHelpers.InjectData(_options, voorraadData);

            using BackOfficeContext context = new BackOfficeContext(_options);
            IVoorraadRepository voorraadRepository = new VoorraadRepository(context);

            // Act
            IEnumerable<VoorraadMagazijn> result = voorraadRepository.GetArtikelenNietOpVoorraad()
                .ToList();

            // Assert
            Assert.AreEqual(2, result.Count());

            Assert.IsNotNull(result.Single(v => v.ArtikelNummer == 1));
            Assert.IsNotNull(result.Single(v => v.ArtikelNummer == 2));
            Assert.IsNull(result.SingleOrDefault(v => v.ArtikelNummer == 3));
        }

        [TestMethod]
        public void GetArtikelenNietOpVoorraad_ReturnsOnlyNietKlaarGemeldeBestellingen()
        {
            // Arrange
            VoorraadMagazijn[] voorraadData = {
                new VoorraadMagazijn {
                    ArtikelNummer = 1,
                    Voorraad = 5,
                    BestelRegels =
                    {
                        new BestelRegel { Aantal = 5, ArtikelNummer = 1, Bestelling = new Bestelling { KlaarGemeld = true } },
                        new BestelRegel { Aantal = 6, ArtikelNummer = 1, Bestelling = new Bestelling() }
                    }
                },
                new VoorraadMagazijn
                {
                    ArtikelNummer = 2,
                    Voorraad = 2,
                    BestelRegels =
                    {
                        new BestelRegel { Aantal = 3, ArtikelNummer = 2, Bestelling = new Bestelling { KlaarGemeld = true } }
                    }
                }
            };

            TestHelpers.InjectData(_options, voorraadData);

            using BackOfficeContext context = new BackOfficeContext(_options);
            IVoorraadRepository voorraadRepository = new VoorraadRepository(context);

            // Act
            IEnumerable<VoorraadMagazijn> result = voorraadRepository.GetArtikelenNietOpVoorraad()
                .ToList();

            // Assert
            Assert.AreEqual(1, result.Count());

            Assert.IsNotNull(result.Single(v => v.ArtikelNummer == 1));
            Assert.IsNull(result.SingleOrDefault(v => v.ArtikelNummer == 2));
        }

        [TestMethod]
        public void GetArtikelenNietOpVoorraad_DoesNotReturnBijbesteldeVoorraad()
        {
            // Arrange
            VoorraadMagazijn[] voorraadData = {
                new VoorraadMagazijn {
                    ArtikelNummer = 1,
                    Voorraad = 5,
                    VoorraadBesteld = true,
                    BestelRegels =
                    {
                        new BestelRegel { Aantal = 5, ArtikelNummer = 1, Bestelling = new Bestelling { KlaarGemeld = true } },
                        new BestelRegel { Aantal = 6, ArtikelNummer = 1, Bestelling = new Bestelling() }
                    }
                }
            };

            TestHelpers.InjectData(_options, voorraadData);

            using BackOfficeContext context = new BackOfficeContext(_options);
            IVoorraadRepository voorraadRepository = new VoorraadRepository(context);

            // Act
            IEnumerable<VoorraadMagazijn> result = voorraadRepository.GetArtikelenNietOpVoorraad()
                .ToList();

            // Assert
            Assert.AreEqual(0, result.Count());
        }
    }
}
