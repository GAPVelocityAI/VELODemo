using Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Models;

namespace Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Services;

public interface ICustomerService
{
    Task<List<CustomerListDto>> GetCustomerListAsync();
    Task<List<Customers>> GetAllAsync();
    Task<Customers?> GetByIdAsync(int customerId);
    Task<Customers> CreateAsync(Customers customer);
    Task<Customers> UpdateAsync(Customers customer);
    Task<int> DeleteAsync(int customerId);
}
