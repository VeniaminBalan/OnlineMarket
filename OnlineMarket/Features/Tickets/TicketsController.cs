using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineMarket.DataBase;
using OnlineMarket.Features.Roles.Views;
using OnlineMarket.Features.Tickets.Models;
using OnlineMarket.Features.Tickets.Views;

namespace OnlineMarket.Features.Tickets;


[ApiController]
[Route("tickets")]
public class TicketsController : ControllerBase
{
    private readonly AppDbContext _appDbContext;
    
    public TicketsController(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    
    [HttpPost("{Id}")]
    public async Task<ActionResult<TicketResponse>> Add([FromRoute]string Id)
    {
        var user = await _appDbContext.Tickets.FirstOrDefaultAsync(t=>);
        
        var ticket = new TicketModel
        {
            UserId = Id,
            ProcessedDate = DateTime.UtcNow
        };

        ticket = (await _appDbContext.Tickets.AddAsync(ticket)).Entity;
        await _appDbContext.SaveChangesAsync();

        var res = new TicketResponse()
        {
            Id = ticket.Id,
            ProcessedDate = ticket.ProcessedDate,
            isProcessed = ticket.isProcessed,
            UserId = ticket.UserId
        };
        return Ok(res);
    }
}