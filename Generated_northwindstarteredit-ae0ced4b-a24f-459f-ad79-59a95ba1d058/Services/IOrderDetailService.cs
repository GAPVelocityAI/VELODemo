using Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Models;

namespace Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Services;

public interface IOrderDetailService
{
    Task<List<OrderDetails>> GetOrderDetailsAsync();
    Task<List<OrderTotalDto>> GetOrderTotalsAsync();
    Task<List<OrderDetails>> GetAllAsync();
    Task<OrderDetails?> GetByIdAsync(int orderDetailId);
    Task<OrderDetails> CreateAsync(OrderDetails orderDetail);
    Task<OrderDetails> UpdateAsync(OrderDetails orderDetail);
    Task<int> DeleteAsync(int orderDetailId);
}
