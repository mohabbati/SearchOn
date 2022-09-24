using SearchOn.Models.Searches;

namespace SearchOn.Services.Implementations;

public class BuildingLockSearch : ISearch<BuildingLock, BuildingLock>
{
    public IEnumerable<BuildingLock> Index(ISearchable data)
    {
        var dataToIndex = (DataFile)data;

        var buildingLocks = dataToIndex.Buildings.Join(dataToIndex.Locks, b => b.Id, l => l.BuildingId, (b, l) => new BuildingLock
        {
            Id = l.Id,
            BuildingId = l.BuildingId,
            BuildingName = b.Name,
            BuildingShortCut = b.ShortCut,
            BuildingDescription = b.Description,
            Name = l.Name,
            Type = l.Type,
            SerialNumber = l.SerialNumber,
            RoomNumber = l.RoomNumber,
            Floor = l.Floor,
            Description = l.Description
        }).AsEnumerable();

        return buildingLocks;
    }

    public IEnumerable<BuildingLock> Search(IEnumerable<BuildingLock> index, string criteria)
    {
        if (index is null)
            throw new LogicException("There is no index to search.");

        if (string.IsNullOrWhiteSpace(criteria))
            throw new LogicException("Please enter criteria.");

        criteria = criteria.ToLower().Trim();

        var searchModel = new BuildingLock();

        searchModel.ManipulateCriteria(criteria);

        var query = from q in index

                    let nameMatchBoost = q.Name is not null && q.Name.ToLower() == searchModel.Name
                    let nameMatch = q.Name is not null && q.Name.ToLower().Contains(searchModel.Name)

                    let typeMatchBoost = q.Type is not null && q.Type.ToLower() == searchModel.Type
                    let typeMatch = q.Type is not null && q.Type.ToLower().Contains(searchModel.Type)

                    let serialNumberMatchBoost = q.SerialNumber is not null && q.SerialNumber.ToLower() == searchModel.SerialNumber
                    let serialNumberMatch = q.SerialNumber is not null && q.SerialNumber.ToLower().Contains(searchModel.SerialNumber)

                    let floorMatchBoost = q.Floor is not null && q.Floor.ToLower() == searchModel.Floor
                    let floorMatch = q.Floor is not null && q.Floor.ToLower().Contains(searchModel.Floor)

                    let roomNumberMatchBoost = q.RoomNumber is not null && q.RoomNumber.ToLower() == searchModel.RoomNumber
                    let roomNumberMatch = q.RoomNumber is not null && q.RoomNumber.ToLower().Contains(searchModel.RoomNumber)

                    let descriptionMatchBoost = q.Description is not null && q.Description.ToLower() == searchModel.Description
                    let descriptionMatch = q.Description is not null && q.Description.ToLower().Contains(searchModel.Description)

                    let buildingNameMatchBoost = q.BuildingName is not null && q.BuildingName.ToLower() == searchModel.BuildingName
                    let buildingNameMatch = q.BuildingName is not null && q.BuildingName.ToLower().Contains(searchModel.BuildingName)

                    let buildingShortCutMatchBoost = q.BuildingShortCut is not null && q.BuildingShortCut.ToLower() == searchModel.BuildingShortCut
                    let buildingShortCutMatch = q.BuildingShortCut is not null && q.BuildingShortCut.ToLower().Contains(searchModel.BuildingShortCut)

                    let buildingDescriptionMatchBoost = q.BuildingDescription is not null && q.BuildingDescription.ToLower() == searchModel.BuildingDescription
                    let buildingDescriptionMatch = q.BuildingDescription is not null && q.BuildingDescription.ToLower().Contains(searchModel.BuildingDescription)

                    let score =
                            q.Score =
                                q.CalculatePropertyScore(nameof(q.Name), nameMatch, nameMatchBoost) +
                                q.CalculatePropertyScore(nameof(q.Type), typeMatch, typeMatchBoost) +
                                q.CalculatePropertyScore(nameof(q.SerialNumber), serialNumberMatch, serialNumberMatchBoost) +
                                q.CalculatePropertyScore(nameof(q.Floor), floorMatch, floorMatchBoost) +
                                q.CalculatePropertyScore(nameof(q.RoomNumber), roomNumberMatch, roomNumberMatchBoost) +
                                q.CalculatePropertyScore(nameof(q.Description), descriptionMatch, descriptionMatchBoost) +
                                q.CalculatePropertyScore(nameof(q.BuildingName), buildingNameMatch, buildingNameMatchBoost) +
                                q.CalculatePropertyScore(nameof(q.BuildingShortCut), buildingShortCutMatch, buildingShortCutMatchBoost) +
                                q.CalculatePropertyScore(nameof(q.BuildingDescription), buildingDescriptionMatch, buildingDescriptionMatchBoost)

                    where
                        nameMatch || typeMatch || serialNumberMatch || floorMatch || roomNumberMatch || descriptionMatch || buildingNameMatch || buildingShortCutMatch || buildingDescriptionMatch ||
                        nameMatchBoost || typeMatchBoost || serialNumberMatchBoost || floorMatchBoost || roomNumberMatchBoost || descriptionMatchBoost || buildingNameMatchBoost || buildingShortCutMatchBoost || buildingDescriptionMatchBoost

                    orderby q.Score descending

                    select q;

        return query;
    }
}
