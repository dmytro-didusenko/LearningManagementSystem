﻿namespace LearningManagementSystem.Domain.Entities
{
    public class Student
    {
        public Guid Id { get; set; }
        public User User { get; set; } = null!;
        public Guid? GroupId { get; set; }
        public Group? Group { get; set; }
        public string ContractNumber { get; set; } = string.Empty;
        public ICollection<TaskAnswer> TaskAnswers { get; set; }
        public ICollection<Certificate>? Certificates { get; set; }
    }
}