namespace LearningManagementSystem.API.Middlewares
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;


        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();


            await _next(context);
        }
    }
}
