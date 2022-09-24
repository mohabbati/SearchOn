namespace SearchOn.Models.Searches;

public class SearchResultElement
{
    public string? Type { get; set; }

    public Guid? Id { get; set; }

    public string? Value { get; set; }

    public decimal Score { get; set; }
}
