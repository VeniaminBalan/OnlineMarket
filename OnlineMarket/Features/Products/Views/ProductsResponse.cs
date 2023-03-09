using OnlineMarket.Features.Comments.Models;
using OnlineMarket.Features.Users.Models;

namespace OnlineMarket.Features.Products.Views;

public class ProductsResponse
{
    public UserModel Seller { get; set; }
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsNegotiable { get; set; }
    public double Price { get; set; }
    public int Quantity { get; set; }
    public bool Display { get; set; }

    public List<CommentModel> Comments { get; set; }
}