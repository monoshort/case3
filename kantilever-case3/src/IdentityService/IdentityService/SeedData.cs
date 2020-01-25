using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using IdentityModel;
using IdentityService.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IdentityService
{
    public class SeedData
    {
        public void EnsureSeedData(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            ILogger<SeedData> logger = loggerFactory.CreateLogger<SeedData>();
            using IServiceScope scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            UserManager<IdentityUser> userMgr = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            EnsureSeedData(userMgr, logger);
        }

        private class SeedUser
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public Claim[] Claims { get; set; }
        }

        private readonly IEnumerable<SeedUser> _userData = new[]
        {
            new SeedUser
            {
                Username = "kees",
                Password = "Pass123$",
                Claims = new []
                {
                    new Claim(JwtClaimTypes.GivenName, "Kees de Koning"),
                    new Claim(Permissions.KanBestellingInpakken, AuthClaims.True),
                    new Claim(Permissions.KanBestellingKeuren, AuthClaims.True),
                    new Claim(Permissions.KanBetalingInvoeren, AuthClaims.True),
                    new Claim(Permissions.KanArtikelenBijbestellen, AuthClaims.True),
                    new Claim(Permissions.KanWanBetalersBekijken, AuthClaims.True),
                    new Claim(Permissions.KanBetalingRegistreren, AuthClaims.True)
                }
            },
            new SeedUser
            {
                Username = "esther",
                Password = "Pass123$",
                Claims = new []
                {
                    new Claim(JwtClaimTypes.GivenName, "Esther"),
                    new Claim(Permissions.KanBestellingInpakken, AuthClaims.True),
                    new Claim(Permissions.KanBestellingKeuren, AuthClaims.True),
                    new Claim(Permissions.KanBetalingInvoeren, AuthClaims.True),
                    new Claim(Permissions.KanArtikelenBijbestellen, AuthClaims.True),
                    new Claim(Permissions.KanWanBetalersBekijken, AuthClaims.True),
                    new Claim(Permissions.KanBetalingRegistreren, AuthClaims.True)
                }
            },
            new SeedUser
            {
                Username = "dennis",
                Password = "Pass123$",
                Claims = new []
                {
                    new Claim(JwtClaimTypes.GivenName, "Dennis"),
                    new Claim(Permissions.KanBestellingInpakken, AuthClaims.True),
                }
            },
            new SeedUser
            {
                Username = "timo",
                Password = "Pass123$",
                Claims = new []
                {
                    new Claim(JwtClaimTypes.GivenName, "Timo"),
                    new Claim(Permissions.KanBestellingInpakken, AuthClaims.True),
                }
            }
        };

        private void EnsureSeedData(UserManager<IdentityUser> userMgr, ILogger logger)
        {
            foreach (SeedUser userData in _userData)
            {
                logger.LogDebug($"Checking if user {userData.Username} exists");

                IdentityUser user = userMgr.FindByNameAsync(userData.Username).Result;

                if (user == null)
                {
                    logger.LogInformation($"Attempting to create user {userData.Username}");
                    user = new IdentityUser
                    {
                        UserName = userData.Username
                    };

                    var result = userMgr.CreateAsync(user, userData.Password).Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    result = userMgr.AddClaimsAsync(user, userData.Claims).Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    logger.LogInformation($"User {userData.Username} successfully created!");
                }
                else
                {
                    logger.LogInformation($"User {userData.Username} already exists, skipping");
                }
            };
        }
    }
}
