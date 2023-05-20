using Newtonsoft.Json;
using OnlineMarket.Features.Products.Models;

namespace OnlineMarket.UnitTest.MockData;

public class MockData
{
    private readonly string _filePath;
    
    public MockData(string filePath)
    {
        _filePath = filePath;
    }

    public List<ProductModel> GetProducts()
    {
        using StreamReader reader = new(_filePath);
        var json = reader.ReadToEnd();
        return JsonConvert.DeserializeObject<List<ProductModel>>(json);
    }
}