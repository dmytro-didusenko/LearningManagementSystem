using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LearningManagementSystem.Domain.Models;

namespace LearningManagementSystem.Core.Services.Interfaces
{
    public interface IBaseService<TModel> where TModel : class
    {
        public Task<Response<TModel>> AddAsync(TModel model);
        public Task UpdateAsync(Guid id, TModel model);
        public Task RemoveAsync(TModel model);
        public Task<Response<TModel>> GetByIdAsync(Guid id);
    }
}
