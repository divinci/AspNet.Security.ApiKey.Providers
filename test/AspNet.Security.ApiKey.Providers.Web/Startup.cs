using System;
using System.Diagnostics;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.Security.ApiKey.Providers.Events;
using AspNet.Security.ApiKey.Providers.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspNet.Security.ApiKey.Providers.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = ApiKeyDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = ApiKeyDefaults.AuthenticationScheme;
                })
                .AddApiKey(options =>
                {
                    options.Header = "Authorization";
                    options.HeaderKey = "ApiKey";
                    options.Events = new ApiKeyEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            var ex = context.Exception;

                            Trace.TraceError(ex.Message);

                            context.Fail(ex);

                            return Task.CompletedTask;
                        },
                        OnApiKeyValidated = context =>
                        {
                            if (context.ApiKey == "123")
                            {
                                context.Principal = new ClaimsPrincipal();

                                context.Success();
                            }
                            else if (context.ApiKey == "789")
                            {
                                throw new NotSupportedException("You must upgrade.");
                            }

                            return Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            if (context.AuthenticateFailure is NotSupportedException)
                            {
                                context.StatusCode = HttpStatusCode.UpgradeRequired;
                            }

                            return Task.CompletedTask;
                        }
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
