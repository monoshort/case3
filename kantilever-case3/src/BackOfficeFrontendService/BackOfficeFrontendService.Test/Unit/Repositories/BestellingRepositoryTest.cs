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
    public class BestellingRepositoryTest
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

        /// <summary>
        ///     Returns dummy bestelling
        /// </summary>
        private Bestelling GenerateBestelling(string bestellingNummer)
        {
            return new Bestelling
            {
                Id = 54,
                BestellingNummer = bestellingNummer,
                AfleverAdres = new Adres
                {
                    Postcode = "3584 CJ",
                    StraatnaamHuisnummer = "Bolognalaan 101",
                    Woonplaats = "Utrecht"
                },
                KlaarGemeld = false,
                Goedgekeurd = true,
                Klant = new Klant
                {
                    Id = 66,
                    Factuuradres = new Adres
                    {
                        Postcode = "3584 CJ",
                        StraatnaamHuisnummer = "Bolognalaan 101",
                        Woonplaats = "Utrecht"
                    },
                    Naam = "Piet Bos",
                    Telefoonnummer = "06123456678"
                },
                BestelRegels =
                {
                    new BestelRegel
                    {
                        Id = 2345,
                        Aantal = 1,
                        Ingepakt = false,
                        Leverancierscode = "rewr43",
                        Naam = "Tandwiel",
                        AfbeeldingUrl = "tandwiel.jpg"
                    }
                }
            };
        }

        [TestMethod]
        public void GetVolgendeInpakOpdracht_ReturnsNullOnNoItems()
        {
            // Arrange
            using BackOfficeContext context = new BackOfficeContext(_options);
            IBestellingRepository repository = new BestellingRepository(context);

            // Act
            Bestelling result = repository.GetVolgendeInpakOpdracht();

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        [DataRow("123ABC")]
        [DataRow("E24232")]
        public void GetVolgendeInpakOpdracht_ReturnsItemOnOneItem(string bestellingNummer)
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                Id = 100,
                KlaarGemeld = false,
                BestellingNummer = bestellingNummer,
                Goedgekeurd = true
            };

            TestHelpers.InjectData(_options, bestelling);

            using BackOfficeContext context = new BackOfficeContext(_options);
            IBestellingRepository repository = new BestellingRepository(context);

            // Act
            Bestelling result = repository.GetVolgendeInpakOpdracht();

            // Assert
            Assert.AreEqual(bestelling.Id, result.Id);
            Assert.AreEqual(bestelling.KlaarGemeld, result.KlaarGemeld);
            Assert.AreEqual(bestelling.BestellingNummer, result.BestellingNummer);
        }

        [TestMethod]
        [DataRow("123ABC")]
        [DataRow("E24232")]
        public void GetVolgendeInpakOpdracht_ReturnsSameItem(string bestellingNummer)
        {
            // Arrange
            Bestelling bestelling0 = new Bestelling
            {
                Id = 100,
                KlaarGemeld = true,
                BestellingNummer = bestellingNummer,
                Goedgekeurd = false
            };


            Bestelling bestelling1 = new Bestelling
            {
                Id = 101,
                Goedgekeurd = false
            };

            Bestelling bestelling2 = new Bestelling
            {
                Id = 102,
                Goedgekeurd = false
            };

            TestHelpers.InjectData(_options, bestelling0, bestelling1, bestelling2);

            using BackOfficeContext context = new BackOfficeContext(_options);
            IBestellingRepository repository = new BestellingRepository(context);

            // Act
            Bestelling result0 = repository.GetVolgendeInpakOpdracht();
            Bestelling result1 = repository.GetVolgendeInpakOpdracht();

            // Assert
            Assert.AreEqual(result0, result1);
        }

        [TestMethod]
        [DataRow("123ABC")]
        [DataRow("E24232")]
        public void GetVolgendeInpakOpdracht_DoesNotReturnAlreadyIngepakteItems(string bestellingNummer)
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                Id = 100,
                KlaarGemeld = true,
                BestellingNummer = bestellingNummer,
                Goedgekeurd = true
            };

            TestHelpers.InjectData(_options, bestelling);

            using BackOfficeContext context = new BackOfficeContext(_options);
            IBestellingRepository repository = new BestellingRepository(context);

            // Act
            Bestelling result = repository.GetVolgendeInpakOpdracht();

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        [DataRow("123ABC")]
        [DataRow("E24232")]
        public void GetVolgendeInpakOpdracht_DoesNotReturnNietGoedgekeurdeItems(string bestellingNummer)
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                Id = 100,
                KlaarGemeld = false,
                BestellingNummer = bestellingNummer,
                Goedgekeurd = false
            };

            TestHelpers.InjectData(_options, bestelling);

            using BackOfficeContext context = new BackOfficeContext(_options);
            IBestellingRepository repository = new BestellingRepository(context);

            // Act
            Bestelling result = repository.GetVolgendeInpakOpdracht();

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        [DataRow("123ABC")]
        [DataRow("E24232")]
        public void GetVolgendeInpakOpdracht_DoesNotReturnNietOpVoorraadItems(string bestellingNummer)
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                Id = 100,
                BestellingNummer = bestellingNummer,
                Goedgekeurd = true,
                BestelRegels =
                {
                    new BestelRegel { Aantal = 5, Voorraad = new VoorraadMagazijn { Voorraad = 2 }}
                }
            };

            TestHelpers.InjectData(_options, bestelling);

            using BackOfficeContext context = new BackOfficeContext(_options);
            IBestellingRepository repository = new BestellingRepository(context);

            // Act
            Bestelling result = repository.GetVolgendeInpakOpdracht();

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        [DataRow(1231211)]
        [DataRow(1451515)]
        [DataRow(1547845)]
        public void GetInpakOpdrachtMetBestelNummer_ReturnsNullOnNotFound(long bestellingId)
        {
            // Arrange
            using BackOfficeContext context = new BackOfficeContext(_options);
            IBestellingRepository repository = new BestellingRepository(context);

            // Act
            Bestelling result = repository.GetInpakOpdrachtMetId(bestellingId);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        [DataRow(1231211, "234233", "Jan willem de Vries", "0612345678", "Warande 111", "3329mk", "Dordrecht", 100, 79)]
        [DataRow(126145, "324324", "Maria Tibet", "068412845", "Blauwweg 110", "5488kk", "Vlaardingen", 200, 158)]
        [DataRow(25231253, "23532532", "Freek van Oorschot", "0632543255", "Heidelberglaan 41", "9731pq", "Rotterdam", 400, 312)]
        public void GetInpakOpdrachtMetBestelNummer_ReturnsBestelling(long bestellingId, string bestelnummer,
            string klantNaam, string telefoonnummer, string straatnaamHuisNummer, string postcode, string woonplaats,
            int subtotaal, int subtotaalInclBtw)
        {
            // Arrange
            var bestelling = new Bestelling
            {
                Id = bestellingId,
                Klant = new Klant
                {
                    Id = 0,
                    Naam = klantNaam,
                    Factuuradres = new Adres
                    {
                        StraatnaamHuisnummer = straatnaamHuisNummer,
                        Postcode = postcode,
                        Woonplaats = woonplaats
                    },
                    Telefoonnummer = telefoonnummer
                },
                BestellingNummer = bestelnummer,
                Goedgekeurd = false,
                KlaarGemeld = false,
                AfleverAdres = new Adres
                {
                    StraatnaamHuisnummer = straatnaamHuisNummer,
                    Postcode = postcode,
                    Woonplaats = woonplaats
                },
                Subtotaal = subtotaal,
                SubtotaalInclusiefBtw = subtotaalInclBtw,
                BestelDatum = default,
                Afgekeurd = false,
                AdresLabelGeprint = false,
                FactuurGeprint = false
            };

            TestHelpers.InjectData(_options, bestelling);

            using var context = new BackOfficeContext(_options);
            IBestellingRepository repository = new BestellingRepository(context);

            // Act
            var result = repository.GetInpakOpdrachtMetId(bestellingId);

            // Assert
            Assert.AreEqual(bestelling.Id, result.Id);
            Assert.AreEqual(bestelling.BestellingNummer, result.BestellingNummer);
            Assert.AreEqual(bestelling.Klant.Naam, result.Klant.Naam);
            Assert.AreEqual(bestelling.Klant.Telefoonnummer, result.Klant.Telefoonnummer);
            Assert.AreEqual(bestelling.Klant.Factuuradres.Postcode, result.Klant.Factuuradres.Postcode);
            Assert.AreEqual(bestelling.Klant.Factuuradres.StraatnaamHuisnummer, result.Klant.Factuuradres.StraatnaamHuisnummer);
            Assert.AreEqual(bestelling.Klant.Factuuradres.Woonplaats, result.Klant.Factuuradres.Woonplaats);
            Assert.AreEqual(bestelling.AfleverAdres.StraatnaamHuisnummer, result.AfleverAdres.StraatnaamHuisnummer);
            Assert.AreEqual(bestelling.AfleverAdres.Postcode, result.AfleverAdres.Postcode);
            Assert.AreEqual(bestelling.AfleverAdres.Woonplaats, result.AfleverAdres.Woonplaats);
            Assert.AreEqual(bestelling.Subtotaal, result.Subtotaal);
            Assert.AreEqual(bestelling.SubtotaalInclusiefBtw, result.SubtotaalInclusiefBtw);
        }

        [TestMethod]
        [DataRow("123ABC")]
        [DataRow("E24232")]
        public void Add_ShouldAddNewBestelling(string bestellingNummer)
        {
            // Arrange
            Bestelling bestelling = GenerateBestelling(bestellingNummer);

            using BackOfficeContext context = new BackOfficeContext(_options);
            IBestellingRepository repository = new BestellingRepository(context);

            // Act
            repository.Add(bestelling);

            // Assert
            using BackOfficeContext assertContext = new BackOfficeContext(_options);
            Assert.AreEqual(1, assertContext.Bestellingen.Count());
            Assert.IsNotNull(assertContext.Bestellingen.Single(x => x.BestellingNummer == bestellingNummer));
        }

        [TestMethod]
        [DataRow("123ABC")]
        [DataRow("E24232")]
        public void Add_ShouldAddBestelRegel(string bestellingNummer)
        {
            // Arrange
            Bestelling bestelling = GenerateBestelling(bestellingNummer);

            using BackOfficeContext context = new BackOfficeContext(_options);
            IBestellingRepository repository = new BestellingRepository(context);

            // Act
            repository.Add(bestelling);

            // Assert
            using BackOfficeContext assertContext = new BackOfficeContext(_options);
            Bestelling bestellingInDb = assertContext.Bestellingen
                .Include(x => x.BestelRegels)
                .First();

            Assert.AreEqual(1, bestellingInDb.BestelRegels.Count);

            Assert.IsNotNull(bestellingInDb.Id);
            Assert.AreEqual(bestelling.BestelRegels.First().Naam, bestellingInDb.BestelRegels.First().Naam);
        }

        [TestMethod]
        public void GetNietGekeurdeBestellingen_GetOnlyNietGoedgekeurdeBestellingen()
        {
            // Arrange
            Bestelling[] bestellingen =
            {
                new Bestelling { Goedgekeurd = false, BestellingNummer = "200000"},
                new Bestelling { Goedgekeurd = false, BestellingNummer = "210000"},
                new Bestelling { Goedgekeurd = false, BestellingNummer = "230000"},
                new Bestelling { Afgekeurd = true, BestellingNummer = "220000"},
                new Bestelling { Afgekeurd = true, BestellingNummer = "270000"},
                new Bestelling { Goedgekeurd = true, BestellingNummer = "256000"},
                new Bestelling { Goedgekeurd = true, BestellingNummer = "320040"},
            };

            TestHelpers.InjectData(_options, bestellingen);

            using BackOfficeContext context = new BackOfficeContext(_options);
            IBestellingRepository repository = new BestellingRepository(context);

            // Act
            IEnumerable<Bestelling> result = repository.GetNietGekeurdeBestellingen();

            // Assert
            Assert.AreEqual(3, result.Count());
            string[] resultNummers = bestellingen.Select(e => e.BestellingNummer).ToArray();

            Assert.IsTrue(resultNummers.Contains("230000"));
            Assert.IsTrue(resultNummers.Contains("256000"));
            Assert.IsTrue(resultNummers.Contains("320040"));
        }

        [TestMethod]
        [DataRow("Jan de Ven", "Zwolle", "ED-232")]
        [DataRow("Bart de Kampioen", "Leeuwarden", "OE-232")]
        public void GetNietGekeurdeBestellingen_GetsBestelRegelsAndCustomerData(string klantNaam, string woonplaats, string code)
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                KlaarGemeld = false,
                Klant = new Klant
                {
                    Naam = klantNaam,
                    Factuuradres = new Adres
                    {
                        Woonplaats = woonplaats
                    }
                },
                BestelRegels =
                {
                    new BestelRegel { Leverancierscode = code }
                }
            };

            TestHelpers.InjectData(_options, bestelling);

            using BackOfficeContext context = new BackOfficeContext(_options);
            IBestellingRepository repository = new BestellingRepository(context);

            // Act
            IEnumerable<Bestelling> result = repository.GetNietGekeurdeBestellingen();

            // Assert
            Bestelling resultBestelling = result.First();

            Assert.AreEqual(klantNaam, resultBestelling.Klant.Naam);
            Assert.AreEqual(woonplaats, resultBestelling.Klant.Factuuradres.Woonplaats);
            Assert.AreEqual(code, resultBestelling.BestelRegels.First().Leverancierscode);
        }

        [TestMethod]
        [DataRow(20.00, 50.00, "1200", "1300")]
        [DataRow(30.00, 90.00, "5000", "2900")]
        public void Update_UpdatesBestellingData(double subtotaal, double newSubtotaal, string nummer, string newNummer)
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                Id = 2,
                BestellingNummer = nummer,
                Subtotaal = (decimal) subtotaal,
            };
            TestHelpers.InjectData(_options, bestelling);

            using BackOfficeContext context = new BackOfficeContext(_options);
            IBestellingRepository repository = new BestellingRepository(context);

            Bestelling newBestelling = context.Bestellingen.Single(e => e.Id == 2);

            newBestelling.BestellingNummer = newNummer;
            newBestelling.Subtotaal = (decimal) newSubtotaal;

            // Act
            repository.Update(newBestelling);

            // Assert
            using BackOfficeContext resultContext = new BackOfficeContext(_options);
            Bestelling resultBestelling = resultContext.Bestellingen.First();
            Assert.AreEqual((decimal) newSubtotaal, resultBestelling.Subtotaal);
            Assert.AreEqual(newNummer, resultBestelling.BestellingNummer);
        }

        [TestMethod]
        [DataRow("2348328")]
        [DataRow("23424249")]
        public void GetBestellingByBestellingNummer_ReturnsBestelling(string bestellingNummer)
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                BestellingNummer = bestellingNummer
            };
            TestHelpers.InjectData(_options, bestelling);

            using BackOfficeContext context = new BackOfficeContext(_options);
            IBestellingRepository bestellingRepository = new BestellingRepository(context);

            // Act
            Bestelling result = bestellingRepository.GetBestellingByBestellingNummer(bestellingNummer);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(bestellingNummer, result.BestellingNummer);
        }

        [TestMethod]
        [DataRow("2348328")]
        [DataRow("23424249")]
        public void GetBestellingByBestellingNummer_ReturnsNullOnNotFound(string bestellingNummer)
        {
            // Arrange
            using BackOfficeContext context = new BackOfficeContext(_options);
            IBestellingRepository bestellingRepository = new BestellingRepository(context);

            // Act
            Bestelling result = bestellingRepository.GetBestellingByBestellingNummer(bestellingNummer);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void IsEmpty_IsFalseIfDatabaseContainsBestellingen()
        {
            // Arrange
            Bestelling bestelling = new Bestelling();
            TestHelpers.InjectData(_options, bestelling);

            using var context = new BackOfficeContext(_options);
            IBestellingRepository repository = new BestellingRepository(context);

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
            IBestellingRepository repository = new BestellingRepository(context);

            // Act
            bool result = repository.IsEmpty();

            // Assert
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void GetWanbetaalBestellingen_ReturnsAllBestellingenWithWanbetalers()
        {
            // Arrange
            Bestelling bestelling1 = new Bestelling
            {
                BestellingNummer = "0",
                IsKlantWanbetaler = false
            };

            Bestelling bestelling2 = new Bestelling
            {
                BestellingNummer = "1",
                IsKlantWanbetaler = true
            };
            TestHelpers.InjectData(_options, bestelling1, bestelling2);

            using var context = new BackOfficeContext(_options);
            IBestellingRepository repository = new BestellingRepository(context);

            // Act
            IEnumerable<Bestelling> result = repository.GetWanbetaalBestellingen().ToList();

            // Assert
            Assert.AreEqual(1, result.Count());

            Bestelling firstItem = result.First();
            Assert.AreEqual("1", firstItem.BestellingNummer);
            Assert.AreEqual(true, firstItem.IsKlantWanbetaler);
        }
    }
}
