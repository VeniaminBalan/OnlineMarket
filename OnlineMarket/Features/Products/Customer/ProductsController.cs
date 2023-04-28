using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMarket.DataBase;
using OnlineMarket.Features.Comments.Models;
using OnlineMarket.Features.Comments.View;
using OnlineMarket.Features.Products.Models;
using OnlineMarket.Features.Products.Views;
using OnlineMarket.Features.Users.Views;
using OnlineMarket.Utils.Filter;
using OnlineMarket.Utils.Helpers;
using OnlineMarket.Utils.Services;
using OnlineMarket.Utils.Wrappers;
using StudentUptBackend.Database;

namespace OnlineMarket.Features.Products;

[ApiController]
[Route("products")]
public class ProductsController : ControllerBase
{
    private readonly IRepository<ProductModel> productRepo;
    private readonly IUriService _uriService;

    public ProductsController(IRepository<ProductModel> productRepo,
        IUriService uriService)
    {
        this.productRepo = productRepo;
        _uriService = uriService;
    }
    
    // get all available products
    [HttpGet]
    public async Task<ActionResult<ProductsResponse>> Get([FromQuery] PaginationFilter filter)
    {
        var validfilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
        var route = Request.Path.Value;

        var products = await productRepo.DbSet
            .Include(s => s.Seller)
            .Where(p=>p.Display == true)
            .Skip((validfilter.PageNumber - 1) * validfilter.PageSize) //
            .Take(validfilter.PageSize) //
            .ToListAsync();

        var totalRecords = await productRepo.DbSet.CountAsync();
        
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
        }).ToList();
        var pagedResponse = PaginationHelper.CreatePagedReponse<ProductsResponse>(res, validfilter, totalRecords, _uriService, route);
        return Ok(pagedResponse);
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

        return Ok(new Response<ProductsResponse>(res));
    }

}