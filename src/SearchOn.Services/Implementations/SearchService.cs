namespace SearchOn.Services.Implementations;

public class SearchService
{
    private readonly IFileReader<DataFile> _fileReader;
    private readonly ISearch<BuildingLock, BuildingLock> _buildingLockSearch;
    private readonly ISearch<Building, Building> _buildingSearch;
    private readonly ISearch<GroupMedium, GroupMedium> _groupMediumSearch;
    private readonly ISearch<Group, Group> _groupSearch;
    private static DataFile _dataFile = default!;

    public SearchService(IFileReader<DataFile> fileReader, ISearch<BuildingLock, BuildingLock> buildingLockSearch, ISearch<Building, Building> buildingSearch, ISearch<GroupMedium, GroupMedium> groupMediumSearch, ISearch<Group, Group> groupSearch)
    {
        _fileReader = fileReader;
        _buildingLockSearch = buildingLockSearch;
        _buildingSearch = buildingSearch;
        _groupMediumSearch = groupMediumSearch;
        _groupSearch = groupSearch;
    }

    public async Task LoadDefaultDataAsync(CancellationToken cancellationToken)
    {
        _dataFile = await _fileReader.ReadFileAsync(cancellationToken);
    }

    public SearchResult Search(string criteria)
    {
        if (_dataFile is null)
            throw new LogicException("There is no data to search. At the first load data.");

        if (string.IsNullOrWhiteSpace(criteria))
            throw new LogicException(nameof(criteria)); 
        
        var indexBuildingLock = _buildingLockSearch.Index(_dataFile);
        var indexBuilding = _buildingSearch.Index(_dataFile);
        var indexGroupMeduim = _groupMediumSearch.Index(_dataFile);
        var indexGroup = _groupSearch.Index(_dataFile);

        var queryBuildingLock = _buildingLockSearch.Search(indexBuildingLock, criteria);
        var queryBuilding = _buildingSearch.Search(indexBuilding, criteria);
        var queryGroupMeduim = _groupMediumSearch.Search(indexGroupMeduim, criteria);
        var queryGroup = _groupSearch.Search(indexGroup, criteria);

        var elements = new List<SearchResultElement>();

        elements.AddRange(ConvertToResultElement(typeof(BuildingLock).Name, queryBuildingLock));
        elements.AddRange(ConvertToResultElement(typeof(Building).Name, queryBuilding));
        elements.AddRange(ConvertToResultElement(typeof(GroupMedium).Name, queryGroupMeduim));
        elements.AddRange(ConvertToResultElement(typeof(Group).Name, queryGroup));

        var result = new SearchResult()
        {
            Count = elements.Count(),
            Elements = elements.OrderByDescending(e => e.Score)
        };

        return result;
    }

    private List<SearchResultElement> ConvertToResultElement(string typeName, IEnumerable<IResultModel> query)
    {
        var items = new List<SearchResultElement>();

        foreach (var item in query)
        {
            items.Add(new SearchResultElement()
            {
                Type = typeName,
                Id = item.Id,
                Value = ConcatenateProperties(item),
                Score = item.Score
            });
        }

        return items;
    }

    private string ConcatenateProperties(IResultModel resultModel)
    {
        var properties = resultModel.GetType().GetProperties();

        var result = string.Empty;
        
        foreach (var property in properties)
        {
            if (property.PropertyType == typeof(string))
            {
                var seprator = string.IsNullOrEmpty(result) ? string.Empty : Environment.NewLine;

                result = $"{result}{seprator}{property.Name} : {property.GetValue(resultModel)}";
            }
        }

        return result;
    }
}