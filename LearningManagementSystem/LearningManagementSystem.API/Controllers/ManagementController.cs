using LearningManagementSystem.Core.Services.Interfaces;
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

        [HttpPost("AddStudent/{studentId}/ToGroup/{groupId}")]
        public async Task<IActionResult> AddStudentToGroup(Guid studentId, Guid groupId)
        {
            await _managementService.AddStudentToGroupAsync(studentId, groupId);
            return Ok();
        }

        [HttpPost("AddCourse/{courseId}/ToGroup/{groupId}")]
        public async Task<IActionResult> AddCourseToGoup(Guid courseId, Guid groupId)
        {
            await _managementService.AddCourseToGroup(courseId, groupId);
            return Ok();
        }

        [HttpPost("AddSubject/{subjectId}/ToCourse/{courseId}")]
        public async Task<IActionResult> AddSubjectToCourse(Guid subjectId, Guid courseId)
        {
            await _managementService.AddSubjectToCourse(subjectId, courseId);
            return Ok();
        }
    }
}