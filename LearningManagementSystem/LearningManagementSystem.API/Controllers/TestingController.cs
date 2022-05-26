using LearningManagementSystem.Core.Services.Interfaces;
using LearningManagementSystem.Domain.Models.Testing;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class TestingController : ControllerBase
    {
        private readonly ITestingService _testingService;

        public TestingController(ITestingService testingService)
        {
            _testingService = testingService;
        }

        [HttpPost("Tests")]
        public async Task<IActionResult> CreateTest([FromBody] TestModel model)
        {
            var res = await _testingService.CreateTestAsync(model);
            if (!res.IsSuccessful)
            {
                return BadRequest(res);
            }

            return Ok(res);
        }

        [HttpPost("Questions/{testId}")]
        public async Task<IActionResult> AddQuestion(Guid testId, [FromBody] QuestionModel questionModel)
        {
            var res = await _testingService.AddQuestionAsync(testId, questionModel);
            if (!res.IsSuccessful)
            {
                return BadRequest(res);
            }

            return Ok(res);
        }
        [HttpPost("Answers/{questionId}")]
        public async Task<IActionResult> AddAnswersToQuestion(Guid questionId, [FromBody] IEnumerable<AnswerCreateModel> answers)
        {
            var res = await _testingService.AddAnswersToQuestion(questionId, answers);
            if (!res.IsSuccessful)
            {
                return BadRequest(res);
            }

            return Ok(res);
        }
    }
}
