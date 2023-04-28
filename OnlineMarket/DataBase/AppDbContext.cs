using Microsoft.EntityFrameworkCore;
using OnlineMarket.Features.Charts.Models;
using OnlineMarket.Features.Products.Models;
using OnlineMarket.Features.Roles.Models;
using OnlineMarket.Features.Tickets.Models;
using OnlineMarket.Features.Users.Models;

namespace OnlineMarket.DataBase;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options) { }
    
    public DbSet<ProductModel> Products { get; set;}
    public DbSet<UserModel> Users { get; set;}
    public DbSet<RoleModel> Roles { get; set;}
    public DbSet<CartModel> Charts { get; set;}
    public DbSet<TicketModel> Tickets { get; set;}
    
}