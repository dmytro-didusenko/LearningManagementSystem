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

        [HttpPost("Create/HomeTask")]
        public async Task<IActionResult> AddHomeTask([FromBody] HomeTaskCreateModel model)
        {
            var res = await _learningService.CreateHomeTaskAsync(model);
            if (!res.IsSuccessful)
            {
                return BadRequest(res);
            }
            return Ok(res);
        }

        [HttpPost("Create/Topic")]
        public async Task<IActionResult> CreateTopic([FromBody] TopicCreateModel model)
        {
            var res = await _learningService.CreateTopicAsync(model);
            if (!res.IsSuccessful)
            {
                return BadRequest(res);
            }
            return Ok(res);
        }

        [HttpPost("Add/TaskAnswer")]
        public async Task<IActionResult> AddTaskAnswer([FromBody] TaskAnswerModel model)
        {
            var res = await _learningService.AddTaskAnswerAsync(model);
            if (!res.IsSuccessful)
            {
                return BadRequest(res);
            }
            return Ok(res);
        }

        [HttpGet("Get/TaskAnswers/{homeTaskId}")]
        public IActionResult GetTaskAnswersByHomeTaskId(Guid homeTaskId)
        {
            return Ok(_learningService.GetTaskAnswersByHomeTaskId(homeTaskId));
        }

        [HttpGet("Get/HomeTask/{topicId}")]
        public async Task<IActionResult> GetHomeTaskByTopicId(Guid topicId)
        {
            return Ok(await _learningService.GetHomeTaskByIdAsync(topicId));
        }

        [HttpGet("Get/Topics/{subjectId}")]
        public IActionResult GetTopicsBySubjectId(Guid subjectId)
        {
            return Ok(_learningService.GetTopicsBySubjectId(subjectId));
        }

        [HttpPut("Update/Topic/{id}")]
        public async Task<IActionResult> UpdateTopic(Guid id, [FromBody] TopicModel model)
        {
            var res = await _learningService.UpdateTopicAsync(id, model);
            if (!res.IsSuccessful)
            {
                return BadRequest(res);
            }
            return Ok(res);
        }

        [HttpPut("Update/HomeTask/{id}")]
        public async Task<IActionResult> UpdateHomeTask(Guid id, [FromBody] HomeTaskModel model)
        {
            var res = await _learningService.UpdateHomeTaskAsync(id, model);
            if (!res.IsSuccessful)
            {
                return BadRequest(res);
            }
            return Ok(res);
        }

        [HttpPut("Update/TaskAnswer/{id}")]
        public async Task<IActionResult> UpdateTaskAnswer(Guid id, [FromBody] TaskAnswerUpdateModel model)
        {
            var res = await _learningService.UpdateTaskAnswerAsync(id, model);
            if(!res.IsSuccessful)
            {
                return BadRequest(res);
            }

            return Ok(res);
        }

        [HttpDelete("Remove/HomeTask/{topicId}")]
        public async Task<IActionResult> RemoveHomeTask(Guid topicId)
        {
            var res = await _learningService.RemoveHomeTaskAsync(topicId);
            if (!res.IsSuccessful)
            {
                return BadRequest(res);
            }

            return Ok(res);
        }

    }
}
