using DiagnostykaItemsAdministrationService.Application.Common.Helpers;
using DiagnostykaItemsAdministrationService.Application.Operations.Items.Queries.Models;
using DiagnostykaItemsAdministrationService.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiagnostykaItemsAdministrationService.Application.Operations.Items.Queries.GetPaginatedItemsById;
public class GetPaginatedItemsByIdQuery : IRequest<PagedModel<ItemDto>>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
}

public class GetPaginatedItemsByIdQueryHandler : IRequestHandler<GetPaginatedItemsByIdQuery, PagedModel<ItemDto>>
{
    private readonly AppDbContext _appDbContext;

    public GetPaginatedItemsByIdQueryHandler(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    public async Task<PagedModel<ItemDto>> Handle(GetPaginatedItemsByIdQuery request, CancellationToken cancellationToken)
    {
        var paginatedItemsEntities = await _appDbContext.Items.Include(i=>i.Color).AsNoTracking().PaginateAsync(request.Page, request.PageSize,cancellationToken);

        return new PagedModel<ItemDto>()
        {
            CurrentPage = paginatedItemsEntities.CurrentPage,
            PageSize = paginatedItemsEntities.PageSize,
            TotalItems = paginatedItemsEntities.TotalItems,
            TotalPages = paginatedItemsEntities.TotalPages,
            Data = paginatedItemsEntities.Data.Select(e => new ItemDto()
            { 
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                Color = (e.Color != null) ? new ColorDto()
                {
                    Id = e.Color.Id,
                    Name = e.Color.Name
                } : null,
            }).ToList()
        };
    }
}
