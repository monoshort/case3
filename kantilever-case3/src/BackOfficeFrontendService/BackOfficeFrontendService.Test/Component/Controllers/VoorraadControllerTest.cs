using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BackOfficeFrontendService.Agents;
using BackOfficeFrontendService.Agents.Abstractions;
using BackOfficeFrontendService.Constants;
using BackOfficeFrontendService.Controllers;
using BackOfficeFrontendService.DAL;
using BackOfficeFrontendService.EventListeners;
using BackOfficeFrontendService.Models;
using BackOfficeFrontendService.Repositories;
using BackOfficeFrontendService.Repositories.Abstractions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minor.Miffy;
using Minor.Miffy.MicroServices.Events;
using Minor.Miffy.MicroServices.Host;
using Minor.Miffy.TestBus;
using RabbitMQ.Client;

namespace BackOfficeFrontendService.Test.Component.Controllers
{
    [TestClass]
    public class VoorraadControllerTest
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
            context.VoorraadMagazijn.RemoveRange(context.VoorraadMagazijn);
            context.Bestellingen.RemoveRange(context.Bestellingen);
            context.SaveChanges();
        }

        /// <summary>
        ///     Since populating a controller with dependencies is quite repetitive, this function makes it
        ///     easy to just generate one
        /// </summary>
        private VoorraadController GeneratePopulatedController(IBusContext<IConnection> testBusContext,
            BackOfficeContext backOfficeContext)
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<VoorraadController>();
            serviceCollection.AddSingleton<IVoorraadRepository, VoorraadRepository>();
            serviceCollection.AddSingleton<IVoorraadAgent, VoorraadAgent>();
            serviceCollection.AddSingleton<IEventPublisher, EventPublisher>();
            serviceCollection.AddSingleton<IHttpAgent, HttpAgent>();
            serviceCollection.AddSingleton(testBusContext);
            serviceCollection.AddSingleton(backOfficeContext);

            return serviceCollection.BuildServiceProvider().GetRequiredService<VoorraadController>();
        }

        [TestMethod]
        [DataRow(10)]
        [DataRow(520)]
        public void BestelBijAsync_SendsAndReceivesEventAndSetsVoorraadProperly(long artikelNummer)
        {
            // Arrange
            Environment.SetEnvironmentVariable(EnvNames.VoorraadServiceUrl, "http://example.com");

            VoorraadMagazijn voorraadMagazijn = new VoorraadMagazijn
            {
                ArtikelNummer = artikelNummer,
                VoorraadBesteld = false,
                BestelRegels = new List<BestelRegel>
                {
                    new BestelRegel { Aantal = 5, Bestelling = new Bestelling() }
                }
            };

            TestHelpers.InjectData(_options, voorraadMagazijn);

            using BackOfficeContext backOfficeContext = new BackOfficeContext(_options);
            IBusContext<IConnection> testBusContext = new TestBusContext();

            VoorraadController voorraadController = GeneratePopulatedController(testBusContext, backOfficeContext);

            MicroserviceHostBuilder hostBuilder = new MicroserviceHostBuilder()
                .WithBusContext(testBusContext)
                .RegisterDependencies(services =>
                {
                    services.AddSingleton<VoorraadController>();
                    services.AddSingleton<IVoorraadRepository, VoorraadRepository>();
                    services.AddSingleton<IVoorraadAgent, VoorraadAgent>();
                    services.AddSingleton<IEventPublisher, EventPublisher>();
                    services.AddSingleton<IHttpAgent, HttpAgent>();
                    services.AddSingleton(testBusContext);
                    services.AddSingleton(backOfficeContext);
                })
                .AddEventListener<VoorraadEventListeners>();

            using IMicroserviceHost host = hostBuilder.CreateHost();
            host.Start();

            // Act
            voorraadController.BijBesteld(artikelNummer);

            Thread.Sleep(WaitTime);

            // Assert
            using BackOfficeContext resultContext = new BackOfficeContext(_options);
            VoorraadMagazijn result = resultContext.VoorraadMagazijn.Single();

            Assert.AreEqual(true, result.VoorraadBesteld);
        }
    }
}
