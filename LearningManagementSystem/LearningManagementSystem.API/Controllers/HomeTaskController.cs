using LearningManagementSystem.Core.Services.Interfaces;
using LearningManagementSystem.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class HomeTaskController : ControllerBase
    {
        private readonly IHomeTaskService _homeTaskService;

        public HomeTaskController(IHomeTaskService homeTaskService)
        {
            _homeTaskService = homeTaskService;

        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateHomeTask([FromBody] HomeTaskDto model)
        {
            var res = await _homeTaskService.CreateHomeTaskAsync(model);
            if (!res.IsSuccessful)
            {
                return BadRequest(res);
            }
            return Ok(res);
        }

        [HttpGet("GetAllTasks")]
        public IActionResult GetAllTasks()
        {
            return Ok(_homeTaskService.GetAllHomeTasks());
        }

    }
}
