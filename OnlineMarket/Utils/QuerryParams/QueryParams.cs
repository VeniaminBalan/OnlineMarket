using OnlineMarket.Utils.QuerryParams.Filters.Filter;
using OnlineMarket.Utils.QuerryParams.Filters.PaginationFilter;
using OnlineMarket.Utils.QuerryParams.Filters.SearchFilter;
using OnlineMarket.Utils.QuerryParams.Filters.SortingFilter;

namespace OnlineMarket.Utils.QuerryParams;

public class QueryParams
{
    public IFilterParams? filterParams;
    public IPaginationFilter? paginationParams;
    public ISearchParams? searchParams;
    public ISortingParams? sortingParams;

    public QueryParams(IFilterParams? filterParams, IPaginationFilter? paginationParams, ISearchParams? searchParams, ISortingParams? sortingParams)
    {
        this.filterParams = filterParams;
        this.paginationParams = paginationParams;
        this.searchParams = searchParams;
        this.sortingParams = sortingParams;
    }
}