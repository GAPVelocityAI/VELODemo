namespace Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Services;

public interface IModDebugService
{
    void NotImplemented(string? message = null);
}

public class ModDebugService : IModDebugService
{
    public void NotImplemented(string? message = null)
    {
        var fullMessage = $"This feature is not yet implemented.{Environment.NewLine}{message}".TrimEnd();
        throw new NotSupportedException(fullMessage);
    }
}
