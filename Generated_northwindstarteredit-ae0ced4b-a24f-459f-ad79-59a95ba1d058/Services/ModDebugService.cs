namespace Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Services;

public class ModDebugService : IModDebugService
{
    private readonly ILogger<ModDebugService> _logger;

    public ModDebugService(ILogger<ModDebugService> logger)
    {
        _logger = logger;
    }

    public void NotImplemented(string? message = null)
    {
        var resolvedMessage = "This feature is not yet implemented.";

        if (!string.IsNullOrWhiteSpace(message))
        {
            resolvedMessage = $"{resolvedMessage}{Environment.NewLine}{message}";
        }

        _logger.LogWarning("{Message}", resolvedMessage);
    }
}
