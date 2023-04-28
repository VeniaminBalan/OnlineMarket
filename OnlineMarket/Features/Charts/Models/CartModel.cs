using OnlineMarket.Base;
using OnlineMarket.Features.Products.Models;
using OnlineMarket.Features.Users.Models;

namespace OnlineMarket.Features.Charts.Models;

public class CartModel : Model // rename to cartModel
{
    public UserModel Customer { get; set; }
    public ProductModel Product { get; set; }
    public DateTime? PurchasedDate{ get; set; }
    public bool IsBought { get; set; }
}