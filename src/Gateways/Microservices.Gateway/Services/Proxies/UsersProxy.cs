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
    public class UsersProxy : IUsersProxy
    {
        private readonly ICommunicationService _communicationService;
        private string ReadHost { get; }

        public UsersProxy(ICommunicationService communicationService)
        {
            _communicationService = communicationService ?? throw new ArgumentNullException(nameof(communicationService));
            ReadHost = communicationService.Services.UsersReadHost;
        }

        public async Task<User> ReadOneAsync(string userId)
        {
            var url = $"{ReadHost}api/users/{userId}";
            var user = await _communicationService.GetAsync<User>(url);
            return user;
        }
    }
}