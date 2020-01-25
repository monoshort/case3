using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using BackOfficeFrontendService.Constants;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace BackOfficeFrontendService
{
    /**
     * This class is tested through integration tests
     */
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddOpenIdConnect(options =>
            {
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.Authority = Environment.GetEnvironmentVariable(EnvNames.AuthenticationServerAddress);
                options.RequireHttpsMetadata = false;
                options.ClientId = Environment.GetEnvironmentVariable(EnvNames.AuthClientId);
                options.ClientSecret = Environment.GetEnvironmentVariable(EnvNames.AuthClientSecret);
                options.ResponseType = OpenIdConnectResponseType.Code;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.Scope.Add(AuthConfig.OpenId);
                options.Scope.Add(AuthConfig.Profile);
                options.Scope.Add(AuthConfig.KantileverBackofficeApi);
                options.SaveTokens = true;
                options.UsePkce = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = AuthConfig.Name,
                    RoleClaimType = AuthConfig.Groups,
                    ValidateIssuer = true
                };

                options.Events = new OpenIdConnectEvents
                {
                    OnTokenValidated = context =>
                    {
                        var handler = new JwtSecurityTokenHandler();
                        var accessToken = handler.ReadJwtToken(context.TokenEndpointResponse.AccessToken);

                        var appIdentity = new ClaimsIdentity(accessToken.Claims, JwtBearerDefaults.AuthenticationScheme);
                        context.Principal.AddIdentity(appIdentity);
                        return Task.CompletedTask;
                    }
                };
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthPolicies.KanBestellingInpakkenPolicy,
                    policy => policy.RequireClaim(AuthClaims.KanBestellingInpakkan, AuthClaims.True));

                options.AddPolicy(AuthPolicies.KanBestellingKeurenPolicy,
                    policy => policy.RequireClaim(AuthClaims.KanBestellingKeuren, AuthClaims.True));

                options.AddPolicy(AuthPolicies.KanBetalingRegistrerenPolicy,
                    policy => policy.RequireClaim(AuthClaims.KanBetalingRegistreren, AuthClaims.True));

                options.AddPolicy(AuthPolicies.KanArtikelenBijbestellenPolicy,
                    policy => policy.RequireClaim(AuthClaims.KanArtikelenBijbestellen, AuthClaims.True));

                options.AddPolicy(AuthPolicies.KanWanBetalersBekijken,
                    policy => policy.RequireClaim(AuthClaims.KanWanBetalersBekijken, AuthClaims.True));
            });

            services.AddControllersWithViews();

            // Add global services to webhost
            foreach (ServiceDescriptor serviceDescriptor in Program.ApplicationServices)
            {
                services.Add(serviceDescriptor);
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePages();
            }

            var supportedCultures = new[]
                {
                    new CultureInfo("nl-NL"),
                };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("nl-NL"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            }) ;

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
