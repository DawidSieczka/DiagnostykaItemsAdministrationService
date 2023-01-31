using DiagnostykaItemsAdministrationService.Application.Common.Helpers;
using FluentValidation;

namespace DiagnostykaItemsAdministrationService.Application.Operations.Items.Queries.GetItemById;

internal class GetItemByIdQueryValidator : AbstractValidator<GetItemByIdQuery>
{
    public GetItemByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(ValidationMessages.GetIntValueGreaterThanMessage(0));
    }
}