using System.ComponentModel.DataAnnotations;
using OnlineMarket.Features.Users.Models;

namespace OnlineMarket.Features.Products.Views;

public class ProductsRequestForPatch
{
    //[Required] public string SellerId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool? IsNegotiable { get; set; }
    public double? Price { get; set; }
    public int? Quantity { get; set; }
    public bool? Display { get; set; }
}