using DiagnostykaItemsAdministrationService.Application.Common.Exceptions;
using DiagnostykaItemsAdministrationService.Application.Operations.Items.Commands.DeleteItem;
using DiagnostykaItemsAdministrationService.Application.Operations.Items.Commands.UpdateItem;
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
internal class UpdateItemCommandTests : TestBase
{
    [Test]
    public async Task UpdateItem_WithValidInput_ShouldUpdateSuccesfuly()
    {
        //arrange
        var color = new Color()
        {
            Name = "White"
        };

        var changedColor = new Color()
        {
            Name = "Black"
        };

        await _dbContext.Colors.AddAsync(color);
        await _dbContext.Colors.AddAsync(changedColor);
        await _dbContext.SaveChangesAsync();

        var item = new Item()
        {
            Name = "anything",
            Code = "12characters",
            ColorId = color.Id,
        };
        await _dbContext.Items.AddAsync(item);
        await _dbContext.SaveChangesAsync();

        var command = new UpdateItemCommand()
        {
            Id = item.Id,
            Name = "Changed name",
            ColorId = changedColor.Id
        };

        //act
        var handler = new UpdateItemCommandHandler(_dbContext);
        await handler.Handle(command, CancellationToken.None);

        //assert
        var updatedItem = await _dbContext.Items.FirstOrDefaultAsync(x => x.Id == item.Id);
        updatedItem.Should().NotBeNull();
        updatedItem?.Name.Should().Be(command.Name);
        updatedItem?.ColorId.Should().Be(changedColor.Id);
    }

    [Test]
    public async Task UpdateItem_WithNotExistingColor_ShouldThrowNotFoundException()
    {
        //arrange
        var color = new Color()
        {
            Name = "White"
        };

        await _dbContext.Colors.AddAsync(color);
        await _dbContext.SaveChangesAsync();

        var item = new Item()
        {
            Name = "anything",
            Code = "12characters",
            ColorId = color.Id,
        };

        await _dbContext.Items.AddAsync(item);
        await _dbContext.SaveChangesAsync();

        var command = new UpdateItemCommand()
        {
            Id = item.Id,
            ColorId = It.Is<int>(val => val != color.Id),
            Name = "Changed Name"
        };

        //act
        var result = await FluentActions.Invoking(() => _sender.Send(command)).Should().ThrowAsync<NotFoundException>();

        //assert
        var notUpdatedItem = await _dbContext.Items.FirstOrDefaultAsync(x=>x.Id == item.Id);
        notUpdatedItem.Should().NotBeNull();
        notUpdatedItem?.ColorId.Should().Be(item.ColorId);
        notUpdatedItem?.Name.Should().Be(item.Name);
    }
}
