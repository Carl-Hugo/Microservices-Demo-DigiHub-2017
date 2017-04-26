using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microservices.Users.Api.Contracts;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Microservices.Users.Write.Api
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        // POST api/values
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(User))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PostAsync([FromBody]User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var createdUser = await _userService.InsertAsync(user);
            
            // TODO: refactor the URI, it should not be a literal.
            return Created(
                $"http://localhost:28080/api/users/{createdUser.Id}",
                createdUser
            );
        }

        // PUT api/values/5
        [HttpPut]
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PutAsync([FromBody]User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var updatedUser = await _userService.UpdateAsync(user);
            return Ok(updatedUser);
        }

        // DELETE api/values/5
        [HttpDelete("{userId}")]
        [ProducesResponseType(200, Type = typeof(User))]
        public async Task<IActionResult> DeleteAsync(string userId)
        {
            var deletedUser = await _userService.DeleteAsync(userId);
            return Ok(deletedUser);
        }
    }
}
