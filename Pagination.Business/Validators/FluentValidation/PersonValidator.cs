using FluentValidation;
using Pagination.Dto.Concrete;

namespace Pagination.Business.Validators.FluentValidation
{
    public class PersonValidator : AbstractValidator<PersonDto>
    {
        public PersonValidator()
        {
            RuleFor(p => p.FirstName)
                .NotNull()
                .NotEmpty();
            RuleFor(p => p.FirstName)
                .MaximumLength(30);

            RuleFor(p => p.LastName)
                .NotNull()
                .NotEmpty();
            RuleFor(p => p.LastName)
                .MaximumLength(30);

            RuleFor(p => p.Email)
                .NotNull()
                .NotEmpty();
            RuleFor(p => p.Email)
                .MaximumLength(50);
            RuleFor(p => p.Email)
                .EmailAddress();

            RuleFor(p => p.DateOfBirth)
                .NotNull()
                .NotEmpty();

        }
    }
}
