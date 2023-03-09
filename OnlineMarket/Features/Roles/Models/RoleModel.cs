using OnlineMarket.Base;
using OnlineMarket.Features.Users.Models;

namespace OnlineMarket.Features.Roles.Models;

public class RoleModel : Model
{
    public string Name { get; set; }
    public string Description { get; set; }
    
    public IList<UserModel> Users { get; set; }
}