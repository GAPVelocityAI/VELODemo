using Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Models;

namespace Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Services;

public interface IEmployeeService
{
    Task<List<Employees>> GetEmployeeDetailsAsync();
    Task<List<EmployeeListDto>> GetEmployeeListAsync();
    Task<List<EmployeeLoginDto>> GetEmployeeLoginAsync();
    Task<List<CboEmployeeDto>> GetCboEmployeesAsync();
    Task<List<Employees>> GetAllAsync();
    Task<Employees?> GetByIdAsync(int employeeId);
    Task<Employees> CreateAsync(Employees employee);
    Task<Employees> UpdateAsync(Employees employee);
    Task<int> DeleteAsync(int employeeId);
}
