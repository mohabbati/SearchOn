namespace SearchOn.Models.Searches;

public class Building : ISearchModel, IResultModel
{
    public Guid Id { get; set; }

    public string? ShortCut { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }
    
    public decimal Score { get; set; } = 0;

    public int Weight(string propertyName)
    {
        return propertyName switch
        {
            nameof(Id) => 0,
            nameof(Name) => 9,
            nameof(ShortCut) => 7,
            nameof(Description) => 5,
            _ => 0
        };
    }
}
