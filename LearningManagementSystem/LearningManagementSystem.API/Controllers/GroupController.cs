using LearningManagementSystem.Core.Services.Interfaces;
using LearningManagementSystem.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;
        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup([FromBody] GroupCreationModel group)
        {
            var res = await _groupService.AddAsync(group);
            if (!res.IsSuccessful)
            {
                return BadRequest(res);
            }
            return CreatedAtRoute("GetById", new { Id = res.Data.Id }, res.Data);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_groupService.GetAll());
        }

        [HttpGet("{id}", Name = "GetById")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await _groupService.GetByIdAsync(id));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] GroupCreationModel model)
        {
            await _groupService.UpdateAsync(id, model);
            return NoContent();
        }

    }
}
