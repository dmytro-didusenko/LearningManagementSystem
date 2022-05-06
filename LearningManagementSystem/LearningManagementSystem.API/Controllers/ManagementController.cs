using LearningManagementSystem.Core.Services.Interfaces;
using LearningManagementSystem.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ManagementController : ControllerBase
    {
        private readonly IManagementService _managementService;

        public ManagementController(IManagementService managementService)
        {
            _managementService = managementService;
        }



        [HttpPost("create-group")]
        public async Task<IActionResult> CreateGroup([FromBody] GroupModel group)
        {
            ArgumentNullException.ThrowIfNull(group);
            var res = await _managementService.CreateGroupAsync(group);
            if (!res.IsSuccessful)
            {
                return BadRequest(res);
            }

            return Ok(res);
        }


        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_managementService.GetAll());
        }
    }
}