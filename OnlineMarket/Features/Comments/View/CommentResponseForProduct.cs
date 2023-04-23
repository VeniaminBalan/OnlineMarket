using OnlineMarket.Features.Users.Views;

namespace OnlineMarket.Features.Comments.View;

public class CommentResponseForProduct
{
    public string Id { get; set; }
    public string Text { get; set; }
    public UserResponseForProducts User { get; set; }
}