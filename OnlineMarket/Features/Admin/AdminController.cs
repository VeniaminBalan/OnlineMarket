using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMarket.DataBase;
using OnlineMarket.Features.Roles.Views;
using OnlineMarket.Features.Users.Views;

namespace OnlineMarket.Features.Admin;

[ApiController]
[Route("admin")]
public class AdminController : ControllerBase
{
    private readonly AppDbContext _appDbContext;

    public AdminController(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    
}