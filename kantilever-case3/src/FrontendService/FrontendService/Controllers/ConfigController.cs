using FrontendService.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace FrontendService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        private readonly IConfiguration _config;

        public ConfigController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                angular_authority = _config.GetValue<string>(EnvNames.AngularAuthority),
                angular_clientid = _config.GetValue<string>(EnvNames.AngularClientId),
                angular_response_type = _config.GetValue<string>(EnvNames.AngularReponseType),
                angular_redirect_uri = _config.GetValue<string>(EnvNames.AngularRedirectUri),
                angular_scope = _config.GetValue<string>(EnvNames.AngularScope),
                angular_post_logout_redirect_uri = _config.GetValue<string>(EnvNames.AngularPostLogoutRedirectUri),
            });
        }
    }
}
