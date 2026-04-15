using Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Models;

namespace Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Services;

public interface IOrderService
{
    Task<List<ActiveCustomerDto>> GetActiveCustomersAsync();
    Task<List<CustomerProductSummaryDto>> GetCustomerProductSummaryAsync();
    Task<List<DistinctProductsThisWeekDto>> GetDistinctProductsThisWeekAsync();
    Task<List<OrderListDto>> GetOrdersAsync();
    Task<List<OrderListDto>> GetMostRecentOrdersByCustomerAsync(int customerId);
    Task<List<OrderListDto>> GetMostRecentOrdersByEmployeeAsync(int employeeId);
    Task<List<OrderListDto>> GetMostRecentOrdersByModifiedOnAsync();
    Task<List<OrderListDto>> GetMostRecentOrdersByOrderDateAsync();
    Task<List<ProductOrderDto>> GetProductOrdersAsync();
    Task<List<Orders>> GetAllAsync();
    Task<Orders?> GetByIdAsync(int orderId);
    Task<Orders> CreateAsync(Orders order);
    Task<Orders> UpdateAsync(Orders order);
    Task<int> DeleteAsync(int orderId);
}
