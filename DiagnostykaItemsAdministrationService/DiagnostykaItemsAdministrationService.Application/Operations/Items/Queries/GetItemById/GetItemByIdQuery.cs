using DiagnostykaItemsAdministrationService.Application.Common.Exceptions;
using DiagnostykaItemsAdministrationService.Application.Operations.Items.Queries.Models;
using DiagnostykaItemsAdministrationService.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DiagnostykaItemsAdministrationService.Application.Operations.Items.Queries.GetItemById;

public class GetItemByIdQuery : IRequest<ItemDto>
{
    public int Id { get; set; }
}

public class GetItemByIdQueryHandler : IRequestHandler<GetItemByIdQuery, ItemDto>
{
    private readonly AppDbContext _dbContext;

    public GetItemByIdQueryHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ItemDto> Handle(GetItemByIdQuery request, CancellationToken cancellationToken)
    {
        var itemEntity = await _dbContext.Items.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (itemEntity is null)
            throw new NotFoundException($"Item of Id: {request.Id} not found.");

        return new ItemDto()
        {
            Id = itemEntity.Id,
            Name =  itemEntity.Name,
            Code = itemEntity.Code,
            Color = new ColorDto()
            {
                Id = itemEntity.Id,
                Name = itemEntity.Name
            }
        };
    }
}