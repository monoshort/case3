using System;
using System.Diagnostics.CodeAnalysis;
using BackOfficeFrontendService.Agents;
using BackOfficeFrontendService.Agents.Abstractions;
using BackOfficeFrontendService.Constants;
using BackOfficeFrontendService.DAL;
using BackOfficeFrontendService.Repositories;
using BackOfficeFrontendService.Repositories.Abstractions;
using BackOfficeFrontendService.Seeding;
using BackOfficeFrontendService.Seeding.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Minor.Miffy;
using Minor.Miffy.MicroServices.Commands;
using Minor.Miffy.MicroServices.Events;
using Minor.Miffy.MicroServices.Host;
using Minor.Miffy.RabbitMQBus;
using RabbitMQ.Client;

namespace BackOfficeFrontendService
{
    /**
     * This class is tested through integration tests
     */
    [ExcludeFromCodeCoverage]
    public static class Program
    {
        private const string QueueName = "BackOfficeFrontendService.Queue";

        public static void Main(string[] args)
        {
            using ILoggerFactory loggerFactory = LoggerFactory.Create(configure =>
            {
                configure.AddConsole().SetMinimumLevel(LogLevel.Debug);
            });

            MiffyLoggerFactory.LoggerFactory = loggerFactory;
            RabbitMqLoggerFactory.LoggerFactory = loggerFactory;

            using IBusContext<IConnection> context = new RabbitMqContextBuilder()
                .ReadFromEnvironmentVariables()
                .CreateContext();

            using var replayContext = new RabbitMqContextBuilder()
                .ReadFromEnvironmentVariables()
                .WithExchange(Environment.GetEnvironmentVariable(EnvNames.ReplayExchangeName))
                .CreateContext();

            ConfigureApplicationServices(loggerFactory, context);

            IMicroserviceHost host = new MicroserviceHostBuilder()
                .SetLoggerFactory(loggerFactory)
                .WithBusContext(context)
                .WithQueueName(QueueName)
                .RegisterDependencies(ApplicationServices)
                .UseConventions()
                .CreateHost();

            // Start and immediately pause host to ensure messages are coming in
            host.Start();
            host.Pause();

            // Fill cache database
            IDatabaseCacher databaseCacher = ApplicationServices.BuildServiceProvider().GetRequiredService<IDatabaseCacher>();
            databaseCacher.EnsureVoorraad();
            databaseCacher.EnsureKlanten(replayContext);
            databaseCacher.EnsureBestellingen(replayContext);

            // Resume host
            host.Resume();

            CreateHostBuilder(args).Build().Run();
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
            ApplicationServices.AddSingleton(context);
            ApplicationServices.AddSingleton(loggerFactory);

            ApplicationServices.AddScoped<IBestellingRepository, BestellingRepository>();
            ApplicationServices.AddScoped<IKlantRepository, KlantRepository>();
            ApplicationServices.AddScoped<IBestellingRepository, BestellingRepository>();
            ApplicationServices.AddScoped<IVoorraadRepository, VoorraadRepository>();
            ApplicationServices.AddScoped<ICommandPublisher, CommandPublisher>();
            ApplicationServices.AddScoped<IBestellingAgent, BestellingAgent>();
            ApplicationServices.AddScoped<IVoorraadAgent, VoorraadAgent>();
            ApplicationServices.AddScoped<IHttpAgent, HttpAgent>();
            ApplicationServices.AddScoped<IEventReplayer, EventReplayer>();
            ApplicationServices.AddScoped<IAuditAgent, AuditAgent>();
            ApplicationServices.AddScoped<ICatalogusAgent, CatalogusAgent>();
            ApplicationServices.AddScoped<IDatabaseCacher, DatabaseCacher>();
            ApplicationServices.AddScoped<IEventPublisher, EventPublisher>();
            ApplicationServices.AddScoped<IDatabaseCacher, DatabaseCacher>();

            ApplicationServices.AddDbContext<BackOfficeContext>(e =>
            {
                e.UseNpgsql(Environment.GetEnvironmentVariable(EnvNames.DbConnectionString));
                e.UseLoggerFactory(loggerFactory);
            });

            ApplicationServices.AddSingleton(ApplicationServices);

            using IServiceScope serviceScope = ApplicationServices.BuildServiceProvider()
                .GetRequiredService<IServiceScopeFactory>().CreateScope();

            BackOfficeContext backOfficeContext = serviceScope.ServiceProvider.GetService<BackOfficeContext>();
            backOfficeContext.Database.EnsureCreated();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}
