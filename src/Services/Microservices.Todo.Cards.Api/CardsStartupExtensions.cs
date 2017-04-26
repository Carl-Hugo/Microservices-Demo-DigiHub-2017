using AutoMapper;
using ForEvolve.Azure.Storage.Table;
using Microservices.Todo.Cards.Api.Models;
using Microservices.Todo.Cards.Api.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Todo.Cards.Api
{
    public static class CardsStartupExtensions
    {
        public static void AddTodoCardsServices(this IServiceCollection services, IConfigurationRoot configuration)
        {
            // Table storage settings
            var tableStorageSettings = new TableStorageSettings
            {
                AccountKey = configuration.GetValue<string>("CardsApi:Storage:AccountKey"),
                AccountName = configuration.GetValue<string>("CardsApi:Storage:AccountName"),
                TableName = configuration.GetValue<string>("CardsApi:Storage:CardsTableName")
            };

            // Todo services
            services.AddSingleton<ICardService, CardService>();
            services.AddSingleton<ITableStorageRepository<CardEntity>>(serviceProvider =>
            {
                return new TableStorageRepository<CardEntity>(tableStorageSettings);
            });
        }
    }
}
