using FluentValidation;
using LearningManagementSystem.Domain.Models.Group;

namespace LearningManagementSystem.Domain.Validators
{
    public class GroupCreateModelValidator: AbstractValidator<GroupCreateModel>
    {
        public GroupCreateModelValidator()
        {
            RuleFor(r => r.Name)
                .NotNull()
                .NotEmpty()
                .Length(3, 25);

            RuleFor(r=>r.StartEducation)
                .NotNull()
                .NotEmpty()
                .GreaterThanOrEqualTo(DateTime.Now);

            RuleFor(r=>r.EndEducation)
                .NotNull()
                .NotEmpty()
                .GreaterThanOrEqualTo(DateTime.Now)
                .NotEqual(h => h.StartEducation);
        }
    }
}
