using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;
using Microservices.Todo.Boards.Api.Contracts;
using Newtonsoft.Json;
using Microservices.Todo.Cards.Api.Contracts;

namespace Microservices.Gateway.Services
{
    public interface IBoardsProxy
    {
        Task<Board> ReadOneAsync(string userId, string boardId);
        Task<IEnumerable<Board>> ReadAllAsync(string userId);
    }
}