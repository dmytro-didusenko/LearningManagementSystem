using LearningManagementSystem.Domain.Models;
using LearningManagementSystem.Domain.Models.Testing;

namespace LearningManagementSystem.Core.Services.Interfaces
{
    public interface ITestingService
    {
        public Task<Response<TestModel>> CreateTestAsync(TestModel model);
    }
    
}
