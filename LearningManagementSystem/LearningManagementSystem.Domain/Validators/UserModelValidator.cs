using FluentValidation;
using LearningManagementSystem.Domain.Models.User;


namespace LearningManagementSystem.Domain.Validators
{
    public class UserModelValidator : AbstractValidator<UserModel>
    {
        public UserModelValidator()
        {
        
            RuleFor(r => r.Email)
                .EmailAddress();

            RuleFor(r => r.Gender)
                .IsInEnum();

            RuleFor(r => r.Birthday)
                .NotNull().LessThan(DateTime.Now)
                .WithMessage("Your age is not valid!");

            RuleFor(r => r.FirstName)
                .NotNull()
                .NotEmpty()
                .Length(2, 15).WithMessage("Invalid name length")
                .Must(ValidatorHelper.OnlyCharacters).WithMessage(ValidatorHelper.OnlyCharactersError);

            RuleFor(r => r.UserName)
                .NotNull()
                .NotEmpty()
                .Length(3, 15);

            RuleFor(r => r.LastName)
                .NotNull()
                .NotEmpty()
                .Length(3, 15)
                .Must(ValidatorHelper.OnlyCharacters).WithMessage(ValidatorHelper.OnlyCharactersError);
        }
    }
}
