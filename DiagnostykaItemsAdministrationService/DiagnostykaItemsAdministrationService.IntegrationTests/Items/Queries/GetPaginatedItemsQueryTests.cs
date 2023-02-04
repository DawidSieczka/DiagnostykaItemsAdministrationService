using DiagnostykaItemsAdministrationService.Application.Common.Helpers;
using DiagnostykaItemsAdministrationService.Application.Operations.Items.Queries.GetPaginatedItemsById;
using DiagnostykaItemsAdministrationService.Application.Operations.Items.Queries.Models;
using DiagnostykaItemsAdministrationService.Domain.Entities;
using DiagnostykaItemsAdministrationService.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using System.Linq;

namespace DiagnostykaItemsAdministrationService.IntegrationTests.Items.Queries;

public class GetPaginatedItemsQueryTests : TestBase
{
    [Test]
    [TestCase(0, 1, 10)]
    [TestCase(1, 1, 10)]
    [TestCase(2, 1, 10)]
    [TestCase(10, 1, 5)]
    [TestCase(10, 2, 5)]
    [TestCase(100, 3, 10)]
    [TestCase(999, 10, 100)]
    public async Task GetPaginatedItemsQuery_WithExistingData_ShouldReturnPaginatedResultSucessfuly(int dataAmount, int currentPage, int pageSize)
    {
        //arrange
        var color = new Color()
        {
            Name = "White"
        };

        await _dbContext.Colors.AddAsync(color);
        await _dbContext.SaveChangesAsync();

        var items = new List<Item>();
        for (int i = 0; i < dataAmount; i++)
        {
            items.Add(new Item()
            {
                Name = "anyName",
                ColorId = color.Id,
                Code = new HashGenerator().GenerateHash(i)
            });
        }

        await _dbContext.Items.AddRangeAsync(items);
        await _dbContext.SaveChangesAsync();

        var query = new GetPaginatedItemsQuery()
        {
            Page = currentPage,
            PageSize = pageSize,
            FilterBy = "",
            SortingProperty = "",
            SortDescending = false
        };

        //Act
        var result = await _sender.Send(query);

        //Assert
        result.Should().NotBeNull();
        result.CurrentPage.Should().Be(currentPage);
        result.PageSize.Should().Be(pageSize);
        result.TotalItems.Should().Be(dataAmount);
        result.TotalPages.Should().Be((int)Math.Ceiling(dataAmount / (double)pageSize));
        result.Data.Should().NotBeNull();

        var itemsDtos = items.Select(x => new ItemDto()
        {
            Id = x.Id,
            Name = x.Name,
            Color = (x.Color != null) ? new ColorDto()
            {
                Id = x.Color.Id,
                Name = x.Color.Name
            } : null,
            Code = x.Code
        }).ToList();

        itemsDtos.Select(x=>x.Id).Should().ContainInConsecutiveOrder(result.Data.Select(x=>x.Id));

        var innerJoinedItemsDtos = itemsDtos.Where(p => result.Data.Any(p2 => p2.Id == p.Id)).ToList();
        result.Data.Should().BeEquivalentTo(innerJoinedItemsDtos);
    }


    [Test]
    public async Task GetPaginatedItemsQuery_WithExistingDataAndNotExistingColor_ShouldReturnPaginatedResultWithoutColorSucessfuly()
    {
        //arrange

        var item = new Item()
        {
            Name = "anyName",
            Code = "12Characters"
        };

        await _dbContext.Items.AddRangeAsync(item);
        await _dbContext.SaveChangesAsync();

        var query = new GetPaginatedItemsQuery()
        {
            Page = 1,
            PageSize = 1,
            FilterBy = "",
            SortingProperty = "",
            SortDescending = false
        };

        //Act
        var result = await _sender.Send(query);

        //Assert
        result.Should().NotBeNull();
        result.CurrentPage.Should().Be(1);
        result.PageSize.Should().Be(1);
        result.TotalItems.Should().Be(1);
        result.TotalPages.Should().Be((int)Math.Ceiling(1 / (double)1));
        result.Data.Should().NotBeNull();
        result.Data.FirstOrDefault().Should().BeEquivalentTo(new ItemDto()
        {
            Id = item.Id,
            Name = item.Name,
            Code = item.Code,
            Color = (item.Color != null) ? new ColorDto()
            {
                Id = item.Color.Id,
                Name = item.Color.Name
            } : null,
        });
    }
}