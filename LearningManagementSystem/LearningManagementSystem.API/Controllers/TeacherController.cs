using LearningManagementSystem.Core.Services.Interfaces;
using LearningManagementSystem.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [HttpPost]
        public async Task<IActionResult> AddTeacher([FromBody] TeacherCreationModel teacher)
        {
            var res = await _teacherService.AddAsync(teacher);
            if (!res.IsSuccessful)
            {
                return BadRequest(res);
            }

            return Ok(res);
        }

        [HttpGet]
        public IActionResult GetAllTeachers()
        {
            return Ok(_teacherService.GetAll());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveTeacher(Guid id)
        {
            await _teacherService.RemoveTeacherAsync(id);
            return NoContent();
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeacherById(Guid id)
        {
            return Ok(await _teacherService.GetByIdAsync(id));
        }
    }

}
