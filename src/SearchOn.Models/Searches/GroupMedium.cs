namespace SearchOn.Models.Searches;

public class GroupMedium : ISearchModel, IResultModel
{
    public Guid Id { get; set; }

    public Guid GroupId { get; set; }
    
    public string? GroupName { get; set; }
    
    public string? GroupDescription { get; set; }

    public string? Type { get; set; }
    
    public string? Owner { get; set; }
    
    public string? SerialNumber { get; set; }
    
    public string? Description { get; set; }

    public decimal Score { get; set; } = 0;

    public int Weight(string propertyName)
    {
        return propertyName switch
        {
            nameof(Id) => 0,
            nameof(GroupId) => 0,
            nameof(GroupName) => 8,
            nameof(GroupDescription) => 0,
            nameof(Type) => 3,
            nameof(Owner) => 10,
            nameof(SerialNumber) => 8,
            nameof(Description) => 6,
            _ => 0
        };
    }
}
