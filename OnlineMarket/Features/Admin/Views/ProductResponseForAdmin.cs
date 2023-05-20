using OnlineMarket.Features.Comments.View;
using OnlineMarket.Features.Products.Views;
using OnlineMarket.Features.Users.Views;

namespace OnlineMarket.Features.Admin.Views;

public class ProductResponseForAdmin : IProductResponse
{
    public string Id { get; set; }
    public UserResponseForProducts Seller { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsNegotiable { get; set; }
    public double Price { get; set; }
    public int Quantity { get; set; }
    public bool Display { get; set; }
    public List<CommentResponse> Comments { get; set; }
}