using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace LearningManagementSystem.Core.Helpers
{
    public class FileHelper: IFileHelper
    {
        private readonly IConfiguration _configuration;

        public FileHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> UploadFileAsync(IFormFile file)
        {
            string folderName = _configuration["FileStorage"];
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            if (!Directory.Exists(folderPath))
            {
                return "";
            }
            var path = Path.Combine(folderPath, file.FileName);
            await using var fileContentStream = new MemoryStream();

            await file.CopyToAsync(fileContentStream);
            await File.WriteAllBytesAsync(path, fileContentStream.ToArray());
            return path;
        }
    }
}
