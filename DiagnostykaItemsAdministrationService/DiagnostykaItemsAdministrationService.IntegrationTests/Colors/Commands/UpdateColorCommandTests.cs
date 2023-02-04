using DiagnostykaItemsAdministrationService.Application.Operations.Colors.Commands.UpdateColor;
using DiagnostykaItemsAdministrationService.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace DiagnostykaItemsAdministrationService.IntegrationTests.Colors.Commands;

internal class UpdateColorCommandTests : TestBase
{
    [Test]
    public async Task UpdateItem_WithValidInput_ShouldUpdateSuccesfuly()
    {
        //arrange
        var color = new Color()
        {
            Name = "White"
        };

        await _dbContext.Colors.AddAsync(color);
        await _dbContext.SaveChangesAsync();

        var command = new UpdateColorCommand()
        {
            Id = color.Id,
            Name = "Changed name",
        };

        //act
        var handler = new UpdateColorCommandHandler(_dbContext);
        await handler.Handle(command, CancellationToken.None);

        //assert
        var updatedColor = await _dbContext.Colors.FirstOrDefaultAsync(x => x.Id == color.Id);
        updatedColor.Should().NotBeNull();
        updatedColor?.Name.Should().Be(command.Name);
    }
}