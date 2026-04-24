using FluentValidation;
using Remp.Service.DTOs;

namespace Remp.Service.Validators;

public class SelectMediaRequestDtoValidator : AbstractValidator<SelectMediaRequestDto>
{
    public SelectMediaRequestDtoValidator()
    {
        RuleFor(x => x.MediaIds)
            .NotEmpty().WithMessage("At least one media ID must be provided.")
            .Must(ids => ids.Count() <= 10).WithMessage("Cannot select more than 10 media assets.");
    }
}
