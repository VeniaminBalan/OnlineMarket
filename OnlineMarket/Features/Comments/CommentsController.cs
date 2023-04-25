using Microsoft.AspNetCore.Mvc;
using OnlineMarket.Features.Comments.Models;
using OnlineMarket.Features.Comments.View;
using OnlineMarket.Features.Products.Models;
using OnlineMarket.Features.Users.Models;
using StudentUptBackend.Database;

namespace OnlineMarket.Features.Comments;

[ApiController]
[Route("comments")]
public class CommentsController : ControllerBase
{
    private readonly IRepository<CommentModel> commentRepo;
    private readonly IRepository<UserModel> userRepo;
    private readonly IRepository<ProductModel> productRepo;
    
    public CommentsController(IRepository<CommentModel> commentRepo,
        IRepository<UserModel> userRepo,
        IRepository<ProductModel> productRepo
        )
    {
        this.userRepo = userRepo;
        this.productRepo = productRepo;
        this.commentRepo = commentRepo;
    }

    [HttpPost]
    public async Task<ActionResult<CommentResponse>> Add(CommentRequest request)
    {
        var product = await productRepo.GetAsync(request.ProductId);
        if (product is null) return NotFound("Product not found");
        
        var user = await userRepo.GetAsync(request.UserId);
        if (user is null) return NotFound("User not found");

        var comment = new CommentModel
        {
            Text = request.Text,
            Product = product,
            User = user
        };

        comment = await commentRepo.AddAsync(comment);

        var res = new CommentResponse
        {
            Id = comment.Id,
            Text = comment.Text,
            Created = comment.Created,
            ProductId = product.Id,
            UserId = user.Id
        };

        return Created("Comment was created",res);

    }
}