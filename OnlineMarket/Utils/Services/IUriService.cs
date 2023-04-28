using OnlineMarket.Utils.Filter;

namespace OnlineMarket.Utils.Services;

public interface IUriService
{
    public Uri GetPageUri(PaginationFilter filter, string route);
}