using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMarket.Features.Charts.Models;
using OnlineMarket.Features.Products.Models;
using OnlineMarket.Features.Products.Views;
using OnlineMarket.Features.Users.Models;
using OnlineMarket.Features.Users.Views;
using OnlineMarket.Utils.Repository;

namespace OnlineMarket.Features.Charts;


[ApiController]
[Route("cart")]
public class CartController : ControllerBase
{
    private readonly IRepository<CartModel> cartRepo;
    private readonly IRepository<ProductModel> productRepo;
    private readonly IRepository<UserModel> userRepo;

    public CartController(IRepository<CartModel> cartRepo,
        IRepository<ProductModel> productRepo,
        IRepository<UserModel> userRepo)
    {
        this.cartRepo = cartRepo;
        this.productRepo = productRepo;
        this.userRepo = userRepo;
    }

    [HttpPost]
    public async Task<ActionResult<CartResponse>> Add(CartRequest request)
    {
        var user = await userRepo.GetAsync(request.UserId);
        if (user is null) return NotFound("user not found");

        var product = await productRepo.GetAsync(request.ProductId);
        if (product is null) return NotFound("product not found");

        var cart = new CartModel
        {
            Customer = user,
            Product = product,
            IsBought = false,
            PurchasedDate = null
        };

        cart = await cartRepo.AddAsync(cart);

        product = await productRepo.DbSet
            .Include(p => p.Seller)
            .FirstOrDefaultAsync(p => p.Id == product.Id);

        return new CartResponse
        {
            Id = cart.Id,
            PurchasedDate = cart.PurchasedDate,
            IsBought = cart.IsBought,
            Product = new ProductResponseForCart
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Seller = new UserResponseForProducts
                {
                    Id = product.Seller.Id,
                    Email = product.Seller.Email,
                    Name = product.Seller.Name
                }
            }
        };
    }


    [HttpGet("{customerId}")]
    public async Task<ActionResult<IEnumerable<CartResponse>>> Get([FromRoute] string customerId)
    {
        var cart = await cartRepo.DbSet
            .Include(c => c.Product)
            .ThenInclude(p => p.Seller)
            .Where(c => c.Customer.Id == customerId)
            .Where(c => c.IsBought == false)
            .ToListAsync();

        var res = cart.Select(c => new CartResponse
        {
            Id = c.Id,
            PurchasedDate = c.PurchasedDate,
            IsBought = c.IsBought,
            Product = new ProductResponseForCart
            {
                Id = c.Product.Id,
                Name = c.Product.Name,
                Description = c.Product.Description,
                Price = c.Product.Price,
                Seller = new UserResponseForProducts
                {
                    Id = c.Product.Seller.Id,
                    Email = c.Product.Seller.Email,
                    Name = c.Product.Seller.Name
                }
            }
        }).ToList();

        return res;
    }
    
    [HttpGet("history/{customerId}")]
    public async Task<ActionResult<IEnumerable<CartResponse>>> GetHistory([FromRoute] string customerId)
    {
        var cart = await cartRepo.DbSet
            .Include(c => c.Product)
            .ThenInclude(p => p.Seller)
            .Include(c => c.Customer)
            .Where(c => c.Customer.Id == customerId)
            .Where(c => c.IsBought == true)
            .ToListAsync();

        var res = cart.Select(c => new CartResponse
        {
            Id = c.Id,
            PurchasedDate = c.PurchasedDate,
            IsBought = c.IsBought,
            Product = new ProductResponseForCart
            {
                Id = c.Product.Id,
                Name = c.Product.Name,
                Description = c.Product.Description,
                Price = c.Product.Price,
                Seller = new UserResponseForProducts
                {
                    Id = c.Product.Seller.Id,
                    Email = c.Product.Seller.Email,
                    Name = c.Product.Seller.Name
                }
            }
        }).ToList();

        return res;
    }
    
    [HttpPut("{userId}")]
    public async Task<ActionResult<CartResponse>> Buy([FromRoute] string userId)
    {
        var cart = await cartRepo.DbSet
            .Include(c => c.Product)
            .ThenInclude(p => p.Seller)
            .Include(c => c.Customer)
            .Where(c => c.Customer.Id == userId)
            .Where(c => c.IsBought == false).
            ToListAsync();
        

        foreach (var c in cart)
        {
            c.IsBought = true;
            c.PurchasedDate = DateTime.UtcNow;
            await cartRepo.AddOrUpdateAsync(c);
        }
        
        return Ok("cart was emptied");
    }
    
    [HttpDelete("{cartId}")]
    public async Task<ActionResult<CartResponse>> Delete([FromRoute] string cartId)
    {
        var cart = await cartRepo.DeleteAsync(cartId);
        if (cart is null) return NotFound("product not found in cart");
        
        return Ok("product was successfully removed from cart");
    }
    
}