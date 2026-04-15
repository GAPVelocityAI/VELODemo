using System.Globalization;
using System.Text;

namespace Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Services;

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

    public async Task ErrorHandlerExampleAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await Task.CompletedTask;
        }
        catch (Exception exception)
        {
            await HandleErrorAsync(nameof(ClsErrorHandlerService), nameof(ErrorHandlerExampleAsync), exception, cancellationToken: cancellationToken);
        }
    }

    public async Task HandleErrorAsync(
        string moduleName,
        string procedureName,
        Exception exception,
        int? lineNumber = null,
        CancellationToken cancellationToken = default)
    {
        if (exception.HResult == 2501)
        {
            return;
        }

        await LogErrorToFileAsync(exception, moduleName, procedureName, lineNumber, cancellationToken);

        var errorMessage = new StringBuilder()
            .AppendLine("The following error has occurred:")
            .AppendLine(exception.Message)
            .AppendLine($"On line {(lineNumber.HasValue ? lineNumber.Value.ToString(CultureInfo.InvariantCulture) : "<none>")}")
            .AppendLine($"Error number: {exception.HResult.ToString(CultureInfo.InvariantCulture)}")
            .ToString();

        _logger.LogError(exception, "{ErrorMessage}", errorMessage);
    }

    private async Task LogErrorToFileAsync(
        Exception exception,
        string moduleName,
        string procedureName,
        int? lineNumber,
        CancellationToken cancellationToken)
    {
        try
        {
            var entry = new StringBuilder()
                .AppendLine($"Current Time   : {DateTime.Now:O}")
                .AppendLine($"Module         : {moduleName}")
                .AppendLine($"Procedure      : {procedureName}")
                .AppendLine($"Error String   : {exception.Message}")
                .AppendLine($"Error Number   : {exception.HResult.ToString(CultureInfo.InvariantCulture)}")
                .AppendLine($"Error Line     : {(lineNumber.HasValue ? lineNumber.Value.ToString(CultureInfo.InvariantCulture) : "<none>")}")
                .AppendLine($"User           : {Environment.UserName}")
                .AppendLine($"Machine        : {Environment.MachineName}")
                .AppendLine("---------------")
                .ToString();

            await File.AppendAllTextAsync(_logFilePath, entry, cancellationToken);
        }
        catch (Exception loggingException)
        {
            _logger.LogWarning(loggingException, "Unable to log error details to file {LogFilePath}", _logFilePath);
        }
    }
}
