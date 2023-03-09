using System.ComponentModel.DataAnnotations;

namespace OnlineMarket.Features.Roles.Views;

public class RoleRequest
{
    [Required]public string Name { get; set; }
    [Required]public string Description { get; set; }
}