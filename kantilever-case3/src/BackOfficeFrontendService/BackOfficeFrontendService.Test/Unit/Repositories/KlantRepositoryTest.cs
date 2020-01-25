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
    public class KlantRepositoryTest
    {
        private static SqliteConnection _connection;
        private static DbContextOptions<BackOfficeContext> _options;

        [ClassInitialize]
        public static void ClassInitialize(TestContext ctx)
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();
            _options = new DbContextOptionsBuilder<BackOfficeContext>()
                .UseSqlite(_connection).Options;

            using BackOfficeContext context = new BackOfficeContext(_options);
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
            using var context = new BackOfficeContext(_options);
            context.RemoveRange(context.Klanten);
            context.SaveChanges();
        }

        [TestMethod]
        [DataRow("Jan de Man", "Breda")]
        [DataRow("Max de Koning", "Rotterdam")]
        public void Add_AddsKlantToDatabase(string naam, string woonplaats)
        {
            // Arrange
            Klant klant = new Klant
            {
                Naam = naam,
                Factuuradres = new Adres
                {
                    Woonplaats = woonplaats
                }
            };

            using BackOfficeContext context = new BackOfficeContext(_options);
            IKlantRepository repository = new KlantRepository(context);

            // Act
            repository.Add(klant);

            // Assert
            using var assertContext = new BackOfficeContext(_options);
            Assert.AreEqual(1, assertContext.Klanten.Count());

            Klant firstItem = assertContext.Klanten.Include(e => e.Factuuradres).First();
            Assert.AreEqual(naam, firstItem.Naam);
            Assert.AreEqual(woonplaats, firstItem.Factuuradres.Woonplaats);
        }

        [TestMethod]
        [DataRow(242, "Dirk-Jan de Bijlmer")]
        [DataRow(7334, "Adam van Loon")]
        public void FindById_ReturnsExpectedKlant(long id, string naam)
        {
            // Arrange
            Klant klant = new Klant
            {
                Naam = naam,
                Id = id
            };
            TestHelpers.InjectData(_options, klant);

            using BackOfficeContext context = new BackOfficeContext(_options);
            IKlantRepository klantRepository = new KlantRepository(context);

            // Act
            Klant resultKlant = klantRepository.FindById(id);

            // Assert
            Assert.IsNotNull(resultKlant);
            Assert.AreEqual(naam, resultKlant.Naam);
            Assert.AreEqual(id, resultKlant.Id);
        }

        [TestMethod]
        [DataRow(242, "Dirk-Jan de Bijlmer")]
        [DataRow(7334, "Adam van Loon")]
        public void FindById_ReturnsNullOnNonExistentKlant(long id, string naam)
        {
            // Arrange
            using BackOfficeContext context = new BackOfficeContext(_options);
            IKlantRepository klantRepository = new KlantRepository(context);

            // Act
            Klant resultKlant = klantRepository.FindById(id);

            // Assert
            Assert.IsNull(resultKlant);
        }

        [TestMethod]
        public void IsEmpty_IsFalseIfDatabaseContainsKlanten()
        {
            // Arrange
            Klant klant = new Klant();
            TestHelpers.InjectData(_options, klant);

            using var context = new BackOfficeContext(_options);
            IKlantRepository repository = new KlantRepository(context);

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
    }
}
