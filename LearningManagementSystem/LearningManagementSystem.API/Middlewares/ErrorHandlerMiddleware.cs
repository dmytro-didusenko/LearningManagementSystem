using System.Net;
using System.Text.Json;
using LearningManagementSystem.Core.Exceptions;
using LearningManagementSystem.Domain.Models.Responses;

namespace LearningManagementSystem.API.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case NotFoundException ex:
                        response.StatusCode = (int) HttpStatusCode.NotFound;
                        break;
                    case BadRequestException ex:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var responseApi = ResponseApi.GetError(error.Message);
                var result = JsonSerializer.Serialize(responseApi);
                await response.WriteAsync(result);
            }
        }

    }
}
