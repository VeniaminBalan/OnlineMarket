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
        
    [HttpGet]
    public async Task<ActionResult<UserResponse>> Get()
    {
        var users = await _appDbContext.Users
            .Include(x => x.Roles)
            .Select(
            user => new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Roles = user.Roles.Select(
                    role => new RoleResponseForUser
                    {
                        Id = role.Id,
                        Name = role.Name,
                        Description = role.Description
                    }
                ).ToList()
            }).ToListAsync();

        return Ok(users);
    }
}