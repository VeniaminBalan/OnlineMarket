namespace OnlineMarket.Features.Tickets.Views;

public class TicketResponse
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public bool isProcessed { get; set; }
    public DateTime? ProcessedDate { get; set; }
}