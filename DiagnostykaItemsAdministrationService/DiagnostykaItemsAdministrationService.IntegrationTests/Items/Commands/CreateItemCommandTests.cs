using DiagnostykaItemsAdministrationService.Application.Common.Exceptions;
using DiagnostykaItemsAdministrationService.Application.Common.Helpers.Interfaces;
using DiagnostykaItemsAdministrationService.Application.Operations.Items.Commands.CreateItem;
using DiagnostykaItemsAdministrationService.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace DiagnostykaItemsAdministrationService.IntegrationTests.Items.Commands;

internal class CreateItemCommandTests : TestBase
{
    [Test]
    public async Task CreateItem_WithValidInput_ShouldCreateSuccess()
    {
        //arrange
        var color = new Color()
        {
            Name = "White"
        };

        await _dbContext.Colors.AddAsync(color);
        await _dbContext.SaveChangesAsync();

        var command = new CreateItemCommand()
        {
            Name = "What ever",
            ColorId = color.Id,
        };

        var hashGeneratorMock = new Mock<IHashGenerator>();
        var mockedHashName = "12characters";
        hashGeneratorMock.Setup(x => x.GenerateHash(It.IsAny<int>())).Returns(mockedHashName);
        hashGeneratorMock.Setup(x => x.GenerateHash(It.IsAny<string>())).Returns(mockedHashName);

        //act
        var handler = new CreateItemCommandHandler(_dbContext, hashGeneratorMock.Object);
        var result = await handler.Handle(command, CancellationToken.None);

        //assert
        result.Should().NotBe(default);

        var createdItem = await _dbContext.Items.FirstOrDefaultAsync(x => x.Id == result);
        createdItem.Should().NotBeNull();
        createdItem?.Name.Should().Be(command.Name);
        createdItem?.ColorId.Should().Be(command?.ColorId);
        createdItem?.Code.Should().HaveLength(12).And.Be(mockedHashName);
    }

    [Test]
    public async Task CreateItem_WithNotExistingColor_ShouldThrowNotFoundException()
    {
        //arrange
        var command = new CreateItemCommand()
        {
            Name = "What ever",
            ColorId = 0
        };

        //act
        var result = await FluentActions.Invoking(() => _sender.Send(command)).Should().ThrowAsync<NotFoundException>();

        //assert

        var existingItemsCount = await _dbContext.Items.CountAsync();
        existingItemsCount.Should().Be(0);
    }
}