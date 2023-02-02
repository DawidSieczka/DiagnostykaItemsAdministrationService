using DiagnostykaItemsAdministrationService.Application.Common.Exceptions;
using DiagnostykaItemsAdministrationService.Application.Common.Helpers.Interfaces;
using DiagnostykaItemsAdministrationService.Application.Operations.Items.Commands.CreateItem;
using DiagnostykaItemsAdministrationService.Application.Operations.Items.Commands.DeleteItem;
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
public class DeleteItemCommandTests : TestBase
{
    [Test]
    public async Task DeleteItem_WithValidInput_ShouldDeleteSuccesfuly()
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

        var command = new DeleteItemCommand()
        {
            Id = item.Id
        };
        
        //act
        var handler = new DeleteItemCommandHandler(_dbContext);
        await handler.Handle(command, CancellationToken.None);

        //assert
        var deletedItem = await _dbContext.Items.FirstOrDefaultAsync(x => x.Id == item.Id);
        deletedItem.Should().BeNull();
     
        var existingColor = await _dbContext.Colors.FirstOrDefaultAsync(x => x.Id == color.Id);
        existingColor.Should().NotBeNull();
    }

    [Test]
    public async Task DeleteItem_WithNotExistingItem_ShouldThrowNotFoundException()
    {
        //arrange
        var command = new DeleteItemCommand()
        {
            Id = It.IsAny<int>()
        };
        
        var beforeOperationItemsCount = await _dbContext.Items.CountAsync();

        //act
        var result = await FluentActions.Invoking(() => _sender.Send(command)).Should().ThrowAsync<NotFoundException>();

        //assert
        var existingItemsCount = await _dbContext.Items.CountAsync();
        existingItemsCount.Should().Be(beforeOperationItemsCount);
    }
}
