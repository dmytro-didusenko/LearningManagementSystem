using LearningManagementSystem.Domain.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Extensions
{
    public static class ResponseExtensions
    {
        public static IActionResult ToActionResult<T>(this Response<T> response)
        {
            ArgumentNullException.ThrowIfNull(response);
            var responseApi = new ResponseApi<T>();
            if (response.Error is not null)
            {
                responseApi.Errors = new List<string>{response.Error.ErrorMessage};
                return new JsonResult(responseApi)
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
