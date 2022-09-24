using Newtonsoft.Json;

namespace SearchOn.Services.Implementations;

public class FileReader<TModel> : IFileReader<TModel>
    where TModel: class, ISearchable, new()
{
    public async Task<TModel> ReadFileAsync(CancellationToken cancellationToken)
    {
        return await ReadFileAsync(@"d:\temp\sv_lsm_data.json", cancellationToken);
    }

    public async Task<TModel> ReadFileAsync(string filePath, CancellationToken cancellationToken)
    {
        using StreamReader file = File.OpenText(filePath);

        var json = await file.ReadToEndAsync();
        var dataFile = JsonConvert.DeserializeObject<TModel>(json);

        if (dataFile is null)
            throw new Exception("There is no data.");

        return dataFile;
    }
}
