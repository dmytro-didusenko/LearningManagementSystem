using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Domain.Models.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LearningManagementSystem.API.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizedAttribute : Attribute, IAuthorizationFilter
    {
        private readonly IList<string> _roles;

        public AuthorizedAttribute(params string[] roles)
        {
            _roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
                return;

            // authorization
            var user = context.HttpContext.Items["User"] as AuthUserModel;
            if (user == null)
            { 
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }

            if ((_roles.Any() && !_roles.Contains(user!.Role)))
            {
                context.Result = new JsonResult(new { message = "Forbidden access" }) { StatusCode = StatusCodes.Status403Forbidden };
            }
            
        }
    }
}
