using OnlineMarket.Features.Admin.Views;
using OnlineMarket.Features.Products.Views;
using OnlineMarket.Utils.QuerryParams.Filters.Filter;
using OnlineMarket.Utils.QuerryParams.Filters.SearchFilter;
using OnlineMarket.Utils.QuerryParams.Filters.SortingFilter;

namespace OnlineMarket.Utils.QuerryParams.Extensions;

public static class QueryableExtensions
{
    
    public static IList<T> Sort<T>(IList<T> list, ISortingParams queryParams) where T : class
    {
        if (queryParams.Column is null)
            return list;

        return queryParams.Direction switch
        {
            "asc" => list.OrderBy(o=>
            {
                return o.GetType().GetProperty(queryParams.Column)!.GetValue(o, null);
                
            }).ToList(),
            "desc" => list.OrderByDescending(o=>
            {
                return o.GetType().GetProperty(queryParams.Column)!.GetValue(o, null);
                
            }).ToList(),
            _ => list
        };
    }

    public static List<ProductResponseForAdmin> Search(List<ProductResponseForAdmin> list, ISearchParams searchParams)
    {
        if (searchParams.Search is null)
        {
            return list;
        }

        var search = searchParams.Search.ToLower();

        var ret = list.Where(p =>
        {
            return p.Name.ToLower().Contains(search);
        }).ToList();

        return ret;
    }
    
    public static List<ProductsResponse> Search(List<ProductsResponse> list, ISearchParams searchParams)
    {
        if (searchParams.Search is null)
        {
            return list;
        }

        var search = searchParams.Search.ToLower();

        var ret = list.Where(p =>
        {
            return p.Name.ToLower().Contains(search);
        }).ToList();

        return ret;
    }
}