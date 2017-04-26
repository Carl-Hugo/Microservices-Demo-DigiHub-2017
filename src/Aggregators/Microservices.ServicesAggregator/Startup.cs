using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microservices.Todo.Boards.Api;
using Microservices.Todo.Cards.Api;
using Microservices.Users.Write.Api;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Swagger;
using AutoMapper;

namespace Microservices.ServicesAggregator
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
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // AutoMapper
            services.AddSingleton(serviceProvider =>
            {
                return new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile(new CardsMapperProfile());
                    cfg.AddProfile(new UsersMapperProfile());
                }).CreateMapper();
            });

            // Add todo api services
            services.AddTodoBoardsServices(Configuration);
            services.AddTodoCardsServices(Configuration);
            services.AddUsersWriteServices(Configuration);

            // Add DynamicInternalServerError
            services.AddDynamicInternalServerError();

            // Add framework services.
            services.AddMvc(options =>
            {
                options.ConfigureDynamicInternalServerError();
                if (HostingEnvironment.IsDevelopment())
                {
                    options.SslPort = 44349;
                }
                options.Filters.Add(new RequireHttpsAttribute());
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Microservices Aggregator DigiHub Démo 2017", Version = "v1" });
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Microservices Aggregator DigiHub Démo 2017 API");
            });
        }
    }
}
