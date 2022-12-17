using LearningManagementSystem.API.Extensions;
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

            return res.ToActionResult();
        }

        [HttpPost("Questions/{testId}")]
        public async Task<IActionResult> AddQuestion(Guid testId, [FromBody] QuestionCreateModel questionModel)
        {
            var res = await _testingService.AddQuestionAsync(testId, questionModel);

            return res.ToActionResult();
        }
        [HttpPost("Answers/{questionId}")]
        public async Task<IActionResult> AddAnswersToQuestion(Guid questionId, [FromBody] IEnumerable<AnswerCreateModel> answers)
        {
            var res = await _testingService.AddAnswersToQuestionAsync(questionId, answers);
            return res.ToActionResult();
        }

        [HttpGet("Tests/{testId}")]
        public async Task<IActionResult> GetTestById(Guid testId)
        {
            var res = await _testingService.GetTestByIdAsync(testId);
            
            return Ok(res);
        }

        [HttpGet("Questions/{testId}")]
        public async Task<IActionResult> GetQuestionsByTestId(Guid testId)
        {
            return Ok(await _testingService.GetQuestionsByTestId(testId));
        }


        [HttpGet("Questions/Passing/{testId}")]
        public async Task<IActionResult> GetQuestionsForPassing(Guid testId)
        {
            return Ok(await _testingService.GetQuestionsForPassing(testId));
        }

        [HttpPost("Tests/Passing")]
        public async Task<IActionResult> PassTest([FromBody] IEnumerable<StudentAnswerModel> model)
        {
            var res =  await _testingService.PassTest(model);
            return res.ToActionResult();
        }

        [HttpGet("Results/{testId}/{studentId}")]
        public async Task<IActionResult> GetStudentResults(Guid testId, Guid studentId)
        {
            var res = await _testingService.GetTestingResultAsync(testId, studentId);
            return Ok(res);
        }
    }
    
}
