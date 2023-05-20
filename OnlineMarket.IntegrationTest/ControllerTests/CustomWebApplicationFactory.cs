using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using OnlineMarket.Features.Products.Models;
using OnlineMarket.Utils.Repository;

namespace OnlineMarket.IntegrationTest;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public Mock<IRepository<ProductModel>> RepositoryMock { get; }

    public CustomWebApplicationFactory()
    {
        RepositoryMock = new Mock<IRepository<ProductModel>>();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureTestServices(services =>
        {
            services.AddSingleton(RepositoryMock.Object);
        });
    }
}