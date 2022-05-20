using LearningManagementSystem.Domain.Models;

namespace LearningManagementSystem.Core.Services.Interfaces
{
    public interface IUserService : IBaseService<UserModel>
    {
        public Task<IEnumerable<UserModel>> GetByFilterAsync(UserQueryModel? query = null);
    }
}
