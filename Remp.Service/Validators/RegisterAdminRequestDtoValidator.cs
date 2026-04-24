using FluentValidation;
using Remp.Service.DTOs;

namespace Remp.Service.Validators;

public class RegisterAdminRequestDtoValidator : AbstractValidator<RegisterAdminRequestDto>
{
    public RegisterAdminRequestDtoValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one number.");
        RuleFor(x => x.PhotographyCompanyName).NotEmpty().MaximumLength(100);
    }
}
