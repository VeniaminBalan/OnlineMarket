using OnlineMarket.Utils.QuerryParams.Extensions;
using OnlineMarket.Utils.QuerryParams.Filters.SearchFilter;
using OnlineMarket.Utils.QuerryParams.Filters.SortingFilter;
using Xunit;

namespace OnlineMarket.UnitTest.Tests;

public class QueryableExtensionsTest
{
    private readonly string path =
        "E:\\UPT\\Anul II\\Sem II\\FIS\\Project\\BackEnd\\OnlineMarket\\OnlineMarket.UnitTest\\MockData\\ProductModelsMockData.json";
    [Fact]
    public void SearchTest()
    {
        var mockData = new MockData.MockData(path);

        var products = mockData.GetProducts();

        SearchParams search = new SearchParams
        {
            Search = "Bread"
        };
        products = QueryableExtensions.Search(products, search);
        
        Assert.Equal(products.Count(), 1);
    }
    
    [Fact]
    public void SearchForNullTest()
    {
        var mockData = new MockData.MockData(path);

        var products = mockData.GetProducts();

        SearchParams search = new SearchParams
        {
            Search = "..."
        };
        products = QueryableExtensions.Search(products, search);
        
        Assert.Equal(products.Count(), 0);
    }
    [Fact]
    public void SearchForFTest()
    {
        var mockData = new MockData.MockData(path);
        var products = mockData.GetProducts();

        SearchParams search = new SearchParams
        {
            Search = "F"
        };
        products = QueryableExtensions.Search(products, search);
        
        Assert.Equal(products.Count(), 2);
    }

    [Fact]
    public void SortPriceAscTest()
    {
        var mockData = new MockData.MockData(path);
        var products = mockData.GetProducts();

        SortingParams sortingParams = new SortingParams
        {
            Column = "Price",
            Direction = "asc"
        };
        products = QueryableExtensions.Sort(products, sortingParams).ToList();
        
        Assert.Equal(products.First().Price, 15);
        Assert.Equal(products.Last().Price, 90);
    }
    
    [Fact]
    public void SortPriceDescTest()
    {
        var mockData = new MockData.MockData(path);
        var products = mockData.GetProducts();

        SortingParams sortingParams = new SortingParams
        {
            Column = "Price",
            Direction = "desc"
        };
        products = QueryableExtensions.Sort(products, sortingParams).ToList();
        
        Assert.Equal(products.First().Price, 90);
        Assert.Equal(products.Last().Price, 15);
    }
    
    [Fact]
    public void SortPriceInvalidDirectionTest()
    {
        var mockData = new MockData.MockData(path);
        var products = mockData.GetProducts();

        SortingParams sortingParams = new SortingParams
        {
            Column = "Price",
            Direction = "..."
        };
        products = QueryableExtensions.Sort(products, sortingParams).ToList();
        
        Assert.Equal(products.First().Price, 90);
    }
    
    [Fact]
    public void SortPriceInvalidColumnTest()
    {
        var mockData = new MockData.MockData(path);
        var products = mockData.GetProducts();

        SortingParams sortingParams = new SortingParams
        {
            Column = "...",
            Direction = "asc"
        };

        Assert.Throws<System.NullReferenceException>(() =>
            products = QueryableExtensions.Sort(products, sortingParams).ToList());
    }
    

}