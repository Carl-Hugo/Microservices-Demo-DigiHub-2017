using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microservices.Gateway.Services;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Microservices.Gateway
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }
            Configuration = builder.Build();
            HostingEnvironment = env;
        }

        public IConfigurationRoot Configuration { get; }
        public IHostingEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add functionality to inject IOptions<T>
            services.AddOptions();

            // Add the Auth0 Settings object so it can be injected
            services.Configure<ServicesHostSettings>(Configuration.GetSection("Services"));

            // Gateway services
            if (HostingEnvironment.IsDevelopment())
            {
                services.AddSingleton(x =>
                {
                    var httpClient = new HttpClient(new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = (HttpRequestMessage, X509Certificate2, X509Chain, SslPolicyErrors) => true
                    });
                    return httpClient;
                });
            }
            else
            {
                services.AddSingleton<HttpClient>();
            }
            services.AddSingleton<ICurrentUserService, CurrentUserService>();
            services.AddSingleton<MemoryCache>();
            services.AddSingleton<IBoardService, BoardService>();
            services.AddSingleton<ICommunicationService, CommunicationService>();
            services.AddSingleton<IProxyService, ProxyService>();
            services.AddSingleton<IBoardsProxy, BoardsProxy>();
            services.AddSingleton<ICardsProxy, CardsProxy>();
            services.AddSingleton<IUsersProxy, UsersProxy>();

            // Add DynamicInternalServerError
            services.AddDynamicInternalServerError();
            services.AddDynamicInternalServerErrorSwagger();

            // JwtBearer
            services.AddAuthentication()
                .AddJwtBearer(options => {
                    var auth0Settings = Configuration.GetSection("Auth0").Get<Auth0Settings>();
                    if (auth0Settings == null) { throw new ArgumentNullException(nameof(auth0Settings)); }

                    options.Audience = auth0Settings.ClientId;
                    options.Authority = $"https://{auth0Settings.Domain}/";
                });

           // Add framework services.
           AuthorizationPolicy shouldBeAuthenticated = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();

            services.AddMvc(options =>
            {
                options.ConfigureDynamicInternalServerError();
                if (HostingEnvironment.IsDevelopment())
                {
                    options.SslPort = 44398;
                }
                options.Filters.Add(new RequireHttpsAttribute());
                options.Filters.Add(new AuthorizeFilter(shouldBeAuthenticated));
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Microservices Gateway DigiHub Démo 2017", Version = "v1" });
                c.AddDynamicInternalServerError();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Microservices Gateway DigiHub Démo 2017 API");
            });
        }
    }
}
