using OnlineMarket.Base;
using OnlineMarket.Features.Users.Models;

namespace OnlineMarket.Features.Tickets.Models;

public class TicketModel : Model
{
    public UserModel User { get; set; }
    public bool isProcessed { get; set; }
    public DateTime ?ProcessedDate { get; set; }
}