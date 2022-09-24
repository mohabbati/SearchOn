namespace SearchOn.Contracts;

public interface IFileReader<TModel> where TModel : class, new()
{
    Task<TModel> ReadFileAsync(string filePath, CancellationToken cancellationToken);

    Task<TModel> ReadFileAsync(CancellationToken cancellationToken);
}
