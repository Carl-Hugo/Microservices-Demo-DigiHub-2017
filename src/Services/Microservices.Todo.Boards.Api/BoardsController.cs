using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microservices.Todo.Boards.Api.Contracts;
using ForEvolve.Azure.Storage.Table;
using Microservices.Todo.Boards.Api.Models;
using System.ComponentModel.DataAnnotations;
using ForEvolve.Azure.Storage.Queue;
using ForEvolve.Azure.Storage.Queue.MessageType;

namespace Microservices.Todo.Boards.Api.Controllers
{
    [Route("api/todo/[controller]")]
    public class BoardsController : Controller
    {
        private readonly ITableStorageRepository<BoardEntity> _boardRepository;

        public BoardsController(ITableStorageRepository<BoardEntity> boardRepository)
        {
            _boardRepository = boardRepository ?? throw new ArgumentNullException(nameof(boardRepository));
        }

        // GET api/todo/boards/some-user-email@adress.com
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAsync(string userId)
        {
            // Get the data
            var entities = await _boardRepository.ReadPartitionAsync(userId);

            // Project the data in the expected contract format
            var boards = entities.Select(board => new Board
            {
                Id = board.RowKey,
                Name = board.Name,
                OwnerId = board.OwnerId
            });

            // Return 200 OK
            return Ok(boards);
        }

        // GET api/todo/boards/some-user-email@adress.com/some-board-id
        [HttpGet("{userId}/{boardId}")]
        public async Task<IActionResult> GetAsync(string userId, string boardId)
        {
            // Get the data
            var entity = await _boardRepository.ReadOneAsync(userId, boardId);

            // If the board is not found, return 404 NotFound
            if (entity == null)
            {
                return NotFound();
            }

            // Project the data in the expected contract format
            var board = new Board
            {
                Id = entity.RowKey,
                Name = entity.Name,
                OwnerId = entity.OwnerId
            };

            // Return 200 OK
            return Ok(board);
        }

        // POST api/todo/boards
        [HttpPost("{userId}")]
        public async Task<IActionResult> PostAsync(string userId, [FromBody]Board board)
        {
            // Validate input and return 400 Bad Request if invalid
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            // Create the valid data entity
            var rowKey = string.IsNullOrWhiteSpace(board.Id) ? Guid.NewGuid().ToString() : board.Id;
            var entity = new BoardEntity
            {
                PartitionKey = userId,
                RowKey = rowKey,
                Name = board.Name,
                OwnerId = board.OwnerId
            };

            // Insert the data
            var createdEntity = await _boardRepository.InsertOrReplaceAsync(entity);

            // Create the return contract
            var createdBoard = new Board
            {
                Id = createdEntity.RowKey,
                Name = createdEntity.Name,
                OwnerId = createdEntity.OwnerId
            };

            // Return 201 Created
            return CreatedAtAction(
                nameof(GetAsync), new
                {
                    userId = userId,
                    boardId = createdBoard.Id
                }, 
                createdBoard
            );
        }

        // PUT api/todo/boards
        [HttpPut("{userId}")]
        public async Task<IActionResult> PutAsync(string userId, [FromBody]Board board)
        {
            // Validate input and return 400 Bad Request if invalid
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            // Create the valid data entity
            var entity = new BoardEntity
            {
                PartitionKey = userId,
                RowKey = board.Id,
                Name = board.Name,
                OwnerId = board.OwnerId
            };

            // Update the data
            var updatedEntity = await _boardRepository.InsertOrMergeAsync(entity);

            // Create the return contract
            var updatedBoard = new Board
            {
                Id = updatedEntity.RowKey,
                Name = updatedEntity.Name,
                OwnerId = updatedEntity.OwnerId
            };

            // Return 200 OK
            return Ok(updatedBoard);
        }

        // DELETE api/todo/boards/some-board-id
        [HttpDelete("{userId}/{boardId}")]
        public async Task<IActionResult> DeleteAsync(string userId, string boardId, [FromServices]ITableMessageQueueStorageRepository deletedBoardQueue)
        {
            // Delete the data
            var deletedEntity = await _boardRepository.RemoveAsync(userId, boardId);

            // if the user is the board owner, queue the "sync operation".
            // This will remove all cards and associations to this board.
            if (deletedEntity.OwnerId == userId) // TODO: validate/rethink/refactor this logic if needed
            {
                await deletedBoardQueue.AddMessageAsync(new TableMessage
                {
                    PartitionKey = deletedEntity.PartitionKey,
                    RowKey = deletedEntity.RowKey
                });
            }

            // Create the return contract
            var deletedBoard = new Board
            {
                Id = deletedEntity.RowKey,
                Name = deletedEntity.Name,
                OwnerId = deletedEntity.OwnerId
            };

            // Return 200 OK
            return Ok(deletedBoard);
        }
    }
}
