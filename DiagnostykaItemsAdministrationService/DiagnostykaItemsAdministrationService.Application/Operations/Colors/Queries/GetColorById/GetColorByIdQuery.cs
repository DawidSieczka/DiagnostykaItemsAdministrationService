using DiagnostykaItemsAdministrationService.Application.Common.Exceptions;
using DiagnostykaItemsAdministrationService.Application.Operations.Colors.Queries.Models;
using DiagnostykaItemsAdministrationService.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnostykaItemsAdministrationService.Application.Operations.Colors.Queries.GetColorById;
public class GetColorByIdQuery : IRequest<ColorDto>
{
    public int Id { get; set; }
}

public class GetColorByIdQueryHandler : IRequestHandler<GetColorByIdQuery, ColorDto>
{
    private readonly AppDbContext _dbContext;

    public GetColorByIdQueryHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<ColorDto> Handle(GetColorByIdQuery request, CancellationToken cancellationToken)
    {
        var colorEntity = await _dbContext.Colors.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (colorEntity is null)
            throw new NotFoundException($"Color of id: {request.Id} not found");

        return new ColorDto()
        {
            Id = colorEntity.Id,
            Name = colorEntity.Name
        };
    }
}
