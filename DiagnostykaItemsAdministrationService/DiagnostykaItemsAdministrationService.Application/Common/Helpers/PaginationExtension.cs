using Microsoft.EntityFrameworkCore;

namespace DiagnostykaItemsAdministrationService.Application.Common.Helpers;

public static class PaginationExtension
{
    public static async Task<PagedModel<TModel>> PaginateAsync<TModel>(this IQueryable<TModel> query,
                                                                           int page,
                                                                           int pageSize,
                                                                           CancellationToken cancellationToken)
                                                                           where TModel : class
    {
        var paged = new PagedModel<TModel>();

        page = (page < 0) ? 1 : page;

        paged.CurrentPage = page;
        paged.PageSize = pageSize;
        paged.TotalItems = await query.CountAsync(cancellationToken);

        var startRow = (page - 1) * pageSize;
        paged.Data = await query.Skip(startRow)
                                 .Take(pageSize)
                                 .ToListAsync(cancellationToken);

        paged.TotalPages = (int)Math.Ceiling(paged.TotalItems / (double)pageSize);

        return paged;
    }
}