using LearningManagementSystem.Core.Services.Interfaces;
using LearningManagementSystem.Domain.Contextes;
using LearningManagementSystem.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpPost("add-student-to-group/{groupId}/{userId}")]
        public async Task<IActionResult> AddStudentToGroup(Guid groupId, Guid userId)
        {
            await _managementService.AddStudentToGroupAsync(groupId, userId);
            return Ok();
        }

        [HttpGet("get-groups")]
        public IActionResult Get()
        {
            return Ok(_managementService.GetAll());
        }

    }
}