using System.Linq;
using KlantService.DAL;
using KlantService.Models;
using KlantService.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KlantService.Test.Unit.Repositories
{
    [TestClass]
    public class KlantRepositoryTest
    {
        private static SqliteConnection _connection;
        private static DbContextOptions<KlantContext> _options;

        [ClassInitialize]
        public static void ClassInitialize(TestContext tc)
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();
            _options = new DbContextOptionsBuilder<KlantContext>()
                .UseSqlite(_connection).Options;

            using var context = new KlantContext(_options);
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
            using var context = new KlantContext(_options);
            context.Set<Klant>().RemoveRange(context.Set<Klant>());
            context.SaveChanges();
        }

        [TestMethod]
        [DataRow("Jan Peter", "0882745827", "Rotterdam", "2352ED", "Jacobstraat")]
        [DataRow("Paul Ellen", "0672849285", "Amsterdam", "2019FE", "Janlaan")]
        public void Add_AddsKlantToDatabase(string naam, string telefoon, string woonplaats, string postcode, string straat)
        {
            // Arrange
            using (KlantContext context = new KlantContext(_options))
            {
                Klant bestelling = new Klant
                {
                    Naam = naam,
                    Telefoonnummer = telefoon,
                    Factuuradres = new Adres
                    {
                        Woonplaats = woonplaats,
                        Postcode = postcode,
                        StraatnaamHuisnummer = straat
                    }
                };

                var repository = new KlantRepository(context);

                // Act
                repository.Add(bestelling);
            }

            // Assert
            using var resultContext = new KlantContext(_options);
            Klant[] resultData = resultContext.Klanten.Include(e => e.Factuuradres).ToArray();
            Assert.AreEqual(1, resultData.Length);

            Klant firstItem = resultData.First();
            Assert.AreEqual(naam, firstItem.Naam);
            Assert.AreEqual(telefoon, firstItem.Telefoonnummer);
            Assert.AreEqual(woonplaats, firstItem.Factuuradres.Woonplaats);
            Assert.AreEqual(postcode, firstItem.Factuuradres.Postcode);
            Assert.AreEqual(straat, firstItem.Factuuradres.StraatnaamHuisnummer);
        }
    }
}
