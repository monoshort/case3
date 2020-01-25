using System;
using System.Diagnostics.CodeAnalysis;
using FrontendService.Agents;
using FrontendService.Agents.Abstractions;
using FrontendService.Constants;
using FrontendService.DAL;
using FrontendService.Repositories;
using FrontendService.Repositories.Abstractions;
using FrontendService.Seeding;
using FrontendService.Seeding.Abstractions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Minor.Miffy;
using Minor.Miffy.MicroServices.Commands;
using Minor.Miffy.MicroServices.Host;
using Minor.Miffy.RabbitMQBus;
using RabbitMQ.Client;

namespace FrontendService
{
    /**
     * This file is tested by integration tests
     */
    [ExcludeFromCodeCoverage]
    public static class Program
    {
        private const string QueueName = "FrontendService.Queue";

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
            databaseCacher.EnsureArtikelen();
            databaseCacher.EnsureKlanten(replayContext);
            databaseCacher.EnsureBestellingen(replayContext);

            // Resume host
            host.Resume();

            CreateWebHostBuilder(args).Build().Run();
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
            ApplicationServices.AddTransient<DatabaseCacher>();
            ApplicationServices.AddTransient<ICatalogusAgent, CatalogusAgent>();
            ApplicationServices.AddTransient<IVoorraadAgent, VoorraadAgent>();
            ApplicationServices.AddTransient<ICommandPublisher, CommandPublisher>();
            ApplicationServices.AddTransient<IHttpAgent, HttpAgent>();
            ApplicationServices.AddTransient<IBestellingAgent, BestellingAgent>();
            ApplicationServices.AddTransient<IDatabaseCacher, DatabaseCacher>();
            ApplicationServices.AddTransient<IAuditAgent, AuditAgent>();
            ApplicationServices.AddTransient<IEventReplayer, EventReplayer>();

            ApplicationServices.AddScoped<IArtikelRepository, ArtikelRepository>();
            ApplicationServices.AddScoped<IKlantRepository, KlantRepository>();
            ApplicationServices.AddScoped<IBestellingRepository, BestellingRepository>();

            ApplicationServices.AddScoped<IKlantAgent, KlantAgent>();
            ApplicationServices.AddScoped<IAccountAgent, AccountAgent>();

            ApplicationServices.AddSingleton(context);
            ApplicationServices.AddSingleton(loggerFactory);

            ApplicationServices.AddDbContext<FrontendContext>(e =>
            {
                e.UseNpgsql(Environment.GetEnvironmentVariable(EnvNames.DbConnectionString));
                e.UseLoggerFactory(loggerFactory);
            });

            ApplicationServices.AddSingleton(ApplicationServices);

            using var serviceScope = ApplicationServices.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>().CreateScope();
            FrontendContext frontendContext = serviceScope.ServiceProvider.GetService<FrontendContext>();
            frontendContext.Database.EnsureCreated();
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
