﻿using LearningManagementSystem.Domain.Models.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LearningManagementSystem.API.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizedAttribute : Attribute, IAuthorizationFilter
    {
        private IList<string> _roles;

        public AuthorizedAttribute(params string[] roles)
        {
            _roles = new List<string>(roles);
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var allowAnonymous = context.ActionDescriptor
                .EndpointMetadata
                .OfType<AllowAnonymousAttribute>()
                .Any();

            if (allowAnonymous)
                return;

            var innerAttribute = context.ActionDescriptor
                .EndpointMetadata
                .OfType<AuthorizedAttribute>()
                .LastOrDefault();

            if (innerAttribute is not null)
            {
                _roles = innerAttribute._roles;
            }

            var user = context.HttpContext.Items["User"] as AuthUserModel;
            if (user is null)
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
                return;
            }

            if ((_roles.Any() && !_roles.Contains(user!.Role)))
            {
                context.Result = new JsonResult(new { message = "Forbidden access" }) { StatusCode = StatusCodes.Status403Forbidden };
            }
        }
    }
}
