using FluentValidation;
using LearningManagementSystem.Domain.Models.Topic;

namespace LearningManagementSystem.Domain.Validators
{
    public class TopicCreateModelValidator : AbstractValidator<TopicCreateModel>
    {
        public TopicCreateModelValidator()
        {
            RuleFor(r => r.Name)
                .NotNull()
                .NotEmpty()
                .Must(ValidatorHelper.OnlyCharacters).WithMessage(ValidatorHelper.OnlyCharactersError)
                .Length(3, 25);

            RuleFor(r => r.SubjectId)
                .NotNull()
                .NotEmpty();
        }
    }
}
