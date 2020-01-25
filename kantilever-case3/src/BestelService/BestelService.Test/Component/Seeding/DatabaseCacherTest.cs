using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BestelService.Agents;
using BestelService.Agents.Abstractions;
using BestelService.Constants;
using BestelService.Core.Models;
using BestelService.Core.Repositories;
using BestelService.Events;
using BestelService.Infrastructure.DAL;
using BestelService.Infrastructure.Repositories;
using BestelService.Seeding;
using BestelService.Seeding.Abstractions;
using Flurl.Http.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Minor.Miffy.MicroServices.Events;
using Minor.Miffy.TestBus;

namespace BestelService.Test.Component.Seeding
{
    [TestClass]
    public class DatabaseCacherTest
    {
        private const int MessageIntervalTime = 50;

        private SqliteConnection _connection;
        private DbContextOptions<BestelContext> _options;

        private HttpTest _httpTest;

        [TestInitialize]
        public void TestInitialize()
        {
            _httpTest = new HttpTest();

            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            _options = new DbContextOptionsBuilder<BestelContext>()
                .UseSqlite(_connection)
                .Options;

            using BestelContext context = new BestelContext(_options);
            context.Database.EnsureCreated();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _httpTest.Dispose();
            _connection.Dispose();
        }

        private IServiceProvider _serviceProvider;

        /// <summary>
        /// Since populating a cacher with dependencies is quite repetitive, this function makes it
        /// easy to just generate one
        /// </summary>
        private IDatabaseCacher GeneratePopulatedController(BestelContext bestelContext)
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton(bestelContext);
            services.AddSingleton<IKlantRepository, KlantRepository>();
            services.AddSingleton<ILoggerFactory, NullLoggerFactory>();
            services.AddSingleton<IDatabaseCacher, DatabaseCacher>();
            services.AddSingleton<IAuditAgent, AuditAgent>();
            services.AddSingleton<IHttpAgent, HttpAgent>();
            services.AddSingleton<IEventReplayer, EventReplayer>();

            services.AddSingleton(services);

            _serviceProvider = services.BuildServiceProvider();

            return _serviceProvider.GetRequiredService<IDatabaseCacher>();
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(4)]
        [DataRow(12)]
        [DataRow(20)]
        public void EnsureKlanten_SavesAllKlanten(int amount)
        {
            // Arrange
            using BestelContext context = new BestelContext(_options);
            TestBusContext busContext = new TestBusContext();

            Environment.SetEnvironmentVariable(EnvNames.AuditLoggerUrl, "http://auditlogger");

            IDatabaseCacher databaseCacher = GeneratePopulatedController(context);

            NieuweKlantAangemaaktEvent[] events = Enumerable.Range(0, amount).Select(b => new NieuweKlantAangemaaktEvent
            {
                Klant = new Klant { Factuuradres = new Adres() }
            }).ToArray();

            _httpTest.RespondWith(amount.ToString());

            IEventReplayer eventReplayer = _serviceProvider.GetRequiredService<IEventReplayer>();

            IEventPublisher eventPublisher = new EventPublisher(busContext);

            eventReplayer.StartedReplaying += () =>
            {
                foreach (NieuweKlantAangemaaktEvent @event in events)
                {
                    eventPublisher.Publish(@event);

                    Thread.Sleep(MessageIntervalTime);
                }
            };

            // Act
            databaseCacher.EnsureKlanten(busContext);

            // Assert
            Assert.AreEqual(events.Length, context.Klanten.Count());
        }
    }
}
