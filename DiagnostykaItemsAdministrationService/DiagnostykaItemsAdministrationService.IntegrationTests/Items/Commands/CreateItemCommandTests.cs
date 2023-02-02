using DiagnostykaItemsAdministrationService.Application.Common.Helpers;
using DiagnostykaItemsAdministrationService.Application.Common.Helpers.Interfaces;
using DiagnostykaItemsAdministrationService.Application.Operations.Items.Commands.CreateItem;
using DiagnostykaItemsAdministrationService.Domain.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        createdItem?.Name.Should().Be(command.Name);
        createdItem?.ColorId.Should().Be(command?.ColorId);
        createdItem?.Code.Should().Be(mockedHashName);
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
        //assert
    }

}
