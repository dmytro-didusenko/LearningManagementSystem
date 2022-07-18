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

            RuleFor(r => r.StartEducation)
                .NotNull()
                .NotEmpty()
                .GreaterThanOrEqualTo(DateTime.Today);

            RuleFor(r => r.EndEducation)
                .NotNull()
                .NotEmpty()
                .GreaterThan(m=> m.StartEducation)
                .WithMessage("End education date must be greater than start education date");
        }
    }
}
