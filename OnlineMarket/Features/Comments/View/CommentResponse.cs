using OnlineMarket.Features.Products.Models;
using OnlineMarket.Features.Users.Models;

namespace OnlineMarket.Features.Comments.View;

public class CommentResponse
{
    public string Id { get; set; }
    public string Text { get; set; }
    public DateTime Created { get; set; }

    public string UserId { get; set; }
    public string ProductId { get; set; }
    
}