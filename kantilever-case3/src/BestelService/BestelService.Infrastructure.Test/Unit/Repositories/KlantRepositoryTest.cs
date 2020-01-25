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
    public class KlantRepositoryTest
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
            context.Set<Klant>().RemoveRange(context.Set<Klant>());
            context.SaveChanges();
        }

        [TestMethod]
        [DataRow("Dani")]
        [DataRow("Janneke")]
        public void Add_AddsKlantToDb(string naam)
        {
            // Arrange
            Klant klant = new Klant {Naam = naam};

            using BestelContext bestelContext = new BestelContext(_options);
            IKlantRepository repository = new KlantRepository(bestelContext);

            // Act
            repository.Add(klant);

            // Assert
            using BestelContext resultContext = new BestelContext(_options);
            Klant result = bestelContext.Klanten.Single();

            Assert.AreEqual(naam, result.Naam);
        }

        [TestMethod]
        [DataRow("Dani")]
        [DataRow("Janneke")]
        public void Add_AddsMultipleKlantenToDb(string naam)
        {
            // Arrange
            Klant klant1 = new Klant {Naam = naam};
            Klant klant2 = new Klant {Naam = naam};

            using BestelContext bestelContext = new BestelContext(_options);
            IKlantRepository repository = new KlantRepository(bestelContext);

            // Act
            repository.Add(klant1, klant2);

            // Assert
            using BestelContext resultContext = new BestelContext(_options);
            Assert.AreEqual(2, resultContext.Klanten.Count());
        }

        [TestMethod]
        [DataRow(20)]
        [DataRow(602)]
        public void GetById_ReturnsKlantOnFound(long id)
        {
            // Arrange
            Klant klant = new Klant {Id = id, Naam = "Pietje"};

            TestHelpers.InjectData(_options, klant);

            using BestelContext bestelContext = new BestelContext(_options);
            IKlantRepository repository = new KlantRepository(bestelContext);

            // Act
            Klant result = repository.GetById(id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Pietje", result.Naam);
        }

        [TestMethod]
        [DataRow(20)]
        [DataRow(602)]
        public void GetById_ReturnsNullOnNotFound(long id)
        {
            // Arrange
            using BestelContext bestelContext = new BestelContext(_options);
            IKlantRepository repository = new KlantRepository(bestelContext);

            // Act
            Klant result = repository.GetById(id);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void IsEmpty_ReturnsTrueOnDatabaseEmpty()
        {
            // Arrange
            using BestelContext bestelContext = new BestelContext(_options);
            IKlantRepository repository = new KlantRepository(bestelContext);

            // Act
            bool result = repository.IsEmpty();

            // Assert
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void IsEmpty_ReturnsFalseOnDatabaseNotEmpty()
        {
            // Arrange
            Klant klant = new Klant();
            TestHelpers.InjectData(_options, klant);

            using BestelContext bestelContext = new BestelContext(_options);
            IKlantRepository repository = new KlantRepository(bestelContext);

            // Act
            bool result = repository.IsEmpty();

            // Assert
            Assert.AreEqual(false, result);
        }
    }
}
