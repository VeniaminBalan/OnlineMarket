using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using OnlineMarket.Features.Products.Models;
using OnlineMarket.Features.Products.Views;
using OnlineMarket.Features.Users.Views;
using OnlineMarket.Utils.QuerryParams.Filters.PaginationFilter;
using OnlineMarket.Utils.QuerryParams.Filters.SearchFilter;
using OnlineMarket.Utils.QuerryParams.Filters.SortingFilter;
using OnlineMarket.Utils.Repository;
using OnlineMarket.Utils.Services;
using QueryableExtensions = OnlineMarket.Utils.QuerryParams.Extensions.QueryableExtensions;

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
    public async Task<ActionResult<IEnumerable<ProductsResponse>>> Get(
        [FromQuery] SearchParams searchParams,
        [FromQuery] SortingParams sortingParams,
        [FromQuery] PaginationFilter filter)
    {
        var validfilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

        var products = await productRepo.DbSet
            .Include(s => s.Seller)
            .Where(p=>p.Display == true)
            .ToListAsync();
        
        products = QueryableExtensions.Search(products, searchParams);
        products = QueryableExtensions.Sort(products, sortingParams).ToList();
        products = products
            .Skip((validfilter.PageNumber - 1) * validfilter.PageSize)
            .Take(validfilter.PageSize).ToList();

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
    public async Task<ActionResult<IEnumerable<ProductsResponse>>>  Get([FromRoute] string Id)
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