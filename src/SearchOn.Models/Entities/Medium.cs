namespace SearchOn.Models.Entities;

public class Medium
{
    public Guid Id { get; set; }

    public Guid GroupId { get; set; }

    public string? Type { get; set; }

    public string? Owner { get; set; }

    public string? SerialNumber { get; set; }

    public string? Description { get; set; }
}
