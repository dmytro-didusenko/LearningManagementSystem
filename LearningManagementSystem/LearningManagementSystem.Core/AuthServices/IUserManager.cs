using LearningManagementSystem.Domain.Models.Auth;
using LearningManagementSystem.Domain.Models.Responses;

namespace LearningManagementSystem.Core.AuthServices
{
    public interface IUserManager
    {
        public Task<Response<bool>> RegisterAsync(RegisterModel model);

        public Task<Response<AuthResponse>> SignInAsync(SignInModel model);
        public Task<AuthUserModel> GetUserById(Guid id);
    }
}
