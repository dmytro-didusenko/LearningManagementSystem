using LearningManagementSystem.Core.Services.Interfaces;
using LearningManagementSystem.Domain.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent(StudentCreateModel model)
        {
            await _studentService.AddAsync(model);
            return Ok();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_studentService.GetAll());
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveTeacher(Guid id)
        {
            await _studentService.RemoveStudentAsync(id);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await _studentService.GetByIdAsync(id));
        }
    }
}
