using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMarket.DataBase;
using OnlineMarket.Features.Comments.Models;
using OnlineMarket.Features.Comments.View;
using OnlineMarket.Features.Products.Models;
using OnlineMarket.Features.Products.Views;
using OnlineMarket.Features.Users.Views;
using StudentUptBackend.Database;

namespace OnlineMarket.Features.Products;

[ApiController]
[Route("products")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _appDbContext;
    private readonly IRepository<ProductModel> productRepo;

    public ProductsController(AppDbContext appDbContext, IRepository<ProductModel> productRepo)
    {
        _appDbContext = appDbContext;
        this.productRepo = productRepo;
    }
    
    // get all available products
    [HttpGet]
    public async Task<ActionResult<ProductsResponse>> Get()
    {
        var products = await productRepo.DbSet
            .Include(s => s.Seller)
            .Where(p=>p.Display == true)
            .ToListAsync();
        
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
            }
        });

        return Ok(res);
    }
    
    //get product by id
    [HttpGet("{Id}")]
    public async Task<ActionResult<ProductsResponse>>  Get([FromRoute] string Id)
    {
        var product = productRepo.DbSet
            .Include(s=>s.Seller)
            .Include(c=>c.Comments)
            .FirstOrDefault(p => p.Id == Id);
        if(product is null) NotFound("Product does not exist");


        var res = new ProductsResponse
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
            }
        };

        return Ok(res);
    }

}