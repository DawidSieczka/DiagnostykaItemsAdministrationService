using DiagnostykaItemsAdministrationService.Application.Common.Helpers;
using DiagnostykaItemsAdministrationService.Domain.Rules;
using FluentValidation;

namespace DiagnostykaItemsAdministrationService.Application.Operations.Items.Commands.CreateItem;

public class CreateItemCommandValidator : AbstractValidator<CreateItemCommand>
{
    public CreateItemCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty()
            .MaximumLength(ItemRules.NameMaxLength)
            .WithMessage(ValidationMessages.GetStringLengthMessage(0, ItemRules.NameMaxLength));

        RuleFor(c => c.ColorId).GreaterThan(0)
            .WithMessage(ValidationMessages.GetIntValueGreaterThanMessage(0));
    }
}