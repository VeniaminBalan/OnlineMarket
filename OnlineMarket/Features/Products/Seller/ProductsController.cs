using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMarket.DataBase;
using OnlineMarket.Features.Products.Models;
using OnlineMarket.Features.Products.Views;
using OnlineMarket.Features.Users.Views;

namespace OnlineMarket.Features.Products;

[ApiController]
[Route("products/seller")]
public class CustomerProductsController : ControllerBase
{
    private readonly AppDbContext _appDbContext;

    public CustomerProductsController(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    
    //post a product by seller
    [HttpPost("{sellerId}")]
    public async Task<ActionResult<ProductsResponse>> Add([FromRoute]string sellerId, ProductsRequest request)
    {
        var user = await _appDbContext.Users
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(seller => sellerId == seller.Id);
        if (user is null) return NotFound("user not found (Vicu sau Dragos -> check cookies)");
        
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
            Comments = null
        };
        
    }

    
    //get all seller's products
    [HttpGet("{SellerId}")]
    public async Task<ActionResult<ProductsResponse>>  Get([FromRoute] string SellerId)
    {
        var products = _appDbContext.Products
            .Include(s=>s.Seller)
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
            }
        });

        return Ok(res);
    }
}