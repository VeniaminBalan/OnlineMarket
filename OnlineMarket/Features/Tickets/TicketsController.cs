using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMarket.DataBase;
using OnlineMarket.Features.Roles;
using OnlineMarket.Features.Roles.Models;
using OnlineMarket.Features.Roles.Views;
using OnlineMarket.Features.Tickets.Models;
using OnlineMarket.Features.Tickets.Views;
using OnlineMarket.Features.Users.Models;
using StudentUptBackend.Database;

namespace OnlineMarket.Features.Tickets;


[ApiController]
[Route("tickets")]
public class TicketsController : ControllerBase
{

    private readonly IRepository<TicketModel> ticketsRepo;
    private readonly IRepository<UserModel> userRepo;
    private readonly IRepository<RoleModel> roleRepo;
    
    public TicketsController(IRepository<TicketModel> ticketsRepo, 
        IRepository<UserModel> userRepo,
        IRepository<RoleModel> roleRepo)
    {
        this.ticketsRepo = ticketsRepo;
        this.userRepo = userRepo;
        this.roleRepo = roleRepo;
    }
    
    [HttpPost("{Id}")]
    public async Task<ActionResult<TicketResponse>> Add([FromRoute]string Id)
    {
        TicketService ticketService = new TicketService(ticketsRepo, userRepo);

        var ticket = await ticketService.CreateTicket(Id);
        if (ticket is null) return NotFound("user not found");

        var res = new TicketResponse()
        {
            Id = ticket.Id,
            ProcessedDate = ticket.ProcessedDate,
            isProcessed = ticket.isProcessed,
            UserId = ticket.UserId,
            Created = ticket.Created
        };
        return Ok(res);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TicketResponse>>> Get()
    {
        var tickets = await ticketsRepo.DbSet
            .Where(t=>t.isProcessed == false)
            .Select(ticket => new TicketResponse
            {
                Id = ticket.Id,
                ProcessedDate = ticket.ProcessedDate,
                isProcessed = ticket.isProcessed,
                UserId = ticket.UserId,
                Created = ticket.Created
            }).ToListAsync();

        return Ok(tickets);
    }

    [HttpPut("{ticketId}")]
    public async Task<ActionResult<TicketResponse>> AddSellerRole([FromRoute] string ticketId)
    {
        var ticket = await ticketsRepo.GetAsync(ticketId);
        if (ticket is null) return NotFound("ticket not found");

        RoleService roleService = new RoleService(roleRepo, userRepo);
        await roleService.AddSellerRole(ticket.UserId);

        ticket.isProcessed = true;
        ticket.ProcessedDate = DateTime.UtcNow;

        ticket = await ticketsRepo.AddOrUpdateAsync(ticket);

        return Ok(new TicketResponse
        {
            Id = ticket.Id,
            ProcessedDate = ticket.ProcessedDate,
            isProcessed = ticket.isProcessed,
            UserId = ticket.UserId,
            Created = ticket.Created
        });
    }

    [HttpDelete("{ticketId}")]
    public async Task<ActionResult<TicketResponse>> Delete([FromRoute] string ticketId)
    {
        var ticket = await ticketsRepo.DeleteAsync(ticketId);
        if (ticket is null) return NotFound("ticket not found");

        return Ok($"ticket with id= {ticket.Id} was successfully deleted");
    }
}