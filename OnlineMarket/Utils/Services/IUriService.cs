using OnlineMarket.Utils.QuerryParams;
using OnlineMarket.Utils.QuerryParams.Filters.PaginationFilter;

namespace OnlineMarket.Utils.Services;

public interface IUriService
{
    public Uri GetPageUri(PaginationFilter filter, string route);
    public Uri GetPageUri(QueryParams querryParams ,string route);
}