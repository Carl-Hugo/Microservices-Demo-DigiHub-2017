using Microservices.Gateway.Contracts.Boards;
using Microservices.Gateway.Contracts.Cards;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microservices.Gateway.Services
{
    public class BoardService : IBoardService
    {
        private readonly IProxyService _proxies;
        private readonly ICurrentUserService _currentUserService;

        public BoardService(IProxyService proxyService, ICurrentUserService currentUserService)
        {
            _proxies = proxyService ?? throw new ArgumentNullException(nameof(proxyService));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        }

        public async Task<IEnumerable<Board>> ReadMyBoardsAsync()
        {
            var result = new List<Board>();
            var boards = await _proxies.Boards.ReadAllAsync(_currentUserService.Id);
            foreach (var board in boards)
            {
                var b = await LoadBoardDependencies(board);
                result.Add(b);
            }
            return result;
        }

        public async Task<Board> ReadBoardAsync(string boardId)
        {
            var board = await _proxies.Boards.ReadOneAsync(_currentUserService.Id, boardId);
            var b = await LoadBoardDependencies(board);
            return b;
        }

        private async Task<Board> LoadBoardDependencies(Todo.Boards.Api.Contracts.Board board)
        {
            // Create the board
            var b = new Board
            {
                Id = board.Id,
                Name = board.Name
            };

            // Load the owner info
            var owner = await _proxies.Users.ReadOneAsync(board.OwnerId);
            b.Owner = new Contracts.Users.PublicUser
            {
                Id = board.OwnerId,
                Name = $"{owner.FirstName} {owner.LastName}"
            };

            // Load the cards
            var cards = await _proxies.Cards.ReadAllAsync(board.Id);
            b.Cards = cards.Select(card => new CardExcerpt
            {
                Id = card.Id,
                Name = card.Name
            });
            return b;
        }
    }

    public interface IBoardService
    {
        Task<IEnumerable<Board>> ReadMyBoardsAsync();
        Task<Board> ReadBoardAsync(string boardId);
    }
}
