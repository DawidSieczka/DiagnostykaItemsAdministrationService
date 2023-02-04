using DiagnostykaItemsAdministrationService.Application.Operations.Colors.Commands.CreateColor;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace DiagnostykaItemsAdministrationService.IntegrationTests.Colors.Commands;

internal class CreateColorCommandTests : TestBase
{
    [Test]
    public async Task CreateItem_WithValidInput_ShouldCreateSuccess()
    {
        //arrange
        var command = new CreateColorCommand()
        {
            Name = "What ever"
        };

        //act
        var handler = new CreateColorCommandHandler(_dbContext);
        var result = await handler.Handle(command, CancellationToken.None);

        //assert
        result.Should().NotBe(default);

        var createdColor = await _dbContext.Colors.FirstOrDefaultAsync(x => x.Id == result);
        createdColor.Should().NotBeNull();
        createdColor?.Name.Should().Be(command.Name);
    }
}