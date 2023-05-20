namespace OnlineMarket.Utils.QuerryParams.Filters.SortingFilter;

public interface ISortingParams
{
    public string? Direction { get; set; }
    public string? Column { get; set; }
}