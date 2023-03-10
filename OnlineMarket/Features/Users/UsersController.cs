using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMarket.DataBase;
using OnlineMarket.Features.Roles.Models;
using OnlineMarket.Features.Roles.Views;
using OnlineMarket.Features.Users.Models;
using OnlineMarket.Features.Users.Utils;
using OnlineMarket.Features.Users.Views;

namespace OnlineMarket.Features.Users;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _appDbContext;

    public UsersController(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    
    [HttpPost]
    public async Task<ActionResult<UserResponse>> Add( UserRequest request)
    {
        var role = await _appDbContext.Roles.FirstOrDefaultAsync(r => r.Name == "Customer");
        if (role is null) return NotFound("Member not found");
        
        var  existingUser = await _appDbContext.Users.FirstOrDefaultAsync(r=> r.Email == request.Email);
        if (existingUser is not null) return BadRequest("User already exists with that email");
        
        existingUser = await _appDbContext.Users.FirstOrDefaultAsync(r=> r.Name == request.Name);
        if (existingUser is not null) return BadRequest("User already exists with that name");

        HashPassword hashedPassword = new HashPassword(request.Password);

        var user = new UserModel
        {
            Id = Guid.NewGuid().ToString(),
            Created = DateTime.UtcNow,
            Updated = DateTime.UtcNow,
            Name = request.Name,
            Email = request.Email,
            Salt = hashedPassword.GetSalt(),
            Hashed = hashedPassword.GetHashed(),
            Roles = new List<RoleModel>
            {
                role
            }
        };

        user = (await _appDbContext.Users.AddAsync(user)).Entity;
        await _appDbContext.SaveChangesAsync();
       
        var res = new UserResponse
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
        };

        return Created("user", res);
    }
    
    /*
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
    }*/

    [HttpGet]
    public async Task<ActionResult<UserResponse>> Get(string Email, string Password)
    {
        var user = await _appDbContext.Users
            .Include(x => x.Roles)
            .FirstOrDefaultAsync(user => user.Email == Email);

        if (user is null) return BadRequest("Email or password are incorect");

        if (!HashPassword.isValid(Password, user.Salt, user.Hashed))
            return BadRequest("Password is incorect");

        var res = new UserResponse
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Roles = user.Roles.Select(role => new RoleResponseForUser
                {
                    Id = role.Id,
                    Name = role.Name,
                    Description = role.Description
                }
            ).ToList()

        };

        return res;
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<UserResponse>> AddSellerRole([FromRoute] string id)
    {
        var user = await _appDbContext.Users.Include(x => x.Roles).FirstOrDefaultAsync(x => id == x.Id);
        if(user is null) return  NotFound("User does not exist");
        
        var memberRole = await _appDbContext.Roles.FirstOrDefaultAsync(x => x.Name == "Seller");
        if (memberRole is null) return NotFound("Member role does not exist");
        
        user.Roles.Add(memberRole); // ?

        // user.Roles.
        await _appDbContext.SaveChangesAsync();
        
        return Ok(new UserResponse
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
        });
    }
}