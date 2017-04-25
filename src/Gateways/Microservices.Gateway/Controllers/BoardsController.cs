using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microservices.Gateway.Services;
using Microservices.Gateway.Contracts.Boards;

namespace Microservices.Gateway.Controllers
{
    [Route("api/v1/[controller]")]
    public class BoardsController : Controller
    {
        private readonly IBoardService _boardService;
        public BoardsController(IBoardService boardService)
        {
            _boardService = boardService ?? throw new ArgumentNullException(nameof(boardService));
        }

        // GET api/v1/boards
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Board>))]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetAsync()
        {
            var boards = await _boardService.ReadMyBoardsAsync();
            return Ok(boards);
        }

        // GET api/v1/boards/my-board-id
        [HttpGet("{boardId}")]
        [ProducesResponseType(200, Type = typeof(Board))]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetAsync(string boardId)
        {
            var board = await _boardService.ReadBoardAsync(boardId);
            if(board == null)
            {
                return NotFound();
            }
            return Ok(board);
        }
    }
}
