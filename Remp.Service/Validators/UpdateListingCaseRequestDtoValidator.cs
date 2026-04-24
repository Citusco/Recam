using FluentValidation;
using Remp.Service.DTOs;

namespace Remp.Service.Validators;

public class UpdateListingCaseRequestDtoValidator : AbstractValidator<UpdateListingCaseRequestDto>
{
    public UpdateListingCaseRequestDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.Street).NotEmpty().MaximumLength(200);
        RuleFor(x => x.City).NotEmpty().MaximumLength(100);
        RuleFor(x => x.State).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Postcode).GreaterThan(0);
        RuleFor(x => x.Price).GreaterThan(0);
        RuleFor(x => x.Bedrooms).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Bathrooms).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Garages).GreaterThanOrEqualTo(0);
        RuleFor(x => x.FloorArea).GreaterThan(0);
    }
}
