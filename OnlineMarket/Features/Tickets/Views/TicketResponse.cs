using OnlineMarket.Features.Users.Views;
namespace OnlineMarket.Features.Tickets.Views;

public class TicketResponse
{
    public string Id { get; set; }
    public UserResponse User { get; set; }
    public bool isProcessed { get; set; }
    public DateTime? ProcessedDate { get; set; }
    public DateTime Created { get; set; }
}