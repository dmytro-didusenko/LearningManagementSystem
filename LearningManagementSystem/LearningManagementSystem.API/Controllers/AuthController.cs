using LearningManagementSystem.API.Extensions;
using LearningManagementSystem.Core.AuthServices;
using LearningManagementSystem.Domain.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthManager _authManager;

        public AuthController(IAuthManager authManager)
        {
            _authManager = authManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            await _authManager.RegisterAsync(registerModel);
            return Ok();
        }

        [HttpPost("signin")]
        public async Task<IActionResult> Register(SignInModel signInModel)
        {
            var res = await _authManager.SignInAsync(signInModel);
            return res.ToActionResult();
        }

        private void SettTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        private string? GetIpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];

            return HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
        }
    }
}
