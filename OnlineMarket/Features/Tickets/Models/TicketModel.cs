using OnlineMarket.Base;

namespace OnlineMarket.Features.Tickets.Models;

public class TicketModel : Model
{
    public string UserId { get; set; }
    public DateTime ResponseDate { get; set; }
}