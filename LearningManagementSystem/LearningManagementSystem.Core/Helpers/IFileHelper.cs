using Microsoft.AspNetCore.Http;

namespace LearningManagementSystem.Core.Helpers
{
    public interface IFileHelper
    {
        public Task<string> UploadFileAsync(IFormFile file);
    }
}
