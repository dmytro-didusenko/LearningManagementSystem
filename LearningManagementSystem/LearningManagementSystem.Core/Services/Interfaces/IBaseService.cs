using LearningManagementSystem.Domain.Models.Responses;

namespace LearningManagementSystem.Core.Services.Interfaces
{
    public interface IBaseService<TModel> where TModel : class
    {
        public Task<Response<TModel>> AddAsync(TModel model);
        public Task<Response<TModel>> UpdateAsync(Guid id, TModel model);
        public Task RemoveAsync(TModel model);
        public Task<TModel> GetByIdAsync(Guid id);
    }
}
