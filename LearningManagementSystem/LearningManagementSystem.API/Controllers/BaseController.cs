using LearningManagementSystem.Domain.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controllers
{
    public abstract class BaseController: ControllerBase
    {
        AuthUserModel? User => this?.HttpContext.Items["User"] as AuthUserModel;
    }
}
