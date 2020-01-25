using System.Linq;
using FrontendService.DAL;
using FrontendService.Models;
using FrontendService.Repositories;
using FrontendService.Repositories.Abstractions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FrontendService.Test.Unit.Repositories
{
    [TestClass]
    public class ArtikelRepositoryTest
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
            context.SaveChanges();
        }

        [TestMethod]
        [DataRow(2)]
        [DataRow(6)]
        [DataRow(8)]
        public void GetAll_ReturnsAllArtikelenInDatabase(int artikelenCount)
        {
            // Arrange
            Artikel[] testData = Enumerable
                .Range(0, artikelenCount)
                .Select((e, k) => new Artikel { Artikelnummer = k })
                .ToArray();

            TestHelpers.InjectData(_options, testData);

            using FrontendContext context = new FrontendContext(_options);
            ArtikelRepository artikelRepository = new ArtikelRepository(context);

            // Act
            int result = artikelRepository.GetAll().Count();

            // Assert
            Assert.AreEqual(artikelenCount, result);
        }

        [TestMethod]
        [DataRow("Fietsband")]
        [DataRow("Pomp")]
        public void Add_AddsOneArtikelToDatabase(string naam)
        {
            // Arrange
            using (FrontendContext context = new FrontendContext(_options))
            {

                ArtikelRepository artikelRepository = new ArtikelRepository(context);

                Artikel artikel = new Artikel { Naam = naam };

                // Act
                artikelRepository.Add(artikel);
            }

            // Assert
            using FrontendContext resultContext = new FrontendContext(_options);
            Assert.AreEqual(1, resultContext.Artikelen.Count());

            Artikel firstArtikel = resultContext.Artikelen.First();
            Assert.AreEqual(naam, firstArtikel.Naam);
        }

        [TestMethod]
        [DataRow(2)]
        [DataRow(6)]
        [DataRow(8)]
        public void Add_AddsMultipleArtikelenToDatabase(int artikelenCount)
        {
            // Arrange
            using (FrontendContext context = new FrontendContext(_options))
            {

                Artikel[] artikelen = Enumerable
                    .Range(0, artikelenCount)
                    .Select((e) => new Artikel())
                    .ToArray();

                ArtikelRepository artikelRepository = new ArtikelRepository(context);

                // Act
                artikelRepository.Add(artikelen);
            }

            // Assert
            using FrontendContext resultContext = new FrontendContext(_options);
            Assert.AreEqual(artikelenCount, resultContext.Artikelen.Count());
        }

        [TestMethod]
        [DataRow(28)]
        [DataRow(29482)]
        public void GetById_ReturnsItemWithId(int artikelId)
        {
            // Arrange
            Artikel artikel = new Artikel { Id = artikelId };
            TestHelpers.InjectData(_options, artikel);

            using FrontendContext frontendContext = new FrontendContext(_options);
            ArtikelRepository artikelRepository = new ArtikelRepository(frontendContext);

            // Act
            Artikel resultArtikel = artikelRepository.GetById(artikelId);

            // Assert
            Assert.IsNotNull(resultArtikel);
            Assert.AreEqual(artikelId, resultArtikel.Id);
        }

        [TestMethod]
        [DataRow(28)]
        [DataRow(29482)]
        public void GetById_ReturnsNullOnNotFound(int artikelId)
        {
            // Arrange
            using FrontendContext frontendContext = new FrontendContext(_options);
            ArtikelRepository artikelRepository = new ArtikelRepository(frontendContext);

            // Act
            Artikel resultArtikel = artikelRepository.GetById(artikelId);

            // Assert
            Assert.IsNull(resultArtikel);
        }

        [TestMethod]
        [DataRow(1, 28, 45)]
        [DataRow(5, 29, 76)]
        public void Update_UpdatesValues(long artikelId, int voorraad, int nieuweVoorraad)
        {
            // Arrange
            Artikel artikel = new Artikel
            {
                Id = artikelId,
                Voorraad = voorraad
            };
            TestHelpers.InjectData(_options, artikel);

            using FrontendContext frontendContext = new FrontendContext(_options);
            ArtikelRepository artikelRepository = new ArtikelRepository(frontendContext);

            Artikel dbArtikel = frontendContext.Artikelen.First();
            dbArtikel.Voorraad = nieuweVoorraad;

            // Act
            artikelRepository.Update(dbArtikel);

            // Assert
            using FrontendContext context = new FrontendContext(_options);
            Assert.AreEqual(nieuweVoorraad, context.Artikelen.Find(artikelId).Voorraad);
        }

        [TestMethod]
        [DataRow(102, "Test Artikel")]
        [DataRow(12, "Mooi Artikel")]
        public void GetByArtikelnummer_GetsArtikelByItsArtikelNummer(long artikelnummer, string naam)
        {
            // Arrange
            Artikel artikel = new Artikel { Artikelnummer = artikelnummer, Naam = naam};
            TestHelpers.InjectData(_options, artikel);

            using FrontendContext context = new FrontendContext(_options);
            IArtikelRepository artikelRepository = new ArtikelRepository(context);

            // Act
            Artikel result = artikelRepository.GetByArtikelnummer(artikelnummer);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(naam, result.Naam);
            Assert.AreEqual(artikelnummer, result.Artikelnummer);
        }


        [TestMethod]
        [DataRow(10)]
        [DataRow(12)]
        public void GetByArtikelnummer_ReturnsNullOnNotFound(long artikelnummer)
        {
            // Arrange
            using FrontendContext context = new FrontendContext(_options);
            IArtikelRepository artikelRepository = new ArtikelRepository(context);

            // Act
            Artikel result = artikelRepository.GetByArtikelnummer(artikelnummer);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void IsEmpty_ReturnsFalseIfItemsExist()
        {
            // Arrange
            TestHelpers.InjectData(_options, new Artikel());

            using FrontendContext context = new FrontendContext(_options);
            ArtikelRepository target = new ArtikelRepository(context);

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
            ArtikelRepository target = new ArtikelRepository(context);

            // Act
            bool result = target.IsEmpty();

            // Assert
            Assert.AreEqual(true, result);
        }
    }
}
