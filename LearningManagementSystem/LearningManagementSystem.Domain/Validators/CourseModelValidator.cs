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
                .GreaterThanOrEqualTo(DateTime.Now);

        }
    }
}
