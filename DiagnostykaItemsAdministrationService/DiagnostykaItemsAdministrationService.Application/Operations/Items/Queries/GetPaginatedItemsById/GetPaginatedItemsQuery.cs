using DiagnostykaItemsAdministrationService.Application.Common.Exceptions;
using DiagnostykaItemsAdministrationService.Application.Common.Helpers;
using DiagnostykaItemsAdministrationService.Application.Operations.Items.Queries.Models;
using DiagnostykaItemsAdministrationService.Domain.Entities;
using DiagnostykaItemsAdministrationService.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace DiagnostykaItemsAdministrationService.Application.Operations.Items.Queries.GetPaginatedItemsById;

public class GetPaginatedItemsQuery : IRequest<PagedModel<ItemDto>>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string? FilterBy { get; set; }
    public string SortingProperty { get; set; }
    public bool SortDescending { get; set; }
}

public class GetPaginatedItemsQueryHandler : IRequestHandler<GetPaginatedItemsQuery, PagedModel<ItemDto>>
{
    private readonly AppDbContext _appDbContext;

    public GetPaginatedItemsQueryHandler(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<PagedModel<ItemDto>> Handle(GetPaginatedItemsQuery request, CancellationToken cancellationToken)
    {
        string sortingProperty = nameof(Item.Id);

        if (!string.IsNullOrEmpty(request.SortingProperty))
            sortingProperty = typeof(Item).GetProperty(request.SortingProperty) is not null ? 
                request.SortingProperty : 
                throw new BadRequestException("Invalid sorting property");

        PagedModel<Item> paginatedItemsEntities = request.SortDescending ?
            paginatedItemsEntities = 
                await _appDbContext.Items
                .Include(i => i.Color)
                .AsNoTracking()
                .Where(x=> x.Name.Contains(request.FilterBy) || x.Code.Contains(request.FilterBy))
                .OrderBy(x => EF.Property<Item>(x, sortingProperty))
                .OrderByDescending(x=> EF.Property<Item>(x, sortingProperty))
                .PaginateAsync(request.Page, request.PageSize, cancellationToken) 
                : 
                await _appDbContext.Items
                .Include(i => i.Color)
                .AsNoTracking()
                .Where(x => x.Name.Contains(request.FilterBy) || x.Code.Contains(request.FilterBy))
                .OrderBy(x => EF.Property<Item>(x, sortingProperty))
                .PaginateAsync(request.Page, request.PageSize, cancellationToken);
        
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