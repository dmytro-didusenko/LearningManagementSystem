using LearningManagementSystem.Domain.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Extensions
{
    public static class ResponseExtensions
    {
        public static IActionResult ToActionResult<T>(this Response<T> response)
        {
            ArgumentNullException.ThrowIfNull(response);

            if (response.Error is not null)
            {
                return new JsonResult(response.Error)
                {
                    StatusCode = (int)response.Error.ErrorCode
                };
            }

            if (Equals(response.Data, default(T)))
            {
                return new OkResult();
            }
            return new OkObjectResult(response.Data);
        }
    }
}
