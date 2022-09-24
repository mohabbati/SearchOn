﻿namespace SearchOn.Services.Implementations;

public class BuildingSearch : ISearch<Building, Building>
{
    public IEnumerable<Building> Index(ISearchable document)
    {
        var dataToIndex = (DataFile)document;

        var buildings = dataToIndex.Buildings.Select(b => new Building
        {
            Id = b.Id,
            Name = b.Name,
            ShortCut = b.ShortCut,
            Description = b.Description
        }).AsEnumerable();

        return buildings;
    }

    public IEnumerable<Building> Search(IEnumerable<Building> index, string criteria)
    {
        if (index is null)
            throw new LogicException("There is no index to search.");

        if (string.IsNullOrWhiteSpace(criteria))
            throw new LogicException("Please enter criteria.");

        criteria = criteria.ToLower().Trim();

        var searchModel = new Building();

        searchModel.ManipulateCriteria(criteria);

        var query = from q in index

                    let nameMatchBoost = q.Name is not null && q.Name.ToLower() == searchModel.Name
                    let nameMatch = q.Name is not null && q.Name.ToLower().Contains(searchModel.Name)

                    let shortCutMatchBoost = q.ShortCut is not null && q.ShortCut.ToLower() == searchModel.ShortCut
                    let shortCutMatch = q.ShortCut is not null && q.ShortCut.ToLower().Contains(searchModel.ShortCut)

                    let descriptionMatchBoost = q.Description is not null && q.Description.ToLower() == searchModel.Description
                    let descriptionMatch = q.Description is not null && q.Description.ToLower().Contains(searchModel.Description)

                    let score =
                            q.Score =
                                q.CalculatePropertyScore(nameof(q.Name), nameMatch, nameMatchBoost) +
                                q.CalculatePropertyScore(nameof(q.ShortCut), shortCutMatch, shortCutMatchBoost) +
                                q.CalculatePropertyScore(nameof(q.Description), descriptionMatch, descriptionMatchBoost)

                    where
                        nameMatch || shortCutMatch || descriptionMatch ||
                        nameMatchBoost || shortCutMatchBoost || descriptionMatchBoost

                    orderby q.Score descending

                    select q;

        return query;
    }
}
