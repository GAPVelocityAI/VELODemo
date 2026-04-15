using System.Globalization;
using Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Models;
using Microsoft.EntityFrameworkCore;
using SystemSettingsEnum = Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Models.Enums.SystemSettings;

namespace Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Services;

public class ModStartupService : IModStartupService
{
    private readonly IDbContextFactory<NorthwindstartereditAe0ced4bA24f459fAd7959a95ba1d058DbContext> _dbFactory;
    private readonly IModGlobalService _modGlobalService;
    private readonly ILogger<ModStartupService> _logger;

    public ModStartupService(
        IDbContextFactory<NorthwindstartereditAe0ced4bA24f459fAd7959a95ba1d058DbContext> dbFactory,
        IModGlobalService modGlobalService,
        ILogger<ModStartupService> logger)
    {
        _dbFactory = dbFactory;
        _modGlobalService = modGlobalService;
        _logger = logger;
    }

    public async Task<object?> GetSystemSettingAsync(SystemSettingsEnum systemSettingId, CancellationToken cancellationToken = default)
    {
        await using var db = await _dbFactory.CreateDbContextAsync(cancellationToken);

        var rawValue = await db.SystemSettings
            .Where(s => s.SettingID == (int)systemSettingId)
            .Select(s => s.SettingValue)
            .FirstOrDefaultAsync(cancellationToken);

        return systemSettingId switch
        {
            SystemSettingsEnum.TaxRate => ParseFloat(rawValue) / 1000f,
            SystemSettingsEnum.LastResetDate => ParseDateTime(rawValue),
            SystemSettingsEnum.FirstTimeRun or SystemSettingsEnum.ShowWelcome => ParseBoolean(rawValue),
            _ => rawValue,
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

        var valueToPersist = ConvertSettingValue(systemSettingId, value);

        var rowsAffected = await db.SystemSettings
            .Where(s => s.SettingID == (int)systemSettingId)
            .ExecuteUpdateAsync(setters => setters.SetProperty(s => s.SettingValue, valueToPersist), cancellationToken);

        if (rowsAffected == 0)
        {
            _logger.LogWarning("SaveSystemSettingAsync found no SystemSettings row for SettingID {SettingID}", (int)systemSettingId);
        }
    }

    private string ConvertSettingValue(SystemSettingsEnum systemSettingId, object? value)
    {
        if (systemSettingId == SystemSettingsEnum.LastResetDate)
        {
            var parsedDate = value switch
            {
                DateTime dateTime => dateTime,
                DateTimeOffset dateTimeOffset => dateTimeOffset.DateTime,
                _ => ParseDateTime(value?.ToString()) ?? DateTime.UtcNow,
            };

            return _modGlobalService.ToAccessDate(parsedDate);
        }

        return value switch
        {
            null => string.Empty,
            bool booleanValue => booleanValue ? "-1" : "0",
            IFormattable formattable => formattable.ToString(null, CultureInfo.InvariantCulture),
            _ => value.ToString() ?? string.Empty,
        };
    }

    private static float ParseFloat(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return 0f;
        }

        if (float.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var invariantFloat))
        {
            return invariantFloat;
        }

        if (float.TryParse(value, NumberStyles.Float, CultureInfo.CurrentCulture, out var cultureFloat))
        {
            return cultureFloat;
        }

        return 0f;
    }

    private static DateTime? ParseDateTime(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var parsedDate))
        {
            return parsedDate;
        }

        if (DateTime.TryParse(value, CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal, out parsedDate))
        {
            return parsedDate;
        }

        return null;
    }

    private static bool ParseBoolean(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        if (bool.TryParse(value, out var parsedBoolean))
        {
            return parsedBoolean;
        }

        if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsedInteger))
        {
            return parsedInteger != 0;
        }

        return string.Equals(value, "yes", StringComparison.OrdinalIgnoreCase);
    }
}
