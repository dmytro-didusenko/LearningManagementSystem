using LearningManagementSystem.Core.Services.Interfaces;
using LearningManagementSystem.Domain.Models;
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
        public async Task<IActionResult> CreateGroup([FromBody] GroupCreationModel group)
        {
            ArgumentNullException.ThrowIfNull(group);
            var res = await _managementService.CreateGroupAsync(group);
            if (!res.IsSuccessful)
            {
                return BadRequest(res);
            }

            return Ok(res);
        }


        [HttpPost("move-student-to-group/{studentId}/{groupId}")]
        public async Task<IActionResult> MoveStudentToGroup(Guid studentId, Guid groupId)
        {
            await _managementService.MoveStudentToOtherGroupAsync(studentId, groupId);
            return Ok();
        }

        [HttpPut("update-group/{id}")]
        public async Task<IActionResult> UpdateUser(Guid groupId, [FromBody] GroupModel group)
        {
            await _managementService.UpdateGroupAsync(groupId, group);
            return NoContent();
        }

        [HttpPost("add-student-to-group/{userId}/{groupId}")]
        public async Task<IActionResult> AddStudentToGroup(Guid userId, Guid groupId)
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