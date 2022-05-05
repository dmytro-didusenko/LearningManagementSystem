using LearningManagementSystem.Core.Services.Implementation;
using LearningManagementSystem.Domain.Models;

namespace LearningManagementSystem.Core.Services.Interfaces
{
    public interface IUserService : IBaseService<UserModel>
    {
        public Task<List<UserModel>> GetByFilterAsync(UserQueryModel? query = null);
    }
}
