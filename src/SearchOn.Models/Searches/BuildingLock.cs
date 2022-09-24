namespace SearchOn.Models.Searches;

public class BuildingLock : ISearchModel, IResultModel
{
    public Guid Id { get; set; }

    public Guid BuildingId { get; set; }

    public string? BuildingShortCut { get; set; }

    public string? BuildingName { get; set; }

    public string? BuildingDescription { get; set; }

    public string? Type { get; set; }

    public string? Name { get; set; }

    public string? SerialNumber { get; set; }

    public string? Floor { get; set; }

    public string? RoomNumber { get; set; }

    public string? Description { get; set; }

    public decimal Score { get; set; } = 0;

    public int Weight(string propertyName)
    {
        return propertyName switch
        {
            nameof(Id) => 0,
            nameof(BuildingId) => 0,
            nameof(BuildingName) => 8,
            nameof(BuildingShortCut) => 5,
            nameof(BuildingDescription) => 0,
            nameof(Type) => 7,
            nameof(Name) => 10,
            nameof(SerialNumber) => 8,
            nameof(Floor) => 6,
            nameof(RoomNumber) => 6,
            nameof(Description) => 6,
            _ => 0
        };
    }
}