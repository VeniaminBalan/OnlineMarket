namespace OnlineMarket.Utils.QuerryParams.Filters.Filter;

public interface IFilterParams
{
    public string? FilterBy { get; set; }
    public string? Value { get; set; }
}