using LearningManagementSystem.Core.Services.Interfaces;
using LearningManagementSystem.Domain.Contextes;
using LearningManagementSystem.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;

        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubject([FromBody] SubjectModel subject)
        {
            var res = await _subjectService.AddAsync(subject);
            if (!res.IsSuccessful)
            {
                return BadRequest(res);
            }
            return Ok(res); 
        }

        [HttpGet]
        public IActionResult GetAllSubjects()
        {
            return Ok(_subjectService.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await _subjectService.GetByIdAsync(id));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] SubjectModel model)
        {
            await _subjectService.UpdateAsync(id, model);
            return NoContent();
        }

        //[HttpGet("drop-db")]
        //public async Task<IActionResult> DropDb([FromServices] AppDbContext db)
        //{
        //    db.Students.RemoveRange(db.Students);
        //    db.Users.RemoveRange(db.Users);
        //    db.Courses.RemoveRange(db.Courses);
        //    db.Documents.RemoveRange(db.Documents);
        //    db.Groups.RemoveRange(db.Groups);
        //    db.Subjects.RemoveRange(db.Subjects);
        //    db.Teachers.RemoveRange(db.Teachers);
        //    await db.SaveChangesAsync();
        //    return Ok();
        //}
    }
}
