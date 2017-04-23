using ForEvolve.Azure.Storage.Queue;
using ForEvolve.Azure.Storage.Table;
using Microservices.Todo.Boards.Api.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Todo.Boards.Api
{
    public static class BoardsStartupExtensions
    {
        public static void AddTodoBoardsServices(this IServiceCollection services, IConfigurationRoot configuration)
        {
            // Storage settings
            var accountKey = configuration.GetValue<string>("BoardsApi:Storage:AccountKey");
            var accountName = configuration.GetValue<string>("BoardsApi:Storage:AccountName");
            var tableStorageSettings = new TableStorageSettings
            {
                AccountKey = accountKey,
                AccountName = accountName,
                TableName = configuration.GetValue<string>("BoardsApi:Storage:BoardsTableName")
            };
            var boardDeletedQueueSettings = new QueueStorageSettings
            {
                AccountKey = accountKey,
                AccountName = accountName,
                QueueName = configuration.GetValue<string>("BoardsApi:Storage:BoardDeletedQueueName")
            };

            // Table message queue
            services.AddTableMessageQueueStorage(boardDeletedQueueSettings);

            // Board services
            services.AddSingleton<ITableStorageRepository<BoardEntity>>(serviceProvider =>
            {
                return new TableStorageRepository<BoardEntity>(tableStorageSettings);
            });
        }
    }
}
