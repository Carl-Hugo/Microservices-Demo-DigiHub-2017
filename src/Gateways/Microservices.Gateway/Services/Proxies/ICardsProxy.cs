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
    public interface ICardsProxy
    {
        Task<Card> ReadOneAsync(string boardId, string cardId);
        Task<IEnumerable<Card>> ReadAllAsync(string boardId);
    }
}