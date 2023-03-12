using OnlineMarket.Base;

namespace OnlineMarket.Features.Tickets.Models;

public class TicketModel : Model
{
    public string UserId { get; set; }
    public bool isProcessed { get; set; }
    public DateTime ?ProcessedDate { get; set; }
}