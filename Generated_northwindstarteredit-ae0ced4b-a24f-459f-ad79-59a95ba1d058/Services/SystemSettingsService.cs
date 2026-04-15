using Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Models;
using Microsoft.EntityFrameworkCore;

namespace Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Services;

public sealed class SystemSettingsService : ISystemSettingsService
{
    private readonly IDbContextFactory<NorthwindstartereditAe0ced4bA24f459fAd7959a95ba1d058DbContext> _dbFactory;
    private readonly ILogger<SystemSettingsService> _logger;

    public SystemSettingsService(
        IDbContextFactory<NorthwindstartereditAe0ced4bA24f459fAd7959a95ba1d058DbContext> dbFactory,
        ILogger<SystemSettingsService> logger)
    {
        _dbFactory = dbFactory;
        _logger = logger;
    }

    /// <summary>
    /// Translates Access query qrySystemSettings: returns all system settings ordered by setting name.
    /// </summary>
    public async Task<List<SystemSettings>> GetSystemSettingsAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.SystemSettings.AsNoTracking().OrderBy(s => s.SettingName).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing qrySystemSettings.");
            throw;
        }
    }

    public async Task<List<SystemSettings>> GetAllAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.SystemSettings.AsNoTracking().OrderBy(s => s.SettingName).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all system settings.");
            throw;
        }
    }

    public async Task<SystemSettings?> GetByIdAsync(int settingId)
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.SystemSettings.FirstOrDefaultAsync(s => s.SettingID == settingId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving system setting by id {SettingId}.", settingId);
            throw;
        }
    }

    public async Task<SystemSettings> CreateAsync(SystemSettings setting)
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            db.SystemSettings.Add(setting);
            await db.SaveChangesAsync();
            return setting;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating system setting.");
            throw;
        }
    }

    public async Task<SystemSettings> UpdateAsync(SystemSettings setting)
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            db.SystemSettings.Update(setting);
            await db.SaveChangesAsync();
            return setting;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating system setting {SettingId}.", setting.SettingID);
            throw;
        }
    }

    public async Task<int> DeleteAsync(int settingId)
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.SystemSettings.Where(s => s.SettingID == settingId).ExecuteDeleteAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting system setting {SettingId}.", settingId);
            throw;
        }
    }
}
