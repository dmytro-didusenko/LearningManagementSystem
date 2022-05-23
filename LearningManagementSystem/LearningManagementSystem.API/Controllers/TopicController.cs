using LearningManagementSystem.Core.Services.Interfaces;
using LearningManagementSystem.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class TopicController : ControllerBase
    {
        private readonly ITopicService _topicService;

        public TopicController(ITopicService topicService)
        {
            _topicService = topicService;
        }

        [HttpPost("AddHomeTask")]
        public async Task<IActionResult> AddHomeTask([FromBody] HomeTaskCreateModel model)
        {
            var res = await _topicService.CreateHomeTask(model);
            if (!res.IsSuccessful)
            {
                return BadRequest(res);
            }
            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTopic([FromBody]TopicCreateModel model)
        {
            var res = await _topicService.CreateTopic(model);
            if (!res.IsSuccessful)
            {
                return BadRequest(res);
            }
            return Ok(res);
        }

        [HttpGet]
        public IActionResult GetAllTopics()
        {
            return Ok(_topicService.GetAllTopics());
        }

    }
}
