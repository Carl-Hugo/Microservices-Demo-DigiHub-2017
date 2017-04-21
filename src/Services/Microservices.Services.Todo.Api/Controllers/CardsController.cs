using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microservices.Services.Todo.Api.Models;
using Microsoft.AspNetCore.Http;
using Microservices.Services.Todo.Api.Services;
using Microservices.Services.Todo.Api.Contracts;

namespace Microservices.Services.Todo.Api.Controllers
{
    [Route("api/todo/[controller]")]
    public class CardsController : Controller
    {
        private readonly ICardService _cardService;

        public CardsController(ICardService cardService)
        {
            _cardService = cardService ?? throw new ArgumentNullException(nameof(cardService));
        }

        // GET api/todo/cards/some-board-id
        [HttpGet("{boardId}")]
        public async Task<IActionResult> GetAsync(string boardId)
        {
            var cards = await _cardService.ReadAllAsync(boardId);
            return Ok(cards);
        }

        // GET api/todo/cards/some-board-id/some-card-id
        [HttpGet("{boardId}/{cardId}")]
        public async Task<IActionResult> GetAsync(string boardId, string cardId)
        {
            var card = await _cardService.ReadOneAsync(boardId, cardId);
            if (card == null)
            {
                return NotFound();
            }
            return Ok(card);
        }

        // POST api/todo/cards/some-board-id
        [HttpPost("{boardId}/")]
        public async Task<IActionResult> PostAsync(string boardId, [FromBody]CardData cardData)
        {
            var createdCard = await _cardService.InsertAsync(boardId, cardData);
            return CreatedAtAction(
                nameof(GetAsync), 
                new
                {
                    boardId = createdCard.BoardId,
                    cardId = createdCard.Id
                }, 
                createdCard
            );
        }

        // PUT api/todo/cards/some-board-id/some-card-id
        [HttpPut("{boardId}/{cardId}")]
        public async Task<IActionResult> PutAsync(string boardId, string cardId, [FromBody]CardData cardData)
        {
            var updatedCard = await _cardService.UpdateAsync(boardId, cardId, cardData);
            return Ok(updatedCard);
        }

        // DELETE api/todo/cards/some-board-id/some-card-id
        [HttpDelete("{boardId}/{cardId}")]
        public async Task<IActionResult> DeleteAsync(string boardId, string cardId)
        {
            var deletedCard = await _cardService.DeleteAsync(boardId, cardId);
            return Ok(deletedCard);
        }
    }
}
