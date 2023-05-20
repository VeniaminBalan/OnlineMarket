using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mysqlx.Crud;
using OnlineMarket.DataBase;
using OnlineMarket.Features.Comments.Models;
using OnlineMarket.Features.Comments.View;
using OnlineMarket.Features.Products.Models;
using OnlineMarket.Features.Products.Views;
using OnlineMarket.Features.Users.Models;
using OnlineMarket.Features.Users.Views;
using OnlineMarket.Utils.Repository;

namespace OnlineMarket.Features.Products;

[ApiController]
[Route("products/seller")]
public class CustomerProductsController : ControllerBase
{
    private IRepository<ProductModel> productRepo;
    private IRepository<UserModel> userRepo;
    private IRepository<CommentModel> commentRepo;

    public CustomerProductsController(
        IRepository<ProductModel> productRepo, 
        IRepository<UserModel> userRepo,
        IRepository<CommentModel> commentRepo
        )
    {
        this.productRepo = productRepo;
        this.userRepo = userRepo;
        this.commentRepo = commentRepo;
    }
    
    //post a product by seller
    [HttpPost("{sellerId}")]
    public async Task<ActionResult<ProductsResponse>> Add([FromRoute]string sellerId, ProductsRequest request)
    {
        var user = await userRepo.DbSet
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(seller => sellerId == seller.Id);
        if (user is null) return NotFound("seller not found");
        
        if( (user.Roles.FirstOrDefault(s=>s.Name == "Seller")) is null )
            return NotFound("user does not have seller rights");
        
        var product = new ProductModel
        {
            Seller = user,
            Name = request.Name,
            Description = request.Description,
            IsNegotiable = request.IsNegotiable,
            Price = request.Price,
            Display = request.Display,
            Quantity = request.Quantity,
            Comments = null
        };

        product = await productRepo.AddAsync(product);

        return new ProductsResponse
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            IsNegotiable = product.IsNegotiable,
            Price = product.Price,
            Quantity = product.Quantity,
            Display = product.Display,
            Comments = null
        };
        
    }

    //get all seller's products
    [HttpGet("{SellerId}")]
    public async Task<ActionResult<ProductsResponse>>  Get([FromRoute] string SellerId)
    {
        var products = productRepo.DbSet
            .Include(s=>s.Seller)
            .Include(p => p.Comments)
            .ThenInclude(c=>c.User)
            .Where(p => p.Seller.Id == SellerId);
        
        if(products is null) NotFound("Product does not exist");


        var res = products.Select(p => new ProductsResponse
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            IsNegotiable = p.IsNegotiable,
            Price = p.Price,
            Quantity = p.Quantity,
            Display = p.Display,
            Seller = new UserResponseForProducts
            {
                Id = p.Seller.Id,
                Email = p.Seller.Email,
                Name = p.Seller.Name
            },
            Comments = p.Comments.Select(c=> new CommentResponseForProduct
            {
                Id = c.Id,
                Text = c.Text,
                User = new UserResponseForProducts
                {
                    Id = c.User.Id,
                    Email = c.User.Email,
                    Name = c.User.Name
                }
            }).ToList()
        });

        return Ok(res);
    }
    
    //update seller's product
    [HttpPatch("{Id}")]
    public async Task<ActionResult<ProductsResponse>> Update([FromRoute]string Id, ProductsRequestForPatch request)
    {
        if (request.Name == "string" || request.Name == "") request.Name = null;
        if (request.Description == "string" || request.Description == "") request.Description = null;

        var product = await productRepo.UpdateAsync(Id, request);
        if (product is null) return NotFound("product not found");

        product = await productRepo.DbSet
            .Include(p => p.Seller)
            .Include(p => p.Comments)
            .ThenInclude(c=>c.User)
            .FirstOrDefaultAsync(p => p.Id == Id);

        var ret = new ProductsResponse
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            IsNegotiable = product.IsNegotiable,
            Price = product.Price,
            Quantity = product.Quantity,
            Display = product.Display,
            Seller = new UserResponseForProducts
            {
                Id = product.Seller.Id,
                Email = product.Seller.Email,
                Name = product.Seller.Name
            },
            Comments = product.Comments.Select(c=> new CommentResponseForProduct
            {
                Id = c.Id,
                Text = c.Text,
                User = new UserResponseForProducts
                {
                    Id = c.User.Id,
                    Email = c.User.Email,
                    Name = c.User.Name
                }
            }).ToList()
        };

        return Ok(ret);
    }

    [HttpDelete("{Id}")]
    public async Task<ActionResult<ProductsResponse>> Update([FromRoute] string Id)
    {
        var product = await productRepo.DbSet
            .Include(p => p.Comments)
            .FirstOrDefaultAsync(p => p.Id == Id);

        if (product is null) 
            return NotFound("product not found");

        var comments = product.Comments;


        string response = "";
        foreach (var c in comments)
        {
            await commentRepo.DeleteAsync(c.Id);
            response += $"\n\tcomment with id={c.Id} was deleted";
        }

        product = await productRepo.DeleteAsync(Id);

        return Ok($"product with id={product.Id} was successfully deleted" + response);
    }

}