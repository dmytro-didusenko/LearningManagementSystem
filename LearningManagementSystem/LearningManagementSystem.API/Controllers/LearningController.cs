using LearningManagementSystem.API.Extensions;
using LearningManagementSystem.Core.Services.Interfaces;
using LearningManagementSystem.Domain.Models.HomeTask;
using LearningManagementSystem.Domain.Models.Topic;
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
            return res.ToActionResult();
        }

        [HttpPost("Topics")]
        public async Task<IActionResult> CreateTopic([FromBody] TopicCreateModel model)
        {
            var res = await _learningService.CreateTopicAsync(model);
            return res.ToActionResult();
        }

        [HttpPost("TaskAnswers")]
        public async Task<IActionResult> AddTaskAnswer([FromBody] TaskAnswerModel model)
        {
            var res = await _learningService.AddTaskAnswerAsync(model);
            return res.ToActionResult();
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
        public async Task<IActionResult> GetTopicsBySubjectId(Guid subjectId)
        {
            return Ok(await _learningService.GetTopicsBySubjectId(subjectId));
        }

        [HttpPut("Topics/{id}")]
        public async Task<IActionResult> UpdateTopic(Guid id, [FromBody] TopicModel model)
        {
            var res = await _learningService.UpdateTopicAsync(id, model);
            return res.ToActionResult();
        }

        [HttpPut("HomeTasks/{id}")]
        public async Task<IActionResult> UpdateHomeTask(Guid id, [FromBody] HomeTaskModel model)
        {
            var res = await _learningService.UpdateHomeTaskAsync(id, model);
            return res.ToActionResult();
        }

        [HttpPut("TaskAnswers/{id}")]
        public async Task<IActionResult> UpdateTaskAnswer(Guid id, [FromBody] TaskAnswerUpdateModel model)
        {
            var res = await _learningService.UpdateTaskAnswerAsync(id, model);
            return res.ToActionResult();
        }

        [HttpDelete("HomeTasks/{topicId}")]
        public async Task<IActionResult> RemoveHomeTask(Guid topicId)
        {
            var res = await _learningService.RemoveHomeTaskAsync(topicId);
            if (!res.IsSuccessful)
            {
                return BadRequest();
            }

            return Ok(res);
        }

        [HttpPost("Grades/{taskAnswerId}")]
        public async Task<IActionResult> AddGrade(Guid taskAnswerId, [FromBody] GradeModel model)
        {
            var res = await _learningService.AddGradeAsync(taskAnswerId, model);
            return res.ToActionResult();
        }

        [HttpGet("Grades/{studentId}")]
        public async Task<IActionResult> GetStudentGrades(Guid studentId)
        {
            var res = await _learningService.GetStudentGrades(studentId);
            return Ok(res);
        }
    }
}
