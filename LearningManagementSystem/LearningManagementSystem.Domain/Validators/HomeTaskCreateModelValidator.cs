using FluentValidation;
using LearningManagementSystem.Domain.Models.HomeTask;

namespace LearningManagementSystem.Domain.Validators
{
    public class HomeTaskCreateModelValidator : AbstractValidator<HomeTaskCreateModel>
    {
        public HomeTaskCreateModelValidator()
        {
            RuleFor(r => r.Name)
                .NotEmpty()
                .NotNull()
                .Length(3, 25);

            RuleFor(r => r.Description)
                .NotEmpty()
                .NotNull()
                .MinimumLength(10);

            RuleFor(r => r.DatePlannedStart)
                .NotNull()
                .NotEmpty()
                .GreaterThanOrEqualTo(DateTime.Now);

            RuleFor(r => r.DateOfExpiration)
                .NotNull()
                .NotEmpty()
                .GreaterThanOrEqualTo(DateTime.Now)
                .NotEqual(h => h.DatePlannedStart);
        }
    }
}
