namespace SearchOn.Services.Implementations;

public class GroupMediumSearch : ISearch<GroupMedium, GroupMedium>
{
    public IEnumerable<GroupMedium> Index(ISearchable document)
    {
        var dataToIndex = (DataFile)document;

        var groupMediums = dataToIndex.Groups.Join(dataToIndex.Mediums, g => g.Id, m => m.GroupId, (g, m) => new GroupMedium
        {
            Id = m.Id,
            GroupId = m.GroupId,
            GroupName = g.Name,
            GroupDescription = g.Description,
            Owner = m.Owner,
            Type = m.Type,
            SerialNumber = m.SerialNumber,
            Description = m.Description
        }).AsEnumerable();

        return groupMediums;
    }

    public IEnumerable<GroupMedium> Search(IEnumerable<GroupMedium> index, string criteria)
    {
        if (index is null)
            throw new LogicException("There is no index to search.");

        if (string.IsNullOrWhiteSpace(criteria))
            throw new LogicException("Please enter criteria.");

        criteria = criteria.ToLower().Trim();

        var searchModel = new GroupMedium();

        searchModel.ManipulateCriteria(criteria);

        var query = from q in index

                    let ownerMatchBoost = q.Owner is not null && q.Owner.ToLower() == searchModel.Owner
                    let ownerMatch = q.Owner is not null && q.Owner.ToLower().Contains(searchModel.Owner)

                    let typeMatchBoost = q.Type is not null && q.Type.ToLower() == searchModel.Type
                    let typeMatch = q.Type is not null && q.Type.ToLower().Contains(searchModel.Type)

                    let serialNumberMatchBoost = q.SerialNumber is not null && q.SerialNumber.ToLower() == searchModel.SerialNumber
                    let serialNumberMatch = q.SerialNumber is not null && q.SerialNumber.ToLower().Contains(searchModel.SerialNumber)

                    let descriptionMatchBoost = q.Description is not null && q.Description.ToLower() == searchModel.Description
                    let descriptionMatch = q.Description is not null && q.Description.ToLower().Contains(searchModel.Description)

                    let groupNameMatchBoost = q.GroupName is not null && q.GroupName.ToLower() == searchModel.GroupName
                    let groupNameMatch = q.GroupName is not null && q.GroupName.ToLower().Contains(searchModel.GroupName)

                    let groupDescriptionMatchBoost = q.GroupDescription is not null && q.GroupDescription.ToLower() == searchModel.GroupDescription
                    let groupDescriptionMatch = q.GroupDescription is not null && q.GroupDescription.ToLower().Contains(searchModel.GroupDescription)

                    let score =
                            q.Score =
                                q.CalculatePropertyScore(nameof(q.Owner), ownerMatch, ownerMatchBoost) +
                                q.CalculatePropertyScore(nameof(q.Type), typeMatch, typeMatchBoost) +
                                q.CalculatePropertyScore(nameof(q.SerialNumber), serialNumberMatch, serialNumberMatchBoost) +
                                q.CalculatePropertyScore(nameof(q.Description), descriptionMatch, descriptionMatchBoost) +
                                q.CalculatePropertyScore(nameof(q.GroupName), groupNameMatch, groupNameMatchBoost) +
                                q.CalculatePropertyScore(nameof(q.GroupDescription), groupDescriptionMatch, groupDescriptionMatchBoost)

                    where
                        ownerMatch || typeMatch || serialNumberMatch || descriptionMatch || groupNameMatch || groupDescriptionMatch ||
                        ownerMatchBoost || typeMatchBoost || serialNumberMatchBoost || descriptionMatchBoost || groupNameMatchBoost || groupDescriptionMatchBoost

                    orderby score descending

                    select q;

        return query;
    }
}
