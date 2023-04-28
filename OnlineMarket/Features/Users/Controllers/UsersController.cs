
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMarket.DataBase;
using OnlineMarket.Features.Roles.Models;
using OnlineMarket.Features.Roles.Views;
using OnlineMarket.Features.Users.Models;
using OnlineMarket.Features.Users.Utils;
using OnlineMarket.Features.Users.Views;
using StudentUptBackend.Database;

namespace OnlineMarket.Features.Users;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly IRepository<UserModel> userRepo;
    private readonly IRepository<RoleModel> roleRepo;

    public UsersController(IRepository<UserModel> userRepo, IRepository<RoleModel> roleRepo)
    {
        this.userRepo = userRepo;
        this.roleRepo = roleRepo;
    }


    [HttpGet]
    public async Task<ActionResult<UserResponse>> Get()
    {
        var users = await userRepo.DbSet
            .Include(r => r.Roles)
            .Select(
                user => new UserResponse
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Roles = user.Roles.Select(
                        role => new RoleResponseForUser
                        {
                            Name = role.Name,
                        }
                    ).ToList()
                }).ToListAsync();

        return Ok(users);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<UserResponse>> AddSellerRole([FromRoute] string id)
    {
        var user = await userRepo.DbSet
            .Include(x => x.Roles)
            .FirstOrDefaultAsync(x => id == x.Id);
        if(user is null) return  NotFound("User does not exist");

        if (user.Roles.FirstOrDefault(r => r.Name == Roles.Models.Roles.Seller.ToString()) != null)
            return BadRequest("user already have seller role");
        
        //--------------------------------------------------------------------------------------------------//
        
        var memberRole = await roleRepo.DbSet
            .FirstOrDefaultAsync(x => x.Name == Roles.Models.Roles.Seller.ToString());
        if (memberRole is null) return NotFound("Member role does not exist");
        
        user.Roles.Add(memberRole); // ?
        
        await userRepo.AddOrUpdateAsync(user);
        
        return Ok(new UserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Roles = user.Roles.Select(
                role => new RoleResponseForUser
                {
                    Name = role.Name,
                }
            ).ToList()
        });
    }

    [HttpPatch("{Id}")]
    public async Task<ActionResult<UserResponse>> Update([FromRoute] string Id, UserRequestForUpdate request)
    {
        return null;
    }


    //[HttpDelete("{ID}")]
}