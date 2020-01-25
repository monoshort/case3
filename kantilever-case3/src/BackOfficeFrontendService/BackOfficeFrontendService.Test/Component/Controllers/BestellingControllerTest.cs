using System.Text;
using System.Threading;
using BackOfficeFrontendService.Agents;
using BackOfficeFrontendService.Agents.Abstractions;
using BackOfficeFrontendService.Commands;
using BackOfficeFrontendService.Constants;
using BackOfficeFrontendService.Controllers;
using BackOfficeFrontendService.DAL;
using BackOfficeFrontendService.Models;
using BackOfficeFrontendService.Repositories;
using BackOfficeFrontendService.Repositories.Abstractions;
using BackOfficeFrontendService.ViewModels;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minor.Miffy;
using Minor.Miffy.MicroServices.Commands;
using Minor.Miffy.TestBus;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace BackOfficeFrontendService.Test.Component.Controllers
{
    [TestClass]
    public class BestellingControllerTest
    {
        private const int WaitTime = 200;

        private static SqliteConnection _connection;
        private static DbContextOptions<BackOfficeContext> _options;

        [ClassInitialize]
        public static void ClassInitialize(TestContext ctx)
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            _options = new DbContextOptionsBuilder<BackOfficeContext>()
                .UseSqlite(_connection)
                .Options;

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
            using BackOfficeContext context = new BackOfficeContext(_options);
            context.Set<BestelRegel>().RemoveRange(context.Set<BestelRegel>());
            context.Bestellingen.RemoveRange(context.Bestellingen);
            context.SaveChanges();
        }

        /// <summary>
        ///     Since populating a controller with dependencies is quite repetitive, this function makes it
        ///     easy to just generate one
        /// </summary>
        private BestellingController GeneratePopulatedController(IBusContext<IConnection> testBusContext,
            BackOfficeContext backOfficeContext)
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<BestellingController>();
            serviceCollection.AddSingleton<ICommandPublisher, CommandPublisher>();
            serviceCollection.AddSingleton<IBestellingRepository, BestellingRepository>();
            serviceCollection.AddSingleton<IBestellingAgent, BestellingAgent>();
            serviceCollection.AddSingleton(testBusContext);
            serviceCollection.AddSingleton(backOfficeContext);

            return serviceCollection.BuildServiceProvider().GetRequiredService<BestellingController>();
        }

        [TestMethod]
        [DataRow(10)]
        [DataRow(5927)]
        public void KeurBestellingGoed_TriggersCommandToBestelServiceWithProperValues(int bestellingId)
        {
            // Arrange
            using BackOfficeContext backOfficeContext = new BackOfficeContext(_options);
            TestBusContext testBusContext = new TestBusContext();

            KeurBestellingGoedCommand receivedCommand = null;

            ICommandReceiver receiver = testBusContext.CreateCommandReceiver(QueueNames.KeurBestellingGoed);
            receiver.DeclareCommandQueue();
            receiver.StartReceivingCommands(e =>
            {
                string stringBody = Encoding.Unicode.GetString(e.Body);
                receivedCommand = JsonConvert.DeserializeObject<KeurBestellingGoedCommand>(stringBody);
                return e;
            });

            BestellingController bestellingController = GeneratePopulatedController(testBusContext, backOfficeContext);
            TestHelpers.InjectData(_options, new Bestelling { Id = bestellingId });

            // Act
            bestellingController.KeurBestellingGoed(bestellingId).Wait();

            Thread.Sleep(WaitTime);

            // Assert
            Assert.AreEqual(bestellingId, receivedCommand.BestellingId);
        }

        [TestMethod]
        [DataRow(10)]
        [DataRow(5927)]
        public void KeurBestellingAf_TriggersCommandToBestelServiceWithProperValues(int bestellingId)
        {
            // Arrange
            using BackOfficeContext backOfficeContext = new BackOfficeContext(_options);
            TestBusContext testBusContext = new TestBusContext();

            KeurBestellingAfCommand receivedCommand = null;

            ICommandReceiver receiver = testBusContext.CreateCommandReceiver(QueueNames.KeurBestellingAf);
            receiver.DeclareCommandQueue();
            receiver.StartReceivingCommands(e =>
            {
                string stringBody = Encoding.Unicode.GetString(e.Body);
                receivedCommand = JsonConvert.DeserializeObject<KeurBestellingAfCommand>(stringBody);
                return e;
            });

            BestellingController bestellingController = GeneratePopulatedController(testBusContext, backOfficeContext);
            TestHelpers.InjectData(_options, new Bestelling { Id = bestellingId });

            // Act
            bestellingController.KeurBestellingAf(bestellingId).Wait();

            Thread.Sleep(WaitTime);

            // Assert
            Assert.AreEqual(bestellingId, receivedCommand.BestellingId);
        }

        [TestMethod]
        [DataRow("10.00")]
        [DataRow("20.00")]
        [DataRow("59.99")]
        public void BetalingRegistreren_PublishesRegistreerBetalingCommand(string bedragStr)
        {
            // Arrange
            var bedrag = decimal.Parse(bedragStr);
            using BackOfficeContext backOfficeContext = new BackOfficeContext(_options);
            TestBusContext testBusContext = new TestBusContext();

            RegistreerBetalingCommand receivedCommand = null;

            ICommandReceiver receiver = testBusContext.CreateCommandReceiver(QueueNames.RegistreerBetaling);
            receiver.DeclareCommandQueue();
            receiver.StartReceivingCommands(e =>
            {
                string stringBody = Encoding.Unicode.GetString(e.Body);
                receivedCommand = JsonConvert.DeserializeObject<RegistreerBetalingCommand>(stringBody);
                return e;
            });

            BestellingController bestellingController = GeneratePopulatedController(testBusContext, backOfficeContext);
            TestHelpers.InjectData(_options, new Bestelling
            {
                Id = 3,
                OpenstaandBedrag = 200M,
                BestellingNummer = "10003"
            }
            );

            // Act
            bestellingController.BetalingRegistreren(new BetalingRegistrerenViewModel
            {
                BetaaldBedrag = bedrag,
                OpenstaandBedrag = 200M,
                BestellingNummer = "10003"
            }).Wait();

            Thread.Sleep(WaitTime);

            // Assert
            Assert.AreEqual(bedrag, receivedCommand.BetaaldBedrag);
        }

        [TestMethod]
        [DataRow(10)]
        [DataRow(30)]
        public void GetFactuur_PublishesPrintFactuurCommand(long bestellingId)
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                Id = bestellingId
            };
            TestHelpers.InjectData(_options, bestelling);

            using BackOfficeContext backOfficeContext = new BackOfficeContext(_options);
            TestBusContext testBusContext = new TestBusContext();

            PrintFactuurCommand receivedCommand = null;

            ICommandReceiver receiver = testBusContext.CreateCommandReceiver(QueueNames.PrintFactuur);
            receiver.DeclareCommandQueue();
            receiver.StartReceivingCommands(e =>
            {
                string stringBody = Encoding.Unicode.GetString(e.Body);
                receivedCommand = JsonConvert.DeserializeObject<PrintFactuurCommand>(stringBody);
                return e;
            });

            BestellingController bestellingController = GeneratePopulatedController(testBusContext, backOfficeContext);

            // Act
            bestellingController.GetFactuur(bestellingId).Wait();

            Thread.Sleep(WaitTime);

            // Assert
            Assert.AreEqual(bestellingId, receivedCommand.BestellingId);
        }

        [TestMethod]
        [DataRow(10)]
        [DataRow(30)]
        public void GetAdresLabel_PublishesPrintAdresLabelCommand(long bestellingId)
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                Id = bestellingId
            };
            TestHelpers.InjectData(_options, bestelling);

            using BackOfficeContext backOfficeContext = new BackOfficeContext(_options);
            TestBusContext testBusContext = new TestBusContext();

            PrintAdresLabelCommand receivedCommand = null;

            ICommandReceiver receiver = testBusContext.CreateCommandReceiver(QueueNames.PrintAdresLabel);
            receiver.DeclareCommandQueue();
            receiver.StartReceivingCommands(e =>
            {
                string stringBody = Encoding.Unicode.GetString(e.Body);
                receivedCommand = JsonConvert.DeserializeObject<PrintAdresLabelCommand>(stringBody);
                return e;
            });

            BestellingController bestellingController = GeneratePopulatedController(testBusContext, backOfficeContext);

            // Act
            bestellingController.GetAdresLabel(bestellingId).Wait();

            Thread.Sleep(WaitTime);

            // Assert
            Assert.AreEqual(bestellingId, receivedCommand.BestellingId);
        }

        [TestMethod]
        [DataRow(10)]
        [DataRow(30)]
        public void MeldKlaar_PublishesMeldBestellingKlaarCommand(long bestellingId)
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                Id = bestellingId,
                KanKlaarGemeldWorden = true
            };
            TestHelpers.InjectData(_options, bestelling);

            using BackOfficeContext backOfficeContext = new BackOfficeContext(_options);
            TestBusContext testBusContext = new TestBusContext();

            MeldBestellingKlaarCommand receivedCommand = null;

            ICommandReceiver receiver = testBusContext.CreateCommandReceiver(QueueNames.MeldBestellingKlaar);
            receiver.DeclareCommandQueue();
            receiver.StartReceivingCommands(e =>
            {
                string stringBody = Encoding.Unicode.GetString(e.Body);
                receivedCommand = JsonConvert.DeserializeObject<MeldBestellingKlaarCommand>(stringBody);
                return e;
            });

            BestellingController bestellingController = GeneratePopulatedController(testBusContext, backOfficeContext);

            // Act
            bestellingController.MeldKlaar(bestellingId).Wait();

            Thread.Sleep(WaitTime);

            // Assert
            Assert.AreEqual(bestellingId, receivedCommand.BestellingId);
        }

        [TestMethod]
        [DataRow(10, 5)]
        [DataRow(30, 2)]
        public void VinkBestelregelAan_PublishesPakBestelRegelInCommand(long bestellingId, long bestelRegelId)
        {
            // Arrange
            Bestelling bestelling = new Bestelling
            {
                Id = bestellingId,
                BestelRegels = { new BestelRegel { Id = bestelRegelId }},
                KanKlaarGemeldWorden = true
            };
            TestHelpers.InjectData(_options, bestelling);

            using BackOfficeContext backOfficeContext = new BackOfficeContext(_options);
            TestBusContext testBusContext = new TestBusContext();

            PakBestelRegelInCommand receivedCommand = null;

            ICommandReceiver receiver = testBusContext.CreateCommandReceiver(QueueNames.PakBestelRegelIn);
            receiver.DeclareCommandQueue();
            receiver.StartReceivingCommands(e =>
            {
                string stringBody = Encoding.Unicode.GetString(e.Body);
                receivedCommand = JsonConvert.DeserializeObject<PakBestelRegelInCommand>(stringBody);
                return e;
            });

            BestellingController bestellingController = GeneratePopulatedController(testBusContext, backOfficeContext);

            // Act
            bestellingController.VinkBestelregelAan(bestellingId, bestelRegelId).Wait();

            Thread.Sleep(WaitTime);

            // Assert
            Assert.AreEqual(bestellingId, receivedCommand.BestellingId);
            Assert.AreEqual(bestelRegelId, receivedCommand.BestelRegelId);
        }
    }
}
