using DiagnostykaItemsAdministrationService.Application.Common.Helpers;
using FluentValidation;

namespace DiagnostykaItemsAdministrationService.Application.Operations.Colors.Commands.DeleteColor;

public class DeleteColorCommandValidator : AbstractValidator<DeleteColorCommand>
{
    public DeleteColorCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(ValidationMessages.GetIntValueGreaterThanMessage(0));
    }
}