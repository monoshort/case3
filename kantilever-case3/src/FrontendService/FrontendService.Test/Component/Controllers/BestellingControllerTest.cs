using System.Linq;
using System.Text;
using System.Threading;
using FrontendService.Agents;
using FrontendService.Agents.Abstractions;
using FrontendService.Commands;
using FrontendService.Constants;
using FrontendService.Controllers;
using FrontendService.DAL;
using FrontendService.Models;
using FrontendService.Repositories;
using FrontendService.Repositories.Abstractions;
using FrontendService.ViewModels;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minor.Miffy;
using Minor.Miffy.MicroServices.Commands;
using Minor.Miffy.TestBus;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace FrontendService.Test.Component.Controllers
{
    [TestClass]
    public class BestellingControllerTest
    {
        private const int WaitTime = 200;

        private static SqliteConnection _connection;
        private static DbContextOptions<FrontendContext> _options;

        [TestInitialize]
        public void ClassInitialize()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            _options = new DbContextOptionsBuilder<FrontendContext>()
                .UseSqlite(_connection)
                .Options;

            using FrontendContext context = new FrontendContext(_options);
            context.Database.EnsureCreated();
        }

        [TestCleanup]
        public void ClassCleanup()
        {
            using FrontendContext context = new FrontendContext(_options);
            context.Artikelen.RemoveRange(context.Artikelen);
            context.SaveChanges();
            _connection.Close();
        }

        /// <summary>
        /// Since populating a controller with dependencies is quite repetitive, this function makes it
        /// easy to just generate one
        /// </summary>
        private BestellingController GeneratePopulatedController(IBusContext<IConnection> testBusContext, FrontendContext frontendContext)
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<ICommandPublisher, CommandPublisher>();
            services.AddSingleton<IBestellingAgent, BestellingAgent>();
            services.AddSingleton<IArtikelRepository, ArtikelRepository>();
            services.AddSingleton<IKlantRepository, KlantRepository>();
            services.AddSingleton<BestellingController>();
            services.AddSingleton(testBusContext);
            services.AddSingleton(frontendContext);
            return services.BuildServiceProvider().GetRequiredService<BestellingController>();
        }

        [TestMethod]
        [DataRow(10)]
        [DataRow(5927)]
        public void Post_TriggersCommandToBestelServiceWithBestelling(int artikelId)
        {
            // Arrange
            using FrontendContext frontendContext = new FrontendContext(_options);
            TestBusContext testBusContext = new TestBusContext();

            bool receivedCommand = false;

            ICommandReceiver receiver = testBusContext.CreateCommandReceiver(QueueNames.MaakNieuweBestellingAanCommand);
            receiver.DeclareCommandQueue();
            receiver.StartReceivingCommands(e =>
            {
                receivedCommand = true;
                return e;
            });

            ICommandReceiver klantReceiver = testBusContext.CreateCommandReceiver(QueueNames.MaakNieuweKlantAanCommand);
            klantReceiver.DeclareCommandQueue();
            klantReceiver.StartReceivingCommands(e =>
            {
                string content = Encoding.Unicode.GetString(e.Body);
                MaakNieuweKlantAanCommand command = JsonConvert.DeserializeObject<MaakNieuweKlantAanCommand>(content);
                command.Klant.Id = 10;
                string returnContent = JsonConvert.SerializeObject(command);
                e.Body = Encoding.Unicode.GetBytes(returnContent);
                return e;
            });

            BestellingController bestellingController = GeneratePopulatedController(testBusContext, frontendContext);
            TestHelpers.InjectData(_options, new Artikel {Id = artikelId});

            WinkelwagenViewModel winkelwagenViewModel = new WinkelwagenViewModel
            {
                Artikelen = new []
                {
                    new WinkelwagenRijViewModel {Artikel = new ArtikelViewModel {Id = artikelId}}
                }
            };

            BestellingViewModel bestellingViewModel = new BestellingViewModel
            {
                Winkelwagen = winkelwagenViewModel,
                Klant = new KlantViewModel(),
                AfleverAdres = new Adres()
            };

            // Act
            bestellingController.Post(bestellingViewModel);

            Thread.Sleep(WaitTime);

            // Assert
            Assert.IsTrue(receivedCommand);
        }

        [TestMethod]
        [DataRow(10, 1920)]
        [DataRow(93, 49)]
        [DataRow(5927, 10)]
        public void Post_TriggersCommandToBestelServiceWithTwoBestellingen(int artikelId, int klantId)
        {
            // Arrange
            Klant klant = new Klant {Id = klantId};
            Artikel artikel = new Artikel {Id = artikelId};

            TestHelpers.InjectData(_options, klant);
            TestHelpers.InjectData(_options, artikel);

            using FrontendContext frontendContext = new FrontendContext(_options);
            TestBusContext testBusContext = new TestBusContext();

            int amountOfTimesCalled = 0;

            ICommandReceiver receiver = testBusContext.CreateCommandReceiver(QueueNames.MaakNieuweBestellingAanCommand);
            receiver.DeclareCommandQueue();
            receiver.StartReceivingCommands(e =>
            {
                Interlocked.Increment(ref amountOfTimesCalled);
                return e;
            });

            BestellingController bestellingController1 = GeneratePopulatedController(testBusContext, frontendContext);
            BestellingController bestellingController2 = GeneratePopulatedController(testBusContext, frontendContext);

            WinkelwagenViewModel winkelwagenViewModel = new WinkelwagenViewModel
            {
                Artikelen = new []
                {
                    new WinkelwagenRijViewModel {Artikel = new ArtikelViewModel {Id = artikelId}}
                }
            };

            BestellingViewModel bestellingViewModel = new BestellingViewModel
            {
                Winkelwagen = winkelwagenViewModel,
                Klant = new KlantViewModel { Id = 10 },
                AfleverAdres = new Adres()
            };

            // Act
            bestellingController1.Post(bestellingViewModel);
            bestellingController2.Post(bestellingViewModel);

            Thread.Sleep(WaitTime);

            // Assert
            Assert.AreEqual(2, amountOfTimesCalled);
        }

        [TestMethod]
        [DataRow(10, 2, "Breda", "Jan van Bergen")]
        [DataRow(2, 6, "Bergen op Zoom", "Bertha Jonkers")]
        public void Post_TriggersCommandToBestelServiceWithProperValues(int artikelId, int aantal, string klantWoonplaats, string klantNaam)
        {
            // Arrange
            using FrontendContext frontendContext = new FrontendContext(_options);
            TestBusContext testBusContext = new TestBusContext();

            MaakNieuweBestellingAanCommand receivedCommand = null;

            Klant klant1 = new Klant
            {
                Id = 10,
                Naam = klantNaam,
                Factuuradres = new Adres
                {
                    Woonplaats = klantWoonplaats
                }
            };

            ICommandReceiver receiver = testBusContext.CreateCommandReceiver(QueueNames.MaakNieuweBestellingAanCommand);
            receiver.DeclareCommandQueue();
            receiver.StartReceivingCommands(e =>
            {
                string stringBody = Encoding.Unicode.GetString(e.Body);
                receivedCommand = JsonConvert.DeserializeObject<MaakNieuweBestellingAanCommand>(stringBody);
                return e;
            });

            BestellingController bestellingController = GeneratePopulatedController(testBusContext, frontendContext);
            TestHelpers.InjectData(_options, new Artikel {Id = artikelId});
            TestHelpers.InjectData(_options, klant1);

            WinkelwagenViewModel winkelwagenViewModel = new WinkelwagenViewModel
            {
                Artikelen = new []
                {
                    new WinkelwagenRijViewModel
                    {
                        Aantal = aantal,
                        Artikel = new ArtikelViewModel
                        {
                            Id = artikelId
                        }
                    }
                }
            };

            KlantViewModel klant = new KlantViewModel
            {
                Id = 10
            };

            BestellingViewModel bestellingViewModel = new BestellingViewModel
            {
                Winkelwagen = winkelwagenViewModel,
                Klant = klant,
                AfleverAdres = new Adres
                {
                    Woonplaats = klantWoonplaats
                }
            };

            // Act
            bestellingController.Post(bestellingViewModel);

            Thread.Sleep(WaitTime);

            // Assert
            BestelRegel winkelwagenResult = receivedCommand.Bestelling.BestelRegels.First();

            Assert.AreEqual(aantal, winkelwagenResult.Aantal);
            Assert.AreEqual(klantNaam, receivedCommand.Bestelling.Klant.Naam);
            Assert.AreEqual(klantWoonplaats, receivedCommand.Bestelling.Klant.Factuuradres.Woonplaats);
            Assert.AreEqual(klantWoonplaats, receivedCommand.Bestelling.AfleverAdres.Woonplaats);
        }

    }
}
