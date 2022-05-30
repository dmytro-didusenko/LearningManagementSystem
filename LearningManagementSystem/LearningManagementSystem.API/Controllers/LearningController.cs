using LearningManagementSystem.Core.Services.Interfaces;
using LearningManagementSystem.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class LearningController : ControllerBase
    {
        private readonly ILearningService _learningService;

        public LearningController(ILearningService learningService)
        {
            _learningService = learningService;
        }

        [HttpPost("HomeTasks")]
        public async Task<IActionResult> AddHomeTask([FromBody] HomeTaskCreateModel model)
        {
            var res = await _learningService.CreateHomeTaskAsync(model);
            if (!res.IsSuccessful)
            {
                return BadRequest(res);
            }
            return Ok(res);
        }

        [HttpPost("Topics")]
        public async Task<IActionResult> CreateTopic([FromBody] TopicCreateModel model)
        {
            var res = await _learningService.CreateTopicAsync(model);
            if (!res.IsSuccessful)
            {
                return BadRequest(res);
            }
            return Ok(res);
        }

        [HttpPost("TaskAnswers")]
        public async Task<IActionResult> AddTaskAnswer([FromBody] TaskAnswerModel model)
        {
            var res = await _learningService.AddTaskAnswerAsync(model);
            if (!res.IsSuccessful)
            {
                return BadRequest(res);
            }
            return Ok(res);
        }

        [HttpGet("TaskAnswers/{homeTaskId}")]
        public IActionResult GetTaskAnswersByHomeTaskId(Guid homeTaskId)
        {
            return Ok(_learningService.GetTaskAnswersByHomeTaskId(homeTaskId));
        }

        [HttpGet("HomeTasks/{topicId}")]
        public async Task<IActionResult> GetHomeTaskByTopicId(Guid topicId)
        {
            return Ok(await _learningService.GetHomeTaskByIdAsync(topicId));
        }

        [HttpGet("Topics/{subjectId}")]
        public IActionResult GetTopicsBySubjectId(Guid subjectId)
        {
            return Ok(_learningService.GetTopicsBySubjectId(subjectId));
        }

        [HttpPut("Topics/{id}")]
        public async Task<IActionResult> UpdateTopic(Guid id, [FromBody] TopicModel model)
        {
            var res = await _learningService.UpdateTopicAsync(id, model);
            if (!res.IsSuccessful)
            {
                return BadRequest(res);
            }
            return Ok(res);
        }

        [HttpPut("HomeTasks/{id}")]
        public async Task<IActionResult> UpdateHomeTask(Guid id, [FromBody] HomeTaskModel model)
        {
            var res = await _learningService.UpdateHomeTaskAsync(id, model);
            if (!res.IsSuccessful)
            {
                return BadRequest(res);
            }
            return Ok(res);
        }

        [HttpPut("TaskAnswers/{id}")]
        public async Task<IActionResult> UpdateTaskAnswer(Guid id, [FromBody] TaskAnswerUpdateModel model)
        {
            var res = await _learningService.UpdateTaskAnswerAsync(id, model);
            if(!res.IsSuccessful)
            {
                return BadRequest(res);
            }

            return Ok(res);
        }

        [HttpDelete("HomeTasks/{topicId}")]
        public async Task<IActionResult> RemoveHomeTask(Guid topicId)
        {
            var res = await _learningService.RemoveHomeTaskAsync(topicId);
            if (!res.IsSuccessful)
            {
                return BadRequest(res);
            }

            return Ok(res);
        }

        [HttpPost("Grades/{taskAnswerId}")]
        public async Task<IActionResult> AddGrade(Guid taskAnswerId, [FromBody] GradeModel model)
        {
            var res = await _learningService.AddGradeAsync(taskAnswerId, model);
            if (!res.IsSuccessful)
            {
                return BadRequest(res);
            }

            return Ok(res);
        }

        [HttpGet("Grades/{studentId}")]
        public async Task<IActionResult> GetStudentGrades(Guid studentId)
        {
            var res = await _learningService.GetStudentGrades(studentId);
            return Ok(res);
        }

        [HttpGet("Grades/Subject/{subjectId}/Student/{studentId}")]
        public IActionResult GetStudentGradesBySubject(Guid subjectId, Guid studentId)
        {
            return Ok(_learningService.GetStudentGradesBySubjectId(studentId, subjectId));
        }

    }
}
