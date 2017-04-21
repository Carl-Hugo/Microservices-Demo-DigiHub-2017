using AutoMapper;
using ForEvolve.Azure.Storage.Table;
using Microservices.Services.Todo.Api.Models;
using Microservices.Services.Todo.Api.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Services.Todo.Api
{
    public static class TodoStartupExtensions
    {
        public static void AddTodoServices(this IServiceCollection services, IConfigurationRoot configuration)
        {
            // AutoMapper
            services.AddSingleton(serviceProvider =>
            {
                return new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile(new TodoMapperProfile());
                }).CreateMapper();
            });

            // Todo services
            services.AddSingleton<ICardService, CardService>();
            services.AddSingleton<ITableStorageRepository<CardEntity>>(serviceProvider =>
            {
                var settings = new TableStorageSettings
                {
                    AccountKey = configuration.GetValue<string>("TodoApi:Table:AccountKey"),
                    AccountName = configuration.GetValue<string>("TodoApi:Table:AccountName"),
                    TableName = configuration.GetValue<string>("TodoApi:Table:CardsTableName")
                };
                return new TableStorageRepository<CardEntity>(settings);
            });
        }
    }
}
