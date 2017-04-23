using Microservices.Todo.Cards.Api.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Todo.Cards.Api.Services
{
    public interface ICardService
    {
        Task<IEnumerable<Card>> ReadAllAsync(string boardId);
        Task<Card> ReadOneAsync(string boardId, string cardId);
        Task<Card> InsertAsync(Card card);
        Task<Card> UpdateAsync(Card card);
        Task<Card> DeleteAsync(string boardId, string cardId);
    }
}
