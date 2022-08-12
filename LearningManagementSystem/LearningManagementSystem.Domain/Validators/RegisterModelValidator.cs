using FluentValidation;
using LearningManagementSystem.Domain.Models.Auth;

namespace LearningManagementSystem.Domain.Validators
{
    public class RegisterModelValidator : AbstractValidator<RegisterModel>
    {
        public RegisterModelValidator()
        {
            RuleFor(r => r.Birthday)
                .NotNull().LessThan(DateTime.Now)
                .WithMessage("'Your age' is not valid!");

            RuleFor(r => r.Email)
                .NotNull()
                .EmailAddress();

            //TODO: Validation for password
            RuleFor(r => r.Password)
                .Must(ValidatorHelper.PasswordValidator)
                .WithMessage("Wrong password");

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
