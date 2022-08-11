using LearningManagementSystem.API.Extensions;
using LearningManagementSystem.Core.AuthServices;
using LearningManagementSystem.Domain.Models.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserManager _userManager;

        public AuthController(IUserManager userManager)
        {
            _userManager = userManager;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            await _userManager.RegisterAsync(registerModel);
            return Ok();
        }

        [HttpPost("signin")]
        public async Task<IActionResult> Register(SignInModel signInModel)
        {
            var res =await _userManager.SignInAsync(signInModel);
            return res.ToActionResult();
        }
    }
}
