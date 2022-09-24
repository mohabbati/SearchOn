using Newtonsoft.Json;

namespace SearchOn.Models.Searches;

public class DataFile : ISearchable
{
    [JsonProperty("buildings")]
    public List<Entities.Building> Buildings { get; set; }

    [JsonProperty("locks")]
    public List<Entities.Lock> Locks { get; set; }

    [JsonProperty("groups")]
    public List<Entities.Group> Groups { get; set; }

    [JsonProperty("media")]
    public List<Entities.Medium> Mediums { get; set; }
}
