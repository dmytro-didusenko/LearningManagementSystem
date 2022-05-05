using LearningManagementSystem.Core.Services.Interfaces;
using LearningManagementSystem.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> AddUserAsync([FromBody] UserModel user)
        {
            ArgumentNullException.ThrowIfNull(user);

            var res= await _userService.AddAsync(user);
            if (!res.IsSuccessful)
            {
                return BadRequest(res);
            }
            return CreatedAtRoute("GetUserById", new {Id = res.Data.Id}, res.Data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserModel model)
        {
            await _userService.UpdateAsync(id, model);
            return NoContent();
        }


        [HttpGet("{id}", Name = "GetUserById")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var res = await _userService.GetByIdAsync(id);
            
            return Ok(res);
        }


        [HttpGet]
        public async Task<IActionResult> GetUsersAsync([FromQuery] UserQueryModel? query = null)
        {
            ArgumentNullException.ThrowIfNull(query);

            return Ok(await _userService.GetByFilterAsync(query));
        }

    }
}