using System.Globalization;

namespace Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Services;

public interface IModFilesService
{
    Task StringToFileAsync(string path, object? value, CancellationToken cancellationToken = default);
}

public class ModFilesService : IModFilesService
{
    public async Task StringToFileAsync(string path, object? value, CancellationToken cancellationToken = default)
    {
        var content = Convert.ToString(value, CultureInfo.InvariantCulture) ?? string.Empty;
        await File.WriteAllTextAsync(path, content, cancellationToken);
    }
}
