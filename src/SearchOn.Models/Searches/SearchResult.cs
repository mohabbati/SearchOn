namespace SearchOn.Models.Searches;

public class SearchResult
{
    public int Count { get; set; }

    public IEnumerable<SearchResultElement> Elements { get; set; } = default!;
}
