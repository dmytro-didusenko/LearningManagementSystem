using FluentValidation;
using LearningManagementSystem.Domain.Models.HomeTask;

namespace LearningManagementSystem.Domain.Validators
{
    public class GradeModelValidator : AbstractValidator<GradeModel>
    {
        public GradeModelValidator()
        {
            RuleFor(r => r.Value)
                .InclusiveBetween(0, 100);
        }
    }
}
