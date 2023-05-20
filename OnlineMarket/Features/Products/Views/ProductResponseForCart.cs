using OnlineMarket.Features.Users.Views;

namespace OnlineMarket.Features.Products.Views;

public class ProductResponseForCart : IProductResponse
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public UserResponseForProducts Seller { get; set; }
}