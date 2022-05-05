using LearningManagementSystem.Core.Services.Interfaces;
using LearningManagementSystem.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserModel user)
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
        public Task<IActionResult> UpdateUser(Guid id, [FromBody] UserModel model)
        {

        }


        [HttpGet("{id}", Name = "GetUserById")]
        public async Task<IActionResult> Get(Guid id)
        {
            var res = await _userService.GetById(id);
            if (!res.IsSuccessful)
            {
                return BadRequest(res);
            }
            return Ok(res);
        }


        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(_userService.GetAll());
        }

    }
}