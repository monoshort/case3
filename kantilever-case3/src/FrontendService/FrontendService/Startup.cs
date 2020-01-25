using System;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using FrontendService.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace FrontendService
{
    /**
     * This file is tested by integration tests
     */
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        private const string FrontendRootPath = "ClientApp/dist";
        private const string FrontendPath = "ClientApp/dist";
        private const string FrontendStartScript = "start";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("Bearer").AddJwtBearer(options =>
            {
                options.Authority = Environment.GetEnvironmentVariable(EnvNames.AuthenticationServerAddress);
                options.RequireHttpsMetadata = false;
                options.Audience = AuthConfig.Audience;
                
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        if (context.SecurityToken is JwtSecurityToken accessToken)
                        {
                            var appIdentity = new ClaimsIdentity(accessToken.Claims, JwtBearerDefaults.AuthenticationScheme);
                            context.Principal.AddIdentity(appIdentity);
                        }
            
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthPolicies.KanBestellenPolicy, policy => policy.RequireClaim(AuthClaims.KanBestellen, AuthClaims.True));
            });

            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    });

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = FrontendRootPath;
            });

            // Add global services to webhost
            foreach (ServiceDescriptor serviceDescriptor in Program.ApplicationServices)
            {
                services.Add(serviceDescriptor);
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseSpaStaticFiles();
            }
            app.UseStaticFiles();

            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = FrontendPath;

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(FrontendStartScript);
                }
            });
        }
    }
}
