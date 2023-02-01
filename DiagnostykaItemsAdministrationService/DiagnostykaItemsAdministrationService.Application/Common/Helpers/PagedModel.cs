namespace DiagnostykaItemsAdministrationService.Application.Common.Helpers;

public class PagedModel<TModel> where TModel : class
{
    private const int _maxPageSize = 100;
    private int _pageSize;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > _maxPageSize) ? _maxPageSize : value;
    }

    public int CurrentPage { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
    public IList<TModel> Data { get; set; } = new List<TModel>();
}