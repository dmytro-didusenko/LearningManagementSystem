using System.Net;
using LearningManagementSystem.Domain.Models.Auth;
using LearningManagementSystem.Domain.Models.Responses;

namespace LearningManagementSystem.Core.AuthServices
{
    public interface IAuthManager
    {
        public Task<Response<bool>> RegisterAsync(RegisterModel model);
        public Task<Response<AuthResponse>> SignInAsync(SignInModel model);
        public Task<AuthResponse> RefreshTokenAsync(string token, string ipAddress);
        void RevokeToken(string token, string ipAddress);
        public Task<AuthUserModel> GetUserById(Guid id);
    }
}