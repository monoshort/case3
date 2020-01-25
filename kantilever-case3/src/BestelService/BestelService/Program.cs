using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using BestelService.Agents;
using BestelService.Agents.Abstractions;
using BestelService.Constants;
using BestelService.Core.Repositories;
using BestelService.Infrastructure.DAL;
using BestelService.Infrastructure.Repositories;
using BestelService.Seeding;
using BestelService.Seeding.Abstractions;
using BestelService.Services.Services;
using BestelService.Services.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Minor.Miffy;
using Minor.Miffy.MicroServices.Events;
using Minor.Miffy.MicroServices.Host;
using Minor.Miffy.RabbitMQBus;
using RabbitMQ.Client;

namespace BestelService
{
    /**
     * This class is tested through integration tests
     */
    [ExcludeFromCodeCoverage]
    public static class Program
    {
        private const string QueueName = "BestelService.Queue";

        public static void Main(string[] args)
        {
            using var loggerFactory = LoggerFactory.Create(configure =>
            {
                configure.AddConsole().SetMinimumLevel(LogLevel.Debug);
            });

            MiffyLoggerFactory.LoggerFactory = loggerFactory;
            RabbitMqLoggerFactory.LoggerFactory = loggerFactory;

            using var context = new RabbitMqContextBuilder()
                .ReadFromEnvironmentVariables()
                .CreateContext();

            using var replayContext = new RabbitMqContextBuilder()
                .ReadFromEnvironmentVariables()
                .WithExchange(Environment.GetEnvironmentVariable(EnvNames.ReplayExchangeName))
                .CreateContext();

            ConfigureApplicationServices(loggerFactory, context);

            using var host = new MicroserviceHostBuilder()
                .SetLoggerFactory(loggerFactory)
                .WithBusContext(context)
                .WithQueueName(QueueName)
                .UseConventions()
                .RegisterDependencies(ApplicationServices)
                .CreateHost();

            // Start and immediately pause host to ensure messages are coming in
            host.Start();
            host.Pause();

            // Fill cache database
            IDatabaseCacher databaseCacher = ApplicationServices.BuildServiceProvider().GetRequiredService<IDatabaseCacher>();
            databaseCacher.EnsureKlanten(replayContext);

            // Resume host
            host.Resume();

            /**
             * Keep the application running
             */
            new AutoResetEvent(false).WaitOne();
        }

        /// <summary>
        /// Service collection that is used to reference our global dependency collection
        /// </summary>
        internal static readonly IServiceCollection ApplicationServices = new ServiceCollection();

        /// <summary>
        /// Since we're hosting both a web server and a microservice host we have to get creative
        /// when it comes to registering dependencies.
        ///
        /// This function sets up all the services required for both the webhost and the microservicehost
        /// </summary>
        private static void ConfigureApplicationServices(ILoggerFactory loggerFactory, IBusContext<IConnection> context)
        {
            ApplicationServices.AddDbContext<BestelContext>(e =>
            {
                e.UseNpgsql(Environment.GetEnvironmentVariable(EnvNames.DbConnectionString));
                e.UseLoggerFactory(loggerFactory);
            });

            ApplicationServices.AddSingleton(context);
            ApplicationServices.AddSingleton(loggerFactory);
            ApplicationServices.AddSingleton<IBestelRepository, BestelRepository>();
            ApplicationServices.AddSingleton<IKlantRepository, KlantRepository>();
            ApplicationServices.AddSingleton<IBestellingService, BestellingService>();
            ApplicationServices.AddSingleton<IEventPublisher, EventPublisher>();
            ApplicationServices.AddSingleton<IHttpAgent, HttpAgent>();
            ApplicationServices.AddSingleton<IAuditAgent, AuditAgent>();
            ApplicationServices.AddSingleton<IEventReplayer, EventReplayer>();
            ApplicationServices.AddSingleton<IDatabaseCacher, DatabaseCacher>();
            ApplicationServices.AddSingleton(ApplicationServices);

            using IServiceScope serviceScope = ApplicationServices.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>()
                .CreateScope();

            BestelContext bestelContext = serviceScope.ServiceProvider.GetService<BestelContext>();
            bestelContext.Database.EnsureCreated();
        }
    }
}
