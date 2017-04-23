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
    public interface IUserOperationQueuesService
    {
        ITableMessageQueueStorageRepository UserCreatedQueue { get; }
        ITableMessageQueueStorageRepository UserUpdatedQueue { get; }
        ITableMessageQueueStorageRepository UserDeletedQueue { get; }
    }
}