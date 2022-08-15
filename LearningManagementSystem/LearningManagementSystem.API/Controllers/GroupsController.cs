using LearningManagementSystem.API.Attributes;
using LearningManagementSystem.API.Extensions;
using LearningManagementSystem.Core.Filters;
using LearningManagementSystem.Core.Services.Interfaces;
using LearningManagementSystem.Domain.Models.Group;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controllers
{
    [Authorized]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupService _groupService;
        public GroupsController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup([FromBody] GroupCreateModel group)
        {
            var res = await _groupService.AddAsync(group);
            return res.ToActionResult();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationFilter filter)
        {
            return Ok(await _groupService.GetAll(filter));
        }
      
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await _groupService.GetByIdAsync(id));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] GroupCreateModel model)
        {
            await _groupService.UpdateAsync(id, model);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
           await _groupService.RemoveAsync(id);
            return NoContent();
        }
    }
}
