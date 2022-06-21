using FluentValidation;
using LearningManagementSystem.Domain.Models.Subject;

namespace LearningManagementSystem.Domain.Validators
{
    public class SubjectModelValidator : AbstractValidator<SubjectModel>
    {
        public SubjectModelValidator()
        {
            RuleFor(r => r.Name)
                .NotNull()
                .NotEmpty()
                .Must(ValidatorHelper.OnlyCharacters).WithMessage(ValidatorHelper.OnlyCharactersError)
                .Length(3, 25);
        }
    }
}
