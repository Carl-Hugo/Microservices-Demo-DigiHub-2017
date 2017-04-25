using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;
using Microservices.Todo.Boards.Api.Contracts;
using Newtonsoft.Json;
using Microservices.Todo.Cards.Api.Contracts;
using Microservices.Users.Api.Contracts;

namespace Microservices.Gateway.Services
{
    public interface IUsersProxy
    {
        Task<User> ReadOneAsync(string userId);
    }
}