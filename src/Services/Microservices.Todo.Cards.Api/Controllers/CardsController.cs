using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microservices.Todo.Cards.Api.Models;
using Microsoft.AspNetCore.Http;
using Microservices.Todo.Cards.Api.Services;
using Microservices.Todo.Cards.Api.Contracts;

namespace Microservices.Todo.Cards.Api.Controllers
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
        [ProducesResponseType(200, Type = typeof(IEnumerable<Card>))]
        public async Task<IActionResult> GetAsync(string boardId)
        {
            var cards = await _cardService.ReadAllAsync(boardId);
            return Ok(cards);
        }

        // GET api/todo/cards/some-board-id/some-card-id
        [HttpGet("{boardId}/{cardId}")]
        [ProducesResponseType(200, Type = typeof(Card))]
        [ProducesResponseType(404)]
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
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Card))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PostAsync([FromBody]Card card)
        {
            // Validate input and return 400 Bad Request if invalid
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var createdCard = await _cardService.InsertAsync(card);
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
        [HttpPut]
        [ProducesResponseType(200, Type = typeof(Card))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PutAsync([FromBody]Card card)
        {
            // Validate input and return 400 Bad Request if invalid
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var updatedCard = await _cardService.UpdateAsync(card);
            return Ok(updatedCard);
        }

        // DELETE api/todo/cards/some-board-id/some-card-id
        [HttpDelete("{boardId}/{cardId}")]
        [ProducesResponseType(200, Type = typeof(Card))]
        public async Task<IActionResult> DeleteAsync(string boardId, string cardId)
        {
            var deletedCard = await _cardService.DeleteAsync(boardId, cardId);
            return Ok(deletedCard);
        }
    }
}
