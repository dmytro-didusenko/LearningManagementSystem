using FluentValidation;
using LearningManagementSystem.Domain.Models.Certificate;

namespace LearningManagementSystem.Domain.Validators
{
    public class CertificateModelValidation : AbstractValidator<CertificateModel>
    {
        public CertificateModelValidation()
        {
            RuleFor(c => c.StudentName)
                .NotNull();
            
            RuleFor(c => c.CourseName)
                .NotNull();
            
            RuleFor(c => c.Date)
                .NotNull();
        }
    }
}