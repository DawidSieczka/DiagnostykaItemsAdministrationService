using DiagnostykaItemsAdministrationService.Application.Common.Exceptions;
using DiagnostykaItemsAdministrationService.Application.Operations.Colors.Queries.GetColorById;
using DiagnostykaItemsAdministrationService.Application.Operations.Items.Queries.GetItemById;
using DiagnostykaItemsAdministrationService.Domain.Entities;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnostykaItemsAdministrationService.IntegrationTests.Colors.Queries;
public class GetColorByIdQueryTests :TestBase
{
    [Test]
    public async Task GetColorByIdQuery_WithExistingColor_ShouldReturnColorSucessfuly()
    {
        //arrange
        var color = new Color()
        {
            Name = "White"
        };

        await _dbContext.Colors.AddAsync(color);
        await _dbContext.SaveChangesAsync();

       
        var query = new GetColorByIdQuery()
        {
            Id = color.Id
        };

        //act
        var handler = new GetColorByIdQueryHandler(_dbContext);
        var result = await handler.Handle(query, CancellationToken.None);

        //assert
        result.Should().NotBeNull();
        result.Id.Should().Be(color.Id);
        result.Name.Should().Be(color.Name);
    }

    [Test]
    public async Task GetColorByIdQuery_WithNotExistingColor_ShouldThrowNotFoundException()
    {
        //arrange
        var command = new GetColorByIdQuery()
        {
            Id = It.IsAny<int>()
        };

        //act //assert
        var handler = new GetColorByIdQueryHandler(_dbContext);
        await FluentActions.Invoking(() => _sender.Send(command)).Should().ThrowAsync<NotFoundException>();
    }
}
