using Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Models;

namespace Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Services;

public interface IEmployeeService
{
    Task<List<Employees>> GetEmployeeDetailsAsync();
    Task<List<Employees>> GetEmployeeListAsync();
    Task<List<Employees>> GetEmployeeLoginAsync();
    Task<List<Employees>> GetEmployeeLookupAsync();
    Task<List<Employees>> GetEmployeeEmailListAsync();
    Task<List<Employees>> GetEmployeePhoneListAsync();
    Task<List<Employees>> GetAllAsync();
    Task<Employees?> GetByIdAsync(int employeeId);
    Task<Employees> CreateAsync(Employees employee);
    Task<Employees> UpdateAsync(Employees employee);
    Task<int> DeleteAsync(int employeeId);
}
