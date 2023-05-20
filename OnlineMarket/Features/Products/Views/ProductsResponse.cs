using OnlineMarket.Features.Comments.Models;
using OnlineMarket.Features.Comments.View;
using OnlineMarket.Features.Users.Models;
using OnlineMarket.Features.Users.Views;

namespace OnlineMarket.Features.Products.Views;

public class ProductsResponse : IProductResponse
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsNegotiable { get; set; }
    public double Price { get; set; }
    public int Quantity { get; set; }
    public bool Display { get; set; }
    public UserResponseForProducts Seller { get; set; }
    public List<CommentResponseForProduct> Comments { get; set; }
}