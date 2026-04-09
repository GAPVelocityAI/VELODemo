using Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Models;

namespace Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Services;

public interface IOrderDetailsService
{
    Task<List<OrderDetails>> GetOrderDetailsAsync();
    Task<List<OrderTotalDto>> GetOrderTotalAsync();
    Task<List<OrderDetails>> GetAllAsync();
    Task<OrderDetails?> GetByIdAsync(int orderDetailId);
    Task<OrderDetails> CreateAsync(OrderDetails orderDetail);
    Task<OrderDetails> UpdateAsync(OrderDetails orderDetail);
    Task<int> DeleteAsync(int orderDetailId);
}
