namespace OnlineMarket.Features.Products.Views;

public interface IProductResponse
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
}