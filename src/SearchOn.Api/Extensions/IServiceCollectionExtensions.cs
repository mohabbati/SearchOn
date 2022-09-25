using SearchOn.Models.Searches;
using SearchOn.Services.Implementations;

namespace Microsoft.Extensions.DependencyInjection;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddSearchService(this IServiceCollection services)
    {
        return services
            .AddSingleton(serviceProvider =>
            {
                var fileReader = new FileReader<DataFile>();
                var buildingLockSearch = new BuildingLockSearch();
                var buildingSearch = new BuildingSearch();
                var groupSearch = new GroupSearch();
                var groupMediumSearch = new GroupMediumSearch();
                var searchService = new SearchService(fileReader, buildingLockSearch, buildingSearch, groupMediumSearch, groupSearch);

                return searchService;
            });
    }
}
