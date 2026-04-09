using Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Models;
using Microsoft.EntityFrameworkCore;

namespace Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Services;

public sealed class SystemSettingsService : ISystemSettingsService
{
    private readonly IDbContextFactory<NorthwindstartereditEfaf7961A5A849A99Cc8803767Fab1DaDbContext> _dbFactory;
    private readonly ILogger<SystemSettingsService> _logger;

    public SystemSettingsService(
        IDbContextFactory<NorthwindstartereditEfaf7961A5A849A99Cc8803767Fab1DaDbContext> dbFactory,
        ILogger<SystemSettingsService> logger)
    {
        _dbFactory = dbFactory;
        _logger = logger;
    }

    /// <summary>
    /// Translates Access query qrySystemSettings (SystemSettings.* ordered by SettingName).
    /// </summary>
    public async Task<List<SystemSettings>> GetSystemSettingsAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.SystemSettings.OrderBy(s => s.SettingName).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error running qrySystemSettings.");
            throw;
        }
    }

    public async Task<List<SystemSettings>> GetAllAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.SystemSettings.OrderBy(s => s.SettingName).ToListAsync();
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
            _logger.LogError(ex, "Error retrieving system setting {SettingId}.", settingId);
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
