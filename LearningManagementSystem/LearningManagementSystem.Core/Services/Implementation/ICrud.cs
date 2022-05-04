using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Core.Services.Implementation
{
    internal interface ICrud<TEntity> where TEntity : class
    {
        public Task AddAsync();
        public Task<IEnumerable<TEntity>> GetAsync(Predicate<TEntity> predicate);
        public Task<TEntity> GetAsync(Guid id);
        public void Update(TEntity entity);
        public void Remove(TEntity entity);
    }
}
