using System.Net;
using Moq;
using OnlineMarket.Features.Products.Models;
using OnlineMarket.Utils.Repository;
using Xunit;

namespace OnlineMarket.IntegrationTest.ControllerTests;

public class ProductsControllerTests : IDisposable
{
    private CustomWebApplicationFactory _factory;
    private HttpClient _client;

    public ProductsControllerTests()
    {
        _factory = new CustomWebApplicationFactory();
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task Get_All_Products()
    {
        var mockProducts = new ProductModel[]
        {
            new() { Id = "1", Name = "Table Cloth 62x114 White", Price = 123 },
            new() { Id = "2", Name = "Bread - Olive", Price = 66 },
            new() { Id = "3", Name = "Coffee - Irish Cream", Price = 10 }
        }.AsQueryable();

        _factory.RepositoryMock.Setup(r => r.DbSet.AsQueryable()).Returns(mockProducts);

        var response = await _client.GetAsync("products");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        
    }

    public void Dispose()
    {
        _factory.Dispose();
        _client.Dispose();
    }
}