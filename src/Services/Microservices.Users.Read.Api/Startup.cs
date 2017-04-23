using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage.Table;
using ForEvolve.Azure.Storage.Table;
using Microservices.Users.Api.Contracts;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Microservices.Users.Read.Api
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
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseRouter(routeBuilder =>
            {
                // Create the user repository
                var userRepo = new TableStorageRepository<UserEntity>(new TableStorageSettings
                {
                    AccountKey = Configuration.GetValue<string>("UsersApi:Storage:AccountKey"),
                    AccountName = Configuration.GetValue<string>("UsersApi:Storage:AccountName"),
                    TableName = Configuration.GetValue<string>("UsersApi:Storage:UsersReadTableName")
                });

                var jsonSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                // GET api/users
                routeBuilder.MapGet("api/users", async (request, response, routeData) =>
                {
                    var entities = await userRepo.ReadAllAsync();
                    var users = entities.Select(x => new User
                    {
                        Id = x.RowKey,
                        FirstName = x.FirstName,
                        LastName = x.LastName
                    });
                    var jsonUsers = JsonConvert.SerializeObject(users, jsonSettings);
                    response.ContentType = "application/json";
                    await response.WriteAsync(jsonUsers);
                });

                // GET api/users/some-fancy-user-id
                routeBuilder.MapGet("api/users/{userId}", async (request, response, routeData) =>
                {
                    var userId = routeData.Values["userId"].ToString();
                    var entity = await userRepo.ReadOneAsync("DefaultUserPartitionKey", userId);
                    var users = new User
                    {
                        Id = entity.RowKey,
                        FirstName = entity.FirstName,
                        LastName = entity.LastName
                    };
                    var jsonUsers = JsonConvert.SerializeObject(users, jsonSettings);
                    response.ContentType = "application/json";
                    await response.WriteAsync(jsonUsers);
                });
            });
        }
    }
}
