namespace SearchOn.Models;

public static class ISearchModelExtensions
{
    public static void ManipulateCriteria<T>(this T searchModel, string criteria)
    {
        var properties = searchModel.GetType().GetProperties();

        foreach (var property in properties)
        {
            if (property.PropertyType == typeof(string))
            {
                property.SetValue(searchModel, Convert.ChangeType(criteria, property.PropertyType), null);
            }
        }
    }

    public static int CalculatePropertyScore<T>(this T searchModel, string propertyName, bool isPartialMatch, bool isFullMatch)
        where T : class, ISearchModel, new()
    {
        var weight = searchModel.Weight(propertyName);
        var score = (isPartialMatch ? weight * (isFullMatch ? 10 : 1) : 0);

        return score;
    }
}
