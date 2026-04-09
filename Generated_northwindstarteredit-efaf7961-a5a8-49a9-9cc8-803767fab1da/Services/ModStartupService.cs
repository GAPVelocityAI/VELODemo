using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Models;
using SystemSettingsEnum = Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Models.Enums.SystemSettings;

namespace Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Services;

public interface IModStartupService
{
    Task<object?> GetSystemSettingAsync(SystemSettingsEnum systemSettingId, CancellationToken cancellationToken = default);
    Task<string> GetEmployeeFNLNAsync(int employeeId, CancellationToken cancellationToken = default);
    Task SaveSystemSettingAsync(SystemSettingsEnum systemSettingId, object? value, CancellationToken cancellationToken = default);
}

public class ModStartupService : IModStartupService
{
    private readonly IDbContextFactory<NorthwindstartereditEfaf7961A5A849A99Cc8803767Fab1DaDbContext> _dbFactory;
    private readonly IModGlobalService _modGlobalService;

    public ModStartupService(
        IDbContextFactory<NorthwindstartereditEfaf7961A5A849A99Cc8803767Fab1DaDbContext> dbFactory,
        IModGlobalService modGlobalService)
    {
        _dbFactory = dbFactory;
        _modGlobalService = modGlobalService;
    }

    public async Task<object?> GetSystemSettingAsync(SystemSettingsEnum systemSettingId, CancellationToken cancellationToken = default)
    {
        await using var db = await _dbFactory.CreateDbContextAsync(cancellationToken);

        var value = await db.SystemSettings
            .Where(s => s.SettingID == (int)systemSettingId)
            .Select(s => s.SettingValue)
            .FirstOrDefaultAsync(cancellationToken);

        if (value is null)
        {
            return null;
        }

        return systemSettingId switch
        {
            SystemSettingsEnum.TaxRate => ParseTaxRate(value),
            SystemSettingsEnum.LastResetDate => ParseDate(value),
            SystemSettingsEnum.FirstTimeRun => ParseBoolean(value),
            SystemSettingsEnum.ShowWelcome => ParseBoolean(value),
            _ => value,
        };
    }

    public async Task<string> GetEmployeeFNLNAsync(int employeeId, CancellationToken cancellationToken = default)
    {
        await using var db = await _dbFactory.CreateDbContextAsync(cancellationToken);

        var employeeName = await db.Employees
            .Where(e => e.EmployeeID == employeeId)
            .Select(e => e.FullNameFNLN)
            .FirstOrDefaultAsync(cancellationToken);

        return string.IsNullOrWhiteSpace(employeeName)
            ? "Error Employee Not Found"
            : employeeName;
    }

    public async Task SaveSystemSettingAsync(SystemSettingsEnum systemSettingId, object? value, CancellationToken cancellationToken = default)
    {
        await using var db = await _dbFactory.CreateDbContextAsync(cancellationToken);

        if (systemSettingId == SystemSettingsEnum.LastResetDate && value is DateTime dateValue)
        {
            value = _modGlobalService.ToAccessDate(dateValue);
        }

        var row = await db.SystemSettings
            .FirstOrDefaultAsync(s => s.SettingID == (int)systemSettingId, cancellationToken);

        if (row is null)
        {
            throw new InvalidOperationException($"System setting {systemSettingId} was not found.");
        }

        row.SettingValue = ConvertToAccessString(value);
        await db.SaveChangesAsync(cancellationToken);
    }

    private static object ParseTaxRate(string value)
    {
        if (float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var invariantRate))
        {
            return invariantRate / 1000f;
        }

        if (float.TryParse(value, out var currentCultureRate))
        {
            return currentCultureRate / 1000f;
        }

        return 0f;
    }

    private static object? ParseDate(string value)
    {
        if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var parsedDate))
        {
            return parsedDate;
        }

        if (DateTime.TryParse(value, out parsedDate))
        {
            return parsedDate;
        }

        return null;
    }

    private static object ParseBoolean(string value)
    {
        if (bool.TryParse(value, out var boolValue))
        {
            return boolValue;
        }

        if (int.TryParse(value, out var intValue))
        {
            return intValue != 0;
        }

        return string.Equals(value, "-1", StringComparison.Ordinal);
    }

    private string? ConvertToAccessString(object? value)
    {
        return value switch
        {
            null => null,
            DateTime dateTime => _modGlobalService.ToAccessDate(dateTime),
            bool boolValue => boolValue ? "-1" : "0",
            IFormattable formattable => formattable.ToString(null, CultureInfo.InvariantCulture),
            _ => value.ToString(),
        };
    }
}
