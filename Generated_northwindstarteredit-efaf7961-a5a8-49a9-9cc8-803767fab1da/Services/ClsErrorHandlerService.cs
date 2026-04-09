using System.Text;
using Microsoft.Extensions.Logging;

namespace Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Services;

public interface IClsErrorHandlerService
{
    void ErrorHandlerExample();
    void HandleError(string moduleName, string procedureName, Exception? exception = null, int? errorLine = null);
}

public class ClsErrorHandlerService : IClsErrorHandlerService
{
    private const string LogFileName = "Northwind-Starter.log";

    private readonly ILogger<ClsErrorHandlerService> _logger;
    private readonly string _logFilePath;

    public ClsErrorHandlerService(ILogger<ClsErrorHandlerService> logger)
    {
        _logger = logger;
        _logFilePath = Path.Combine(AppContext.BaseDirectory, LogFileName);
    }

    public void ErrorHandlerExample()
    {
        try
        {
            throw new NotImplementedException("Code goes here.");
        }
        catch (Exception ex)
        {
            HandleError(nameof(ClsErrorHandlerService), nameof(ErrorHandlerExample), ex);
        }
    }

    public void HandleError(string moduleName, string procedureName, Exception? exception = null, int? errorLine = null)
    {
        if (exception is OperationCanceledException)
        {
            return;
        }

        var errorDescription = exception?.Message ?? "Unknown error.";
        var errorNumber = exception?.HResult ?? 0;

        LogErrorToFile(errorDescription, errorNumber, errorLine, moduleName, procedureName);

        var messageBuilder = new StringBuilder();
        messageBuilder.AppendLine("The following error has occurred:");
        messageBuilder.AppendLine(errorDescription);

        if (errorLine.HasValue)
        {
            messageBuilder.AppendLine($"On line {errorLine.Value}");
        }

        messageBuilder.AppendLine($"Error number: {errorNumber}");

        _logger.LogError(exception, "{ErrorMessage}", messageBuilder.ToString());
    }

    private void LogErrorToFile(string error, int errorNumber, int? errorLine, string moduleName, string procedureName)
    {
        try
        {
            var lines = new[]
            {
                $"Current Time   : {DateTime.Now}",
                $"Module         : {moduleName}",
                $"Procedure      : {procedureName}",
                $"Error String   : {error}",
                $"Error Number   : {errorNumber}",
                $"Error Line     : {(errorLine.HasValue ? errorLine.Value.ToString() : "<none>")}",
                $"User           : {Environment.UserName}",
                $"Machine        : {Environment.MachineName}",
                "---------------",
            };

            File.AppendAllLines(_logFilePath, lines);
        }
        catch (Exception ex)
        {
            _logger.LogDebug(ex, "Unable to write error details to the error log file.");
        }
    }
}
