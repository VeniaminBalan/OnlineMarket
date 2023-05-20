namespace OnlineMarket.Utils.QuerryParams.Filters.PaginationFilter;

public interface IPaginationFilter
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}