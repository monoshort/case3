using System;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityService.Constants;
using IdentityService.DAL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Minor.Miffy;
using Minor.Miffy.MicroServices.Host;
using Minor.Miffy.RabbitMQBus;

namespace IdentityService
{
    public class Program
    {
        private const string QueueName = "Kantilever.IdentityService.Queue";

        public static void Main(string[] args)
        {
            using var loggerFactory = LoggerFactory.Create(configure =>
            {
                configure.AddConsole().SetMinimumLevel(LogLevel.Debug);
            });

            MiffyLoggerFactory.LoggerFactory = loggerFactory;
            RabbitMqLoggerFactory.LoggerFactory = loggerFactory;

            ConfigureApplicationServices(loggerFactory);

            using var context = new RabbitMqContextBuilder()
                .ReadFromEnvironmentVariables()
                .CreateContext();

            using var miffyHost = new MicroserviceHostBuilder()
                .SetLoggerFactory(loggerFactory)
                .WithBusContext(context)
                .WithQueueName(QueueName)
                .RegisterDependencies(ApplicationServices)
                .UseConventions()
                .CreateHost();

            miffyHost.Start();

            IHost host = CreateHostBuilder(args).Build();

            SeedData seedData = new SeedData();
            seedData.EnsureSeedData(ApplicationServices.BuildServiceProvider(), loggerFactory);
            host.Run();
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
        private static void ConfigureApplicationServices(ILoggerFactory loggerFactory)
        {
            string connectionString = Environment.GetEnvironmentVariable(EnvNames.DbConnectionString);
            Config identityConfig = new Config(loggerFactory);

            ApplicationServices.AddSingleton(loggerFactory);

            ApplicationServices.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
                options.UseLoggerFactory(loggerFactory);
            });

            ApplicationServices.AddIdentity<IdentityUser, IdentityRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 1;
                    options.Password.RequiredUniqueChars = 0;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            ApplicationServices.AddIdentityServer(options =>
                {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = dbBuilder =>
                {
                    dbBuilder.UseNpgsql(connectionString, e => e.MigrationsAssembly(typeof(SeedData).Assembly.FullName));
                    dbBuilder.UseLoggerFactory(loggerFactory);
                };
                options.EnableTokenCleanup = true;
            })
            .AddDeveloperSigningCredential()
            .AddInMemoryIdentityResources(identityConfig.Ids)
            .AddInMemoryApiResources(identityConfig.Apis)
            .AddInMemoryClients(identityConfig.Clients)
            .AddAspNetIdentity<IdentityUser>()
            .AddDeveloperSigningCredential();

            using IServiceScope scope = ApplicationServices.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>().CreateScope();
            scope.ServiceProvider.GetService<PersistedGrantDbContext>().Database.Migrate();
            scope.ServiceProvider.GetService<ApplicationDbContext>().Database.Migrate();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
    }
}
