using System.ComponentModel.DataAnnotations;
using OnlineMarket.Features.Users.Models;

namespace OnlineMarket.Features.Products.Views;

public class ProductsRequest
{
    //[Required] public string SellerId { get; set; }
    [Required] public string Name { get; set; }
    [Required] public string Description { get; set; }
    public bool IsNegotiable { get; set; }
    [Required] public double Price { get; set; }
    [Required] public int Quantity { get; set; }
    [Required] public bool Display { get; set; }
}