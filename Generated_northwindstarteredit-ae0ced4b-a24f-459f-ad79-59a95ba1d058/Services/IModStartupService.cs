using SystemSettingsEnum = Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Models.Enums.SystemSettings;

namespace Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Services;

public interface IModStartupService
{
    Task<object?> GetSystemSettingAsync(SystemSettingsEnum systemSettingId, CancellationToken cancellationToken = default);

    Task<string> GetEmployeeFNLNAsync(int employeeId, CancellationToken cancellationToken = default);

    Task SaveSystemSettingAsync(SystemSettingsEnum systemSettingId, object? value, CancellationToken cancellationToken = default);
}
