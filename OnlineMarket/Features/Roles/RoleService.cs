using Microsoft.EntityFrameworkCore;
using OnlineMarket.Features.Roles.Models;
using OnlineMarket.Features.Roles.Views;
using OnlineMarket.Features.Users.Models;
using OnlineMarket.Utils.Repository;

namespace OnlineMarket.Features.Roles;

public class RoleService
{
    private readonly IRepository<RoleModel> roleRepo;
    private readonly IRepository<UserModel> userRepo;
    
    public RoleService(IRepository<RoleModel> repository, IRepository<UserModel> userRepo)
    {
        this.roleRepo = repository;
        this.userRepo = userRepo;

    }

    public async Task<UserModel> AddSellerRole(string userId)
    {
        var user = await userRepo.DbSet
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.Id == userId);
        
        var memberRole = await roleRepo.DbSet
            .FirstOrDefaultAsync(x => x.Name == Roles.Models.Roles.Seller.ToString());
        if (memberRole is null) return null;
        
        user.Roles.Add(memberRole); // ?
        
        await userRepo.AddOrUpdateAsync(user);

        return user;
    }
}