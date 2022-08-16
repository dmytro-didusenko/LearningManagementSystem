using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Domain.Models.Auth;

namespace LearningManagementSystem.Core.Utils
{
    public interface IJwtHandler
    {
        public string GenerateToken(User user);

        public AuthUserModel? ValidateToken(string token);
    }
}
