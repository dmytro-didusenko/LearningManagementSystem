using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LearningManagementSystem.Domain.Models;

namespace LearningManagementSystem.Core.Services.Implementation
{
    public interface IBaseService<TModel> where TModel : class
    {
        public Task AddAsync(TModel model);
        public void Update(TModel model);
        public void Remove(TModel model);
        public IEnumerable<TModel> GetAll();

        //TODO: Implement filtering
        //public IEnumerable<TModel> GetModels();
        //public Task<TModel> GetModelAsync();

    }
}
