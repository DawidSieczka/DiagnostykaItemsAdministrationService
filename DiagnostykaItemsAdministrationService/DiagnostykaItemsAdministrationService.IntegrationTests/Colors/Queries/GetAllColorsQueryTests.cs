using DiagnostykaItemsAdministrationService.Application.Operations.Colors.Queries.GetAllColors;
using DiagnostykaItemsAdministrationService.Application.Operations.Items.Queries.GetItemById;
using DiagnostykaItemsAdministrationService.Application.Operations.Items.Queries.Models;
using DiagnostykaItemsAdministrationService.Domain.Entities;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnostykaItemsAdministrationService.IntegrationTests.Colors.Queries;
public class GetAllColorsQueryTests : TestBase
{
    [Test]
    [TestCase(0)]
    [TestCase(1)]
    [TestCase(10)]
    public async Task GetAllColors_WithExistingData_ShouldReturnColorsSucessfuly(int colorsAmount)
    {
        //arrange
        var colors = new List<Color>();

        for (int i = 0; i < colorsAmount; i++)
        {
            colors.Add(new Color()
            {
                Name = $"colorName{i}"
            });

        }

        await _dbContext.Colors.AddRangeAsync(colors);
        await _dbContext.SaveChangesAsync();

        var query = new GetAllColorsQuery();

        //act
        var handler = new GetAllColorsQueryHandler(_dbContext);
        var result = await handler.Handle(query, CancellationToken.None);

        //assert
        result.Should().NotBeNull();
        result.Count().Should().Be(colorsAmount);

        var colorsDtos = colors.Select(x => new ColorDto()
        {
            Id = x.Id,
            Name = x.Name
        });
        colorsDtos.Select(x => x.Id).Should().ContainInConsecutiveOrder(result.Select(x => x.Id));

        var innerJoinedColorsDtos = colorsDtos.Where(p => result.Any(p2 => p2.Id == p.Id)).ToList();
        result.Should().BeEquivalentTo(innerJoinedColorsDtos);
    }
}
