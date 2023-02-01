using DiagnostykaItemsAdministrationService.Application.Operations.Colors.Queries.Models;
using DiagnostykaItemsAdministrationService.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DiagnostykaItemsAdministrationService.Application.Operations.Colors.Queries.GetAllColors;

public class GetAllColorsQuery : IRequest<IEnumerable<ColorDto>>
{
}

public class GetAllColorsQueryHandler : IRequestHandler<GetAllColorsQuery, IEnumerable<ColorDto>>
{
    private readonly AppDbContext _dbContext;

    public GetAllColorsQueryHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<ColorDto>> Handle(GetAllColorsQuery request, CancellationToken cancellationToken)
    {
        var colorsEntities = await _dbContext.Colors.AsNoTracking().ToListAsync(cancellationToken);

        return colorsEntities.Select(e => new ColorDto()
        {
            Id = e.Id,
            Name = e.Name
        });
    }
}