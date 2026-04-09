using Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Models;

namespace Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Services;

public interface IOrderService
{
    Task<List<ActiveCustomerDto>> GetActiveCustomersAsync();
    Task<List<DistinctProductsThisWeekDto>> GetDistinctProductsThisWeekAsync();
    Task<List<OrderWithStatusDto>> GetOrderAsync();
    Task<List<OrderSummaryDto>> GetOrdersAsync();
    Task<List<OrderSummaryDto>> GetOrdersMostRecentByCustomerAsync(int customerId);
    Task<List<OrderSummaryDto>> GetOrdersMostRecentByEmployeeAsync(int employeeId);
    Task<List<OrderSummaryDto>> GetOrdersMostRecentByModifiedOnAsync();
    Task<List<OrderSummaryDto>> GetOrdersMostRecentByOrderDateAsync();
    Task<List<ProductOrderDto>> GetProductOrdersAsync();
    Task<List<Orders>> GetAllAsync();
    Task<Orders?> GetByIdAsync(int orderId);
    Task<Orders> CreateAsync(Orders order);
    Task<Orders> UpdateAsync(Orders order);
    Task<int> DeleteAsync(int orderId);
}
