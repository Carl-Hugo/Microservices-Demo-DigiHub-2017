using AutoMapper;
using ForEvolve.Azure.Storage.Queue;
using ForEvolve.Azure.Storage.Table;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Users.Write.Api
{
    public static class UsersWriteStartupExtensions
    {
        public static void AddUsersWriteServices(this IServiceCollection services, IConfigurationRoot configuration)
        {
            // Load table/queue storage info from configs
            var accountKey = configuration.GetValue<string>("UsersApi:Storage:AccountKey");
            var accountName = configuration.GetValue<string>("UsersApi:Storage:AccountName");
            var tableStorageSettings = new TableStorageSettings
            {
                AccountKey = accountKey,
                AccountName = accountName,
                TableName = configuration.GetValue<string>("UsersApi:Storage:UsersWriteTableName")
            };

            // User services
            services.AddSingleton<IUserOperationQueuesService>(serviceProvider =>
            {
                var userCreatedQueue = CreateTableMessageQueueStorageRepository(new QueueStorageSettings
                {
                    AccountKey = accountKey,
                    AccountName = accountName,
                    QueueName = configuration.GetValue<string>("UsersApi:Storage:UserCreatedQueueName")
                });
                var userUpdatedQueue = CreateTableMessageQueueStorageRepository(new QueueStorageSettings
                {
                    AccountKey = accountKey,
                    AccountName = accountName,
                    QueueName = configuration.GetValue<string>("UsersApi:Storage:UserUpdatedQueueName")
                });
                var userDeletedQueue = CreateTableMessageQueueStorageRepository(new QueueStorageSettings
                {
                    AccountKey = accountKey,
                    AccountName = accountName,
                    QueueName = configuration.GetValue<string>("UsersApi:Storage:UserDeletedQueueName")
                });

                return new UserOperationQueuesService(userCreatedQueue, userUpdatedQueue, userDeletedQueue);
            });
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<ITableStorageRepository<UserEntity>>(serviceProvider =>
            {
                return new TableStorageRepository<UserEntity>(tableStorageSettings);
            });
        }

        private static ITableMessageQueueStorageRepository CreateTableMessageQueueStorageRepository(QueueStorageSettings settings)
        {
            return new TableMessageQueueStorageRepository(new QueueStorageRepository(settings));
        }
    }
}
