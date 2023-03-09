using OnlineMarket.Base;
using OnlineMarket.Features.Products.Models;
using OnlineMarket.Features.Users.Models;

namespace OnlineMarket.Features.Comments.Models;

public class CommentModel : Model
{
    public UserModel User { get; set; }
    public ProductModel Product { get; set; }

    public string Text { get; set; }
}