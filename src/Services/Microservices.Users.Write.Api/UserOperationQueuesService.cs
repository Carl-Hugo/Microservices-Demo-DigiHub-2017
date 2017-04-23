using System;
using System.Linq;
using System.Collections.Generic;
using ForEvolve.Azure.Storage.Table;
using ForEvolve.Azure.Storage.Queue;
using Microservices.Users.Api.Contracts;
using System.Threading.Tasks;
using AutoMapper;
using ForEvolve.Azure.Storage.Queue.MessageType;

namespace Microservices.Users.Write.Api
{
    public class UserOperationQueuesService : IUserOperationQueuesService
    {
        public UserOperationQueuesService(
            ITableMessageQueueStorageRepository userCreatedQueue,
            ITableMessageQueueStorageRepository userUpdatedQueue,
            ITableMessageQueueStorageRepository userDeletedQueue
            )
        {
            UserCreatedQueue = userCreatedQueue ?? throw new ArgumentNullException(nameof(userCreatedQueue));
            UserUpdatedQueue = userUpdatedQueue ?? throw new ArgumentNullException(nameof(userUpdatedQueue));
            UserDeletedQueue = userDeletedQueue ?? throw new ArgumentNullException(nameof(userDeletedQueue));
        }

        public ITableMessageQueueStorageRepository UserCreatedQueue { get; }
        public ITableMessageQueueStorageRepository UserUpdatedQueue { get; }
        public ITableMessageQueueStorageRepository UserDeletedQueue { get; }
    }
}