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
    public class KlantRepositoryTest
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
            context.Bestellingen.RemoveRange(context.Bestellingen);
            context.Klanten.RemoveRange(context.Klanten);
            context.SaveChanges();
        }

        [TestMethod]
        [DataRow("Hans Janssen", "56327986")]
        [DataRow("Jensen", "5431545")]
        [DataRow("Peer Uitewaard", "34850275")]
        [DataRow("Willy Bord", "463253265")]
        public void Add_AddsTheKlantToTheDatabase(string naam, string telefoonnumer)
        {
            // Arrange
            using FrontendContext context = new FrontendContext(_options);
            KlantRepository target = new KlantRepository(context);
            Klant klant = new Klant { Naam = naam, Telefoonnummer = telefoonnumer };

            // Act
            target.Add(klant);

            //Assert
            using FrontendContext newContext = new FrontendContext(_options);
            Klant klantFromDb = newContext.Klanten.First();
            Assert.AreEqual(naam, klantFromDb.Naam);
            Assert.AreEqual(telefoonnumer, klantFromDb.Telefoonnummer);
        }

        [TestMethod]
        public void IsEmpty_ReturnsFalseIfItemsExist()
        {
            // Arrange
            TestHelpers.InjectData(_options, new Klant());

            using FrontendContext context = new FrontendContext(_options);
            KlantRepository target = new KlantRepository(context);

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
            KlantRepository target = new KlantRepository(context);

            // Act
            bool result = target.IsEmpty();

            // Assert
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        [DataRow(20, "Jan Piet")]
        [DataRow(634, "Test Tester")]
        public void GetById_ReturnsKlantWithId(long id, string naam)
        {
            // Arrange
            Klant klant = new Klant { Id = id, Naam = naam };
            TestHelpers.InjectData(_options, klant);

            using FrontendContext context = new FrontendContext(_options);
            KlantRepository target = new KlantRepository(context);

            // Act
            Klant result = target.GetById(id);

            // Assert
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual(naam, result.Naam);
        }

        [TestMethod]
        [DataRow(23)]
        [DataRow(592)]
        public void GetById_ReturnsNullOnNonExist(long id)
        {
            // Arrange
            using FrontendContext context = new FrontendContext(_options);
            KlantRepository target = new KlantRepository(context);

            // Act
            Klant result = target.GetById(id);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        [DataRow(2, "jan@frederik.nl")]
        [DataRow(2, "fredje@gmail.com")]
        public void GetByUsername_ReturnsKlantIfExists(long id, string naam)
        {
            // Arrange
            Klant klant = new Klant { Id = id, Username = naam };
            TestHelpers.InjectData(_options, klant);

            using FrontendContext context = new FrontendContext(_options);
            KlantRepository target = new KlantRepository(context);

            // Act
            Klant result = target.GetByUsername(naam);

            // Assert
            Assert.AreEqual(id, result.Id);
            Assert.AreEqual(naam, result.Username);
        }

        [TestMethod]
        [DataRow("piet@jan.nl")]
        [DataRow("frederik@gmail.com")]
        public void GetByUsername_ReturnsNullOnNonExist(string naam)
        {
            // Arrange
            using FrontendContext context = new FrontendContext(_options);
            KlantRepository target = new KlantRepository(context);

            // Act
            Klant result = target.GetByUsername(naam);

            // Assert
            Assert.IsNull(result);
        }
    }
}
