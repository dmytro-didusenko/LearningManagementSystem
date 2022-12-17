﻿using LearningManagementSystem.Domain.Models.Subject;
using Microsoft.AspNetCore.Http;

namespace LearningManagementSystem.Domain.Models.Course
{
    public class CourseModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartedAt { get; set; }
        public bool IsActive { get; set; }

    }
}
