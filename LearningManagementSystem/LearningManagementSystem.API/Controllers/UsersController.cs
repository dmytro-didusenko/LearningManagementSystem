using LearningManagementSystem.API.Extensions;
using LearningManagementSystem.Core.Services.Interfaces;
using LearningManagementSystem.Domain.Models.User;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IDocumentService _documentService;

        public UsersController(IUserService userService, IDocumentService documentService)
        {
            _userService = userService;
            _documentService = documentService;
        }

        [HttpPost]
        public async Task<IActionResult> AddUserAsync([FromBody] UserModel user)
        {
            ArgumentNullException.ThrowIfNull(user);

            var res = await _userService.AddAsync(user);
            return res.ToActionResult();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserModel model)
        {
            var res = await _userService.UpdateAsync(id, model);
            return res.ToActionResult();
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var res = await _userService.GetByIdAsync(id);
            return Ok(res);
        }

        [HttpPost("Documents")]
        public async Task<IActionResult> AddDocument([FromBody] DocumentModel model)
        {
            var res = await _documentService.AddDocumentAsync(model);
            return res.ToActionResult();
        }

        [HttpGet("Documents")]
        public async Task<IActionResult> GetDocumentByFilter([FromQuery] DocumentQueryModel? query = null)
        {
            var res = await _documentService.GetDocumentsByFilterAsync(query);
            return Ok(res);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersAsync([FromQuery] UserQueryModel? query = null)
        {
            return Ok(await _userService.GetByFilterAsync(query));
        }

        [HttpDelete("Documents/{id}")]
        public async Task<IActionResult> RemoveDocument(Guid id)
        {
            await _documentService.RemoveDocumentByIdAsync(id);
            return NoContent();
        }

        [HttpGet("Documents/{id}")]
        public async Task<IActionResult> GetDocumentById(Guid id)
        {
            return Ok(await _documentService.GetDocumentByIdAsync(id));
        }
    }
}