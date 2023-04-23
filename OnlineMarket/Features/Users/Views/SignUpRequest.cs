using System.ComponentModel.DataAnnotations;
using OnlineMarket.Features.Roles.Models;

namespace OnlineMarket.Features.Users.Views;

public class SignUpRequest
{
    
    [Required]public string Name { get; set; }
    
    [EmailAddress]
    [Required]
    public string Email { get; set; }
    [Required]public string Password { get; set; }
}