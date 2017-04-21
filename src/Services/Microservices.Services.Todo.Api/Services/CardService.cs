using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservices.Services.Todo.Api.Contracts;
using ForEvolve.Azure.Storage.Table;
using Microservices.Services.Todo.Api.Models;
using AutoMapper;

namespace Microservices.Services.Todo.Api.Services
{
    public class CardService : ICardService
    {
        private readonly ITableStorageRepository<CardEntity> _cardRepository;
        private readonly IMapper _mapper;

        public CardService(ITableStorageRepository<CardEntity> cardRepository, IMapper mapper)
        {
            _cardRepository = cardRepository ?? throw new ArgumentNullException(nameof(cardRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<Card> DeleteAsync(string boardId, string cardId)
        {
            var deletedEntity = await _cardRepository.RemoveAsync(boardId, cardId);
            var deletedCard = _mapper.Map<Card>(deletedEntity);
            return deletedCard;
        }

        public async Task<Card> InsertAsync(string boardId, CardData cardData)
        {
            var card = _mapper.Map<Card>(cardData);
            card.BoardId = boardId;
            card.Id = Guid.NewGuid().ToString();

            var entityToCreate = _mapper.Map<CardEntity>(card);
            var createdEntity = await _cardRepository.InsertOrReplaceAsync(entityToCreate);
            var createCard = _mapper.Map<Card>(createdEntity);
            return createCard;
        }

        public async Task<IEnumerable<Card>> ReadAllAsync(string boardId)
        {
            var all = await _cardRepository.ReadPartitionAsync(boardId);
            return all.Select(x => _mapper.Map<Card>(x));
        }

        public async Task<Card> ReadOneAsync(string boardId, string cardId)
        {
            var card = await _cardRepository.ReadOneAsync(boardId, cardId);
            return _mapper.Map<Card>(card);
        }

        public async Task<Card> UpdateAsync(string boardId, string cardId, CardData cardData)
        {
            var card = _mapper.Map<Card>(cardData);
            card.BoardId = boardId;
            card.Id = cardId;

            var entityToUpdate = _mapper.Map<CardEntity>(card);
            var updatedEntity = await _cardRepository.InsertOrMergeAsync(entityToUpdate);
            var updatedCard = _mapper.Map<Card>(updatedEntity);
            return updatedCard;
        }
    }
}
