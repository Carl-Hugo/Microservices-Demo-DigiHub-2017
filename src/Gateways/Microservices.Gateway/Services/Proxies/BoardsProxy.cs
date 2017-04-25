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
    public class BoardsProxy : IBoardsProxy
    {
        private readonly ICommunicationService _communicationService;
        private string Host { get; }

        public BoardsProxy(ICommunicationService communicationService)
        {
            _communicationService = communicationService ?? throw new ArgumentNullException(nameof(communicationService));
            Host = communicationService.Services.BoardsHost;
        }

        public async Task<Board> ReadOneAsync(string userId, string boardId)
        {
            var url = $"{Host}api/todo/boards/{userId}/{boardId}";
            var board = await _communicationService.GetAsync<Board>(url);
            return board;
        }

        public async Task<IEnumerable<Board>> ReadAllAsync(string userId)
        {
            var url = $"{Host}api/todo/boards/{userId}";
            var boards = await _communicationService.GetAsync<Board[]>(url);
            return boards;
        }
    }
}