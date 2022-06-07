using LearningManagementSystem.API.Extensions;
using LearningManagementSystem.Core.Services.Interfaces;
using LearningManagementSystem.Domain.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        private readonly ITeacherService _teacherService;

        public TeachersController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [HttpPost]
        public async Task<IActionResult> AddTeacher([FromBody] TeacherCreateModel teacher)
        {
            var res = await _teacherService.AddAsync(teacher);
            return res.ToActionResult();
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
