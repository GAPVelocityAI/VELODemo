using Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Models;

namespace Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Services;

public interface ISystemSettingsService
{
    Task<List<SystemSettings>> GetSystemSettingsAsync();
    Task<List<SystemSettings>> GetAllAsync();
    Task<SystemSettings?> GetByIdAsync(int settingId);
    Task<SystemSettings> CreateAsync(SystemSettings setting);
    Task<SystemSettings> UpdateAsync(SystemSettings setting);
    Task<int> DeleteAsync(int settingId);
}
