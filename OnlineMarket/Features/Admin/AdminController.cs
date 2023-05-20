using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using OnlineMarket.DataBase;
using OnlineMarket.Features.Admin.Views;
using OnlineMarket.Features.Comments.View;
using OnlineMarket.Features.Products.Models;
using OnlineMarket.Features.Products.Views;
using OnlineMarket.Features.Users.Views;
using OnlineMarket.Utils.QuerryParams.Filters.PaginationFilter;
using OnlineMarket.Utils.QuerryParams.Filters.SearchFilter;
using OnlineMarket.Utils.QuerryParams.Filters.SortingFilter;
using OnlineMarket.Utils.Repository;
using OnlineMarket.Utils.Services;
using QueryableExtensions = OnlineMarket.Utils.QuerryParams.Extensions.QueryableExtensions;

namespace OnlineMarket.Features.Admin;

[ApiController]
[Route("admin")]
public class AdminController : ControllerBase
{
    private readonly AppDbContext _appDbContext;
    private readonly IRepository<ProductModel> productRepo;
    private readonly IUriService _uriService;

    public AdminController(AppDbContext appDbContext, IRepository<ProductModel> productRepo,
        IUriService uriService)
    {
        _appDbContext = appDbContext;
        this.productRepo = productRepo;
        _uriService = uriService;
        
    }
    
    
    [HttpGet]
    [Route("product")]
    public async Task<ActionResult<IEnumerable<ProductResponseForAdmin>>> GetAllProducts(
        [FromQuery] SearchParams searchParams,
        [FromQuery] PaginationFilter paginatorFilter,
        [FromQuery] SortingParams sortingParams)
    {
        var validfilter = new PaginationFilter(paginatorFilter.PageNumber, paginatorFilter.PageSize);
        
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
            Comments = p.Comments.Select(c => new CommentResponse
            {
                Id = c.Id,
                Text = c.Text,
                Created = c.Created,
                UserId = c.User.Id,
                ProductId = c.Product.Id
            }).ToList()
        });
        
        res = QueryableExtensions.Search(res.ToList() , searchParams);
        res = QueryableExtensions.Sort(res.ToList(), sortingParams);
        
        res = res.Skip((validfilter.PageNumber - 1) * validfilter.PageSize)
            .Take(validfilter.PageSize);
        
        return Ok(res);
    }

}