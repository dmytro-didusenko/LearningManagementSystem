using LearningManagementSystem.Domain.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LearningManagementSystem.API.Filters
{
    public class ValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values.Where(v => v.Errors.Count > 0)
                    .SelectMany(v => v.Errors)
                    .Select(v => v.ErrorMessage)
                    .ToList();


                var responseApi = new ResponseApi()
                {
                    Errors = errors
                };
               
                context.Result = new JsonResult(responseApi)
                {
                    StatusCode = 400
                };
            
            }
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
