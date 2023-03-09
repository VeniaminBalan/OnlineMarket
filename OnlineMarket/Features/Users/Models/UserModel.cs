using OnlineMarket.Base;
using OnlineMarket.Features.Roles.Models;

namespace OnlineMarket.Features.Users.Models;

public class UserModel : Model
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Salt { get; set; }
    public string Hashed { get; set; }
    public IList<RoleModel> Roles { get; set; }
}