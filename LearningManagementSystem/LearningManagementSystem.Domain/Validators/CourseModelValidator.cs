using FluentValidation;
using LearningManagementSystem.Domain.Models.Course;

namespace LearningManagementSystem.Domain.Validators
{
    public class CourseModelValidator : AbstractValidator<CourseModel>
    {
        public CourseModelValidator()
        {
            RuleFor(r => r.Name)
                .NotNull()
                .NotEmpty()
                .Length(3, 50);

            RuleFor(r => r.StartedAt)
                .LessThan(DateTime.Now);

            RuleForEach(f => f.Subjects)
                .InjectValidator();

            When(c => c.ImageFile is not null, () =>
            {
                RuleFor(r => r.ImageFile.FileName)
                    .Must(f => f.EndsWith(".png") || f.EndsWith(".jpg"));
            });
        }
    }
}
