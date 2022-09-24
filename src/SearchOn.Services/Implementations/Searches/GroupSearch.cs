namespace SearchOn.Services.Implementations;

public class GroupSearch : ISearch<Group, Group>
{
    public IEnumerable<Group> Index(ISearchable document)
    {
        var dataToIndex = (DataFile)document;

        var groups = dataToIndex.Groups.Select(g => new Group
        {
            Id = g.Id,
            Name = g.Name,
            Description = g.Description
        }).AsEnumerable();

        return groups;
    }

    public IEnumerable<Group> Search(IEnumerable<Group> index, string criteria)
    {
        if (index is null)
            throw new LogicException("There is no index to search.");

        if (string.IsNullOrWhiteSpace(criteria))
            throw new LogicException("Please enter criteria.");

        criteria = criteria.ToLower().Trim();

        var searchModel = new Group();

        searchModel.ManipulateCriteria(criteria);

        var query = from q in index

                    let nameMatchBoost = q.Name is not null && q.Name.ToLower() == searchModel.Name
                    let nameMatch = q.Name is not null && q.Name.ToLower().Contains(searchModel.Name)

                    let descriptionMatchBoost = q.Description is not null && q.Description.ToLower() == searchModel.Description
                    let descriptionMatch = q.Description is not null && q.Description.ToLower().Contains(searchModel.Description)

                    let score =
                            q.Score =
                                q.CalculatePropertyScore(nameof(q.Name), nameMatch, nameMatchBoost) +
                                q.CalculatePropertyScore(nameof(q.Description), descriptionMatch, descriptionMatchBoost)

                    where
                        nameMatch || descriptionMatch ||
                        nameMatchBoost || descriptionMatchBoost

                    orderby q.Score descending

                    select q;

        return query;
    }
}
