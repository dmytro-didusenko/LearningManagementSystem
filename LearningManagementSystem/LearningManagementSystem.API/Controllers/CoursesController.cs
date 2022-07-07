using LearningManagementSystem.API.Extensions;
using LearningManagementSystem.Core.Services.Interfaces;
using LearningManagementSystem.Domain.Models.Course;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody] CourseModel course)
        {
            var res = await _courseService.AddAsync(course);
            return res.ToActionResult();
        }

        [HttpGet("{id}", Name = "GetCourseById")]
        public async Task<IActionResult> GetCourse(Guid id)
        {
            return Ok(await _courseService.GetByIdAsync(id));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(Guid id, [FromBody] CourseModel model)
        {
            var res = await _courseService.UpdateAsync(id, model);
            return res.ToActionResult();
        }

        [HttpGet]
        public IActionResult GetAllCourses()
        {
            return Ok(_courseService.GetAll());
        }
    }
}