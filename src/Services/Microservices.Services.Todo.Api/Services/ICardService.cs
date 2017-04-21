using Microservices.Services.Todo.Api.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.Services.Todo.Api.Services
{
    public interface ICardService
    {
        Task<IEnumerable<Card>> ReadAllAsync(string boardId);
        Task<Card> ReadOneAsync(string boardId, string cardId);
        Task<Card> InsertAsync(string boardId, CardData cardData);
        Task<Card> UpdateAsync(string boardId, string cardId, CardData cardData);
        Task<Card> DeleteAsync(string boardId, string cardId);
    }
}
