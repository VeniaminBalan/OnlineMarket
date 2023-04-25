namespace OnlineMarket.Features.Comments.View;

public class CommentRequest
{
    public string UserId { get; set; }
    public string ProductId { get; set; }

    public string Text { get; set; }
}