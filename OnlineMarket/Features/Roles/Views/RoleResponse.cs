using OnlineMarket.Features.Users.Views;

namespace OnlineMarket.Features.Roles.Views;

public class RoleResponse
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public IList<UserResponse> Users { get; set; }
}