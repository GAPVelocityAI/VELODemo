using Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Models;

namespace Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Services;

public interface ISystemSettingsService
{
    Task<List<SystemSettings>> GetSystemSettingsAsync();
    Task<List<SystemSettings>> GetAllAsync();
    Task<SystemSettings?> GetByIdAsync(int settingId);
    Task<SystemSettings> CreateAsync(SystemSettings setting);
    Task<SystemSettings> UpdateAsync(SystemSettings setting);
    Task<int> DeleteAsync(int settingId);
}
