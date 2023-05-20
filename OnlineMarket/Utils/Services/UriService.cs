using Microsoft.AspNetCore.WebUtilities;
using OnlineMarket.Utils.QuerryParams;
using OnlineMarket.Utils.QuerryParams.Filters.PaginationFilter;

namespace OnlineMarket.Utils.Services;

public class UriService : IUriService
{
    private string _baseUri;

    public UriService(string baseUri)
    {
        _baseUri = baseUri;
    }

    public Uri GetPageUri(PaginationFilter filter, string route)
    {
        var _enpointUri = new Uri(string.Concat(_baseUri, route));
        var modifiedUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), "pageNumber", filter.PageNumber.ToString());
        modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", filter.PageSize.ToString());
        return new Uri(modifiedUri);
    }
    public Uri GetPageUri(QueryParams queryParams,string route)
    {
        var _enpointUri = new Uri(string.Concat(_baseUri, route));
        string modifiedUri = _enpointUri.ToString();  

        if (queryParams.filterParams is not null)
        {
            var filterParams = queryParams.filterParams;
            if (filterParams.FilterBy is not null)
            {
                modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "FilterBy", filterParams.FilterBy);
                modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "Value", filterParams.Value);
            }
        }
        
        if (queryParams.searchParams.Search is not null)
        {
            var searchParams = queryParams.searchParams;
            if (searchParams.Search is not null)
            {
                modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "Search", searchParams.Search);
            }
        }
        
        if (queryParams.sortingParams is not null)
        {
            var sortingParams = queryParams.sortingParams;
            if (sortingParams.Direction is not null)
            {
                modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "Column", sortingParams.Column);
                modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "Direction", sortingParams.Direction);
            }
        }
        
        if (queryParams.paginationParams is not null)
        {
            var paginatorParams = queryParams.paginationParams;
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageNumber", paginatorParams.PageNumber.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", paginatorParams.PageSize.ToString());
        }
        
        Console.WriteLine(modifiedUri);
        return new Uri(modifiedUri);
    }
}