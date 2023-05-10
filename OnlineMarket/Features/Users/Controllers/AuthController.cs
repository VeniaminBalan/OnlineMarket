using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMarket.Features.Roles.Models;
using OnlineMarket.Features.Roles.Views;
using OnlineMarket.Features.Tickets;
using OnlineMarket.Features.Tickets.Models;
using OnlineMarket.Features.Users.Models;
using OnlineMarket.Features.Users.Utils;
using OnlineMarket.Features.Users.Views;
using StudentUptBackend.Database;

namespace OnlineMarket.Features.Auth;


[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IRepository<UserModel> userRepo;
    private readonly IRepository<RoleModel> roleRepo;
    private readonly IRepository<TicketModel> ticketsRepo;

    public AuthController(IRepository<UserModel> userRepo, 
        IRepository<RoleModel> roleRepo,
        IRepository<TicketModel> ticketsRepo)
    {
        this.userRepo = userRepo;
        this.roleRepo = roleRepo;
        this.ticketsRepo = ticketsRepo;
    }

    [HttpPost]
    [Route("register")]
    public async Task<ActionResult<UserResponse>> Add(SignUpRequest request)
    {
        var role = await roleRepo.DbSet.FirstOrDefaultAsync(r => r.Name == Roles.Models.Roles.Customer.ToString());
        if (role is null) return NotFound("Customer role not found, please add it");

        var existingUser = await userRepo.DbSet.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (existingUser is not null) return BadRequest("User already exists with that email");

        HashPassword hashedPassword = new HashPassword(request.Password);

        var user = new UserModel
        {
            Name = request.Name,
            Email = request.Email,
            Salt = hashedPassword.GetSalt(),
            Hashed = hashedPassword.GetHashed(),
            Roles = new List<RoleModel>
            {
                role
            }
        };

        user = await userRepo.AddAsync(user);
        if (request.SellerRequest)
        {
            var ticketService = new TicketService(ticketsRepo,userRepo);
            await ticketService.CreateTicket(user.Id);
        }

        var res = new UserResponse
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
        };

        return Created("user", res);
    }

    [HttpGet]
    [Route("login")]
    public async Task<ActionResult<UserResponse>> Add(string Email, string Password)
    {
        var user = await userRepo.DbSet
            .Include(r=>r.Roles)
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
                    Name = role.Name,
                }
            ).ToList()

        };

        return res;
    }
}