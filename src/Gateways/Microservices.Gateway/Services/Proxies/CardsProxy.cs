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
    public class CardsProxy : ICardsProxy
    {
        private readonly ICommunicationService _communicationService;
        private string Host { get; }

        public CardsProxy(ICommunicationService communicationService)
        {
            _communicationService = communicationService ?? throw new ArgumentNullException(nameof(communicationService));
            Host = communicationService.Services.CardsHost;
        }

        public async Task<Card> ReadOneAsync(string boardId, string cardId)
        {
            var url = $"{Host}api/todo/cards/{boardId}/{cardId}";
            var card = await _communicationService.GetAsync<Card>(url);
            return card;
        }

        public async Task<IEnumerable<Card>> ReadAllAsync(string boardId)
        {
            var url = $"{Host}api/todo/cards/{boardId}";
            var cards = await _communicationService.GetAsync<Card[]>(url);
            return cards;
        }
    }
}