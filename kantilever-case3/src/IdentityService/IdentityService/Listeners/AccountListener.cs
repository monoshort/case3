using System;
using System.Linq;
using System.Security.Claims;
using IdentityService.Commands;
using IdentityService.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Minor.Miffy.MicroServices.Commands;

namespace IdentityService.Listeners
{
    public class AccountListener
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<AccountListener> _logger;

        public AccountListener(UserManager<IdentityUser> userManager, ILoggerFactory loggerFactory)
        {
            _userManager = userManager;
            _logger = loggerFactory.CreateLogger<AccountListener>();
        }

        [CommandListener(QueueNames.MaakAccountAan)]
        public MaakAccountAanCommand HandleMaakKlantAccountAan(MaakAccountAanCommand command)
        {
            _logger.LogInformation($"Received MaakKlantAanCommand with username {command.Username}");
            IdentityUser user = new IdentityUser
            {
                UserName = command.Username
            };

            _logger.LogDebug($"Creating user {command.Username}");
            IdentityResult createResult = _userManager.CreateAsync(user, command.Password).Result;

            if (!createResult.Succeeded)
            {
                string errorDescription = createResult.Errors.First().Description;
                _logger.LogError($"Creating user {command.Username} failed! Error: {errorDescription}");
                throw new Exception(errorDescription);
            }

            _logger.LogInformation($"Creating user {command.Username} succeeded, setting up default user claims");
            Claim[] userClaims = {
                new Claim(Permissions.KanArtikelenZien, AuthClaims.True),
                new Claim(Permissions.KanBestellen, AuthClaims.True)
            };

            _logger.LogDebug($"Adding claims to user {command.Username}");
            IdentityResult claimsResult = _userManager.AddClaimsAsync(user, userClaims).Result;

            if (claimsResult.Succeeded)
            {
                return command;
            }

            string claimsErrorDescription = claimsResult.Errors.First().Description;
            _logger.LogError($"Adding claims to user {command.Username} failed, error: {claimsErrorDescription}");
            throw new Exception(claimsErrorDescription);
        }

        [CommandListener(QueueNames.VerwijderAccount)]
        public VerwijderAccountCommand HandleVerwijderAccount(VerwijderAccountCommand command)
        {
            var user = _userManager.FindByNameAsync(command.Username).Result;
            var result = _userManager.DeleteAsync(user).Result;
            if (result.Succeeded)
            {
                return command;
            }
            throw new Exception(result.Errors.First().Description);
        }
    }
}
