using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMarket.DataBase;
using OnlineMarket.Features.Admin.Views;
using OnlineMarket.Features.Comments.View;
using OnlineMarket.Features.Products.Models;
using OnlineMarket.Features.Products.Views;
using OnlineMarket.Features.Roles.Views;
using OnlineMarket.Features.Users.Views;
using StudentUptBackend.Database;

namespace OnlineMarket.Features.Admin;

[ApiController]
[Route("admin")]
public class AdminController : ControllerBase
{
    private readonly AppDbContext _appDbContext;
    private readonly IRepository<ProductModel> productRepo;

    public AdminController(AppDbContext appDbContext, IRepository<ProductModel> productRepo)
    {
        _appDbContext = appDbContext;
        this.productRepo = productRepo;
    }
    
    
    [HttpGet]
    [Route("product")]
    public async Task<ActionResult<IEnumerable<ProductResponseForAdmin>>> GetAllProducts()
    {
        var products = await productRepo.DbSet
            .Include(p => p.Seller)
            .Include(p=>p.Comments)
            .ToListAsync();
        
        var res = products.Select(p => new ProductResponseForAdmin
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
            Comments = p.Comments.Select(c=>new CommentResponse
            {
                Id = c.Id,
                Text = c.Text,
                Created = c.Created,
                UserId = c.User.Id,
                ProductId = c.Product.Id
            }).ToList()
        });

        return Ok(res);
    }
    
    [HttpGet("product{Id}")]
    public async Task<ActionResult<ProductResponseForAdmin>> Get([FromRoute]string Id)
    {
        var product = await productRepo.DbSet
            .Include(p => p.Seller)
            .Include(p=>p.Comments)
            .ThenInclude(c=>c.User)
            .FirstOrDefaultAsync(p=>p.Id ==Id);

        if (product is null) return NotFound("product not found");
        
        var res =new ProductResponseForAdmin
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
            Comments = product.Comments.Select(c=>new CommentResponse
            {
                Id = c.Id,
                Text = c.Text,
                Created = c.Created,
                UserId = c.User.Id,
                ProductId = c.Product.Id
            }).ToList()
        };
        
        return Ok(res);
    }

}