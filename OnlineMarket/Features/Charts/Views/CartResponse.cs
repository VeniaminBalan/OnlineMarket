using OnlineMarket.Features.Products.Models;
using OnlineMarket.Features.Products.Views;
using OnlineMarket.Features.Users.Models;

namespace OnlineMarket.Features.Charts;

public class CartResponse
{
    public string Id { get; set; }
    public ProductResponseForCart Product { get; set; }
    public DateTime? PurchasedDate { get; set; }
    public bool IsBought { get; set; }
}