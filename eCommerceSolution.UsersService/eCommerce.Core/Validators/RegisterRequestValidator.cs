using eCommerce.Core.DTO;
using FluentValidation;

namespace eCommerce.Core.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        //Email
        RuleFor(temp => temp.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress();

        //Password
        RuleFor(temp => temp.Password)
            .NotEmpty().WithMessage("Password is required")
            .Length(1, 50).WithMessage("PersonName length must be between 1 and 50 characters");

        //Person Name
        RuleFor(temp => temp.PersonName)
            .NotEmpty().WithMessage("Person name is required")
            .MinimumLength(1)
            .MaximumLength(50);

        //Gender
        RuleFor(temp => temp.Gender)
            .IsInEnum().WithMessage("Gender value must be in the enum list");
    }
}

