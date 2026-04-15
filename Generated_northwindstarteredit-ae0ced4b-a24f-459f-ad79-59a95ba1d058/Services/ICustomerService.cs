using Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Models;

namespace Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Services;

public interface ICustomerService
{
    Task<List<CustomerListDto>> GetCustomerListAsync();
    Task<List<Customers>> GetAllAsync();
    Task<Customers?> GetByIdAsync(int customerId);
    Task<Customers> CreateAsync(Customers customer);
    Task<Customers> UpdateAsync(Customers customer);
    Task<int> DeleteAsync(int customerId);
}
