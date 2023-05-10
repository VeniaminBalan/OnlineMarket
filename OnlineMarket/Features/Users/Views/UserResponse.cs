using OnlineMarket.Features.Roles.Views;

namespace OnlineMarket.Features.Users.Views;

public class UserResponse
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public IList<RoleResponseForUser> Roles { get; set; }
}