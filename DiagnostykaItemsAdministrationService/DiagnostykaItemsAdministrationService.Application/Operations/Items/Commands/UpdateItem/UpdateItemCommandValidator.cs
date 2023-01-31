﻿using DiagnostykaItemsAdministrationService.Application.Common.Helpers;
using DiagnostykaItemsAdministrationService.Domain.Rules;
using FluentValidation;

namespace DiagnostykaItemsAdministrationService.Application.Operations.Items.Commands.UpdateItem;

internal class UpdateItemCommandValidator : AbstractValidator<UpdateItemCommand>
{
    public UpdateItemCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(ValidationMessages.GetIntValueGreaterThanMessage(0));

        RuleFor(c => c.Name).NotEmpty()
            .MaximumLength(ItemRules.NameMaxLength)
            .WithMessage(ValidationMessages.GetStringLengthMessage(0, ItemRules.NameMaxLength));
    }
}