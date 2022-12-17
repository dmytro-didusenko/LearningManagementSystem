﻿namespace LearningManagementSystem.Domain.Entities
{
    public class Course : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartedAt { get; set; }
        public string? ImagePath { get; set; }
        public ICollection<Subject>? Subjects { get; set; }
        public bool IsActive { get; set; } = true;

    }
}
