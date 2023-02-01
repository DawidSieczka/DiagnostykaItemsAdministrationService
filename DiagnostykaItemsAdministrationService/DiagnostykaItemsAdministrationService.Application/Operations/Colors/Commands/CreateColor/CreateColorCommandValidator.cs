using DiagnostykaItemsAdministrationService.Application.Common.Helpers;
using DiagnostykaItemsAdministrationService.Domain.Rules;
using FluentValidation;

namespace DiagnostykaItemsAdministrationService.Application.Operations.Colors.Commands.CreateColor;

public class CreateColorCommandValidator : AbstractValidator<CreateColorCommand>
{
    public CreateColorCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty()
            .MaximumLength(ItemRules.NameMaxLength)
            .WithMessage(ValidationMessages.GetStringLengthMessage(0, ColorRules.NameMaxLength));
    }
}