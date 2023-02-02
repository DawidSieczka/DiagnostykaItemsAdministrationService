using DiagnostykaItemsAdministrationService.Application.Common.Exceptions;
using DiagnostykaItemsAdministrationService.Application.Operations.Items.Commands.DeleteItem;
using DiagnostykaItemsAdministrationService.Application.Operations.Items.Queries.GetItemById;
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

namespace DiagnostykaItemsAdministrationService.IntegrationTests.Items.Queries;
public  class GetItemByIdQueryTests : TestBase
{
    [Test]
    public async Task GetItemByIdQuery_WithExistingItem_ShouldReturnItemSucessfuly()
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

        var command = new GetItemByIdQuery()
        {
            Id = item.Id
        };

        //act
        var handler = new GetItemByIdQueryHandler(_dbContext);
        var result = await handler.Handle(command, CancellationToken.None);

        //assert
        result.Should().NotBeNull();
        result.Id.Should().Be(item.Id);
        result.Name.Should().Be(item.Name);
        result.Code.Should().Be(item.Code);
        result.Color.Should().NotBeNull();
        result.Color.Id.Should().Be(color.Id);
        result.Color.Name.Should().Be(color.Name);
    }

    [Test]
    public async Task GetItemByIdQuery_WithExistingItemAndDeletedColor_ShouldReturnItemWithNullColorSucessfuly()
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

        _dbContext.Colors.Remove(color);
        await _dbContext.SaveChangesAsync();

        var command = new GetItemByIdQuery()
        {
            Id = item.Id
        };

        //act
        var handler = new GetItemByIdQueryHandler(_dbContext);
        var result = await handler.Handle(command, CancellationToken.None);

        //assert
        result.Should().NotBeNull();
        result.Id.Should().Be(item.Id);
        result.Name.Should().Be(item.Name);
        result.Code.Should().Be(item.Code);
        result.Color.Should().BeNull();
    }

    [Test]
    public async Task GetItemByIdQuery_WithNotExistingItem_ShouldThrowNotFoundException()
    {
        //arrange
        
        var command = new GetItemByIdQuery()
        {
            Id = It.IsAny<int>()
        };

        //act //assert
        var handler = new GetItemByIdQueryHandler(_dbContext);
        await FluentActions.Invoking(() => _sender.Send(command)).Should().ThrowAsync<NotFoundException>();
    }
}
