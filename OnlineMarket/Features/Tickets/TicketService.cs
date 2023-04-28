using OnlineMarket.Features.Tickets.Models;
using OnlineMarket.Features.Tickets.Views;
using OnlineMarket.Features.Users.Models;
using StudentUptBackend.Database;

namespace OnlineMarket.Features.Tickets;

public class TicketService
{
    private readonly IRepository<TicketModel> ticketsRepo;
    private readonly IRepository<UserModel> userRepo;
    
    public TicketService(IRepository<TicketModel> ticketsRepo,
        IRepository<UserModel> userRepo)
    {
        this.ticketsRepo = ticketsRepo;
        this.userRepo = userRepo;
    }

    public async Task<TicketModel> CreateTicket(string userId)
    {
        var user = await userRepo.GetAsync(userId);
        if (user is null) return null;

        var ticket = new TicketModel
        {
            UserId = user.Id,
            isProcessed = false,
            ProcessedDate = null
        };

        ticket = await ticketsRepo.AddAsync(ticket);
        
        return ticket;
    }
}