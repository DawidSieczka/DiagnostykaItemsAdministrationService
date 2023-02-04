using DiagnostykaItemsAdministrationService.Application.Common.Exceptions;
using DiagnostykaItemsAdministrationService.Application.Operations.Colors.Commands.DeleteColor;
using DiagnostykaItemsAdministrationService.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace DiagnostykaItemsAdministrationService.IntegrationTests.Colors.Commands;

public class DeleteColorCommandTests : TestBase
{
    [Test]
    public async Task DeleteColor_WithValidInput_ShouldDeleteSuccesfuly()
    {
        //arrange
        var color = new Color()
        {
            Name = "White"
        };

        await _dbContext.Colors.AddAsync(color);
        await _dbContext.SaveChangesAsync();

        var command = new DeleteColorCommand()
        {
            Id = color.Id
        };

        //act
        var handler = new DeleteColorCommandHandler(_dbContext);
        await handler.Handle(command, CancellationToken.None);

        //assert
        var deletedColor = await _dbContext.Items.FirstOrDefaultAsync(x => x.Id == color.Id);
        deletedColor.Should().BeNull();
    }

    [Test]
    public async Task DeleteItem_WithNotExistingItem_ShouldThrowNotFoundException()
    {
        //arrange
        var command = new DeleteColorCommand()
        {
            Id = It.IsAny<int>()
        };

        var beforeOperationColorsCount = await _dbContext.Items.CountAsync();

        //act
        var result = await FluentActions.Invoking(() => _sender.Send(command)).Should().ThrowAsync<NotFoundException>();

        //assert
        var existingColorsCount = await _dbContext.Colors.CountAsync();
        existingColorsCount.Should().Be(beforeOperationColorsCount);
    }
}