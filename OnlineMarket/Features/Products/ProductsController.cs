using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMarket.DataBase;
using OnlineMarket.Features.Products.Models;
using OnlineMarket.Features.Products.Views;

namespace OnlineMarket.Features.Products;

[ApiController]
[Route("products")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _appDbContext;

    public ProductsController(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    
    [HttpPost]
    public async Task<ActionResult<ProductsResponse>> Add(string sellerId, ProductsRequest request)
    {
        var user = await _appDbContext.Users
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(seller => sellerId == seller.Id);
        if (user is null) return NotFound("user not found (Vicu sau Dragos -> check cookies)");
        
        if( (user.Roles.FirstOrDefault(s=>s.Name == "Seller")) is null )
            return NotFound("user does not have seller rights");
        
        var product = new ProductModel
        {
            Id = Guid.NewGuid().ToString(),
            Created = DateTime.UtcNow,
            Updated = DateTime.UtcNow,
            
            Seller = user,
            Name = request.Name,
            Description = request.Description,
            IsNegotiable = request.IsNegotiable,
            Price = request.Price,
            Display = request.Display,
            Quantity = request.Quantity,
            Comments = null
        };
        
        
        product = (await _appDbContext.Products.AddAsync(product)).Entity;
        await _appDbContext.SaveChangesAsync();
        
        return new ProductsResponse
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            IsNegotiable = product.IsNegotiable,
            Price = product.Price,
            Quantity = product.Quantity,
            Display = product.Display,
            Comments = product.Comments
            
        };
        
    }
    
    [HttpGet]
    public async Task<ActionResult<ProductsResponse>> Get()
    {
        var products = await _appDbContext.Products
            .Include(s => s.Seller).ToListAsync();
        
        var res = products.Select(p => new ProductsResponse
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            IsNegotiable = p.IsNegotiable,
            Price = p.Price,
            Quantity = p.Quantity,
            Display = p.Display,
            Seller = p.Seller
        });

        return Ok(res);
    }
    
    [HttpGet("{Id}")]
    public async Task<ActionResult<ProductsResponse>>  Get([FromRoute] string Id)
    {
        var product = _appDbContext.Products
            .Include(s=>s.Seller)
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
            Seller = product.Seller
        };

        return Ok(res);
    }
}