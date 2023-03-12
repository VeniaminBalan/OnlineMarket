﻿using System.ComponentModel.DataAnnotations;

namespace OnlineMarket.Features.Tickets.Views;

public class TicketRequest
{
    [Required] public string UserId { get; set; }
    [Required] public DateTime ProcessedDate { get; set; }
}