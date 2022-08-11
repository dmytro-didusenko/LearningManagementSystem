using LearningManagementSystem.API.Utils;
using LearningManagementSystem.Core.AuthServices;

namespace LearningManagementSystem.API.Middlewares
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        
        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IUserManager userManager, JwtHandler jwtHandler)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            
            if (string.IsNullOrEmpty(token))
            {
                var accessToken = context.Request.Query["access_token"];
                //If request is to signalR hubs
                var path = context.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) &&
                    (path.StartsWithSegments("/hubs")))
                {
                    token = accessToken;
                }
            }
            var userId = jwtHandler.ValidateToken(token);
            if (userId != null)
            {
                context.Items["User"] = await userManager.GetUserById(userId.Value);
            }

            await _next(context);
        }
    }
}
