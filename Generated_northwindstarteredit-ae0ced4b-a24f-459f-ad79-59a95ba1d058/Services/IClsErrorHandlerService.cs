namespace Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Services;

public interface IClsErrorHandlerService
{
    Task HandleErrorAsync(
        string moduleName,
        string procedureName,
        Exception exception,
        int? lineNumber = null,
        CancellationToken cancellationToken = default);

    Task ErrorHandlerExampleAsync(CancellationToken cancellationToken = default);
}
