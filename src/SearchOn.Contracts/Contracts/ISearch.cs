namespace SearchOn.Contracts;

public interface ISearch<TSearchModel, TResultModel>
    where TSearchModel : class, ISearchModel, new()
    where TResultModel : class, IResultModel, new()
{
    IEnumerable<TSearchModel> Index(ISearchable document);

    IEnumerable<TResultModel> Search(IEnumerable<TSearchModel> index, string criteria);
}
