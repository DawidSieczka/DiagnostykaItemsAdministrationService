using DiagnostykaItemsAdministrationService.Application.Common.Helpers;
using FluentValidation;

namespace DiagnostykaItemsAdministrationService.Application.Operations.Items.Commands.DeleteItem;

internal class DeleteItemCommandValidator : AbstractValidator<DeleteItemCommand>
{
    public DeleteItemCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(ValidationMessages.GetIntValueGreaterThanMessage(0));
    }
}