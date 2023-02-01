using DiagnostykaItemsAdministrationService.Application.Common.Helpers;
using FluentValidation;

namespace DiagnostykaItemsAdministrationService.Application.Operations.Colors.Queries.GetColorById;

public class GetColorByIdQueryValidator : AbstractValidator<GetColorByIdQuery>
{
    public GetColorByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(ValidationMessages.GetIntValueGreaterThanMessage(0));
    }
}