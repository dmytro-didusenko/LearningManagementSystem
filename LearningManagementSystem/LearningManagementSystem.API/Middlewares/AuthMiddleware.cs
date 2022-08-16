using LearningManagementSystem.Core.AuthServices;
using LearningManagementSystem.Core.Utils;

namespace LearningManagementSystem.API.Middlewares
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IUserManager userManager, IJwtHandler jwtHandler)
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
            var user = jwtHandler.ValidateToken(token);
            if (user != null)
            {
                context.Items["User"] = user;
            }

            await _next(context);
        }
    }
}
