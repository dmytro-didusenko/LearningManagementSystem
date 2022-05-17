using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Http;

namespace LearningManagementSystem.Domain.Models
{
    public class CourseModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty; 
        public DateTime StartedAt { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string? ImagePath { get; set; }
    }
}
