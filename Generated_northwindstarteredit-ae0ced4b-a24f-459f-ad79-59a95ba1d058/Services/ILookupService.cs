namespace Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Services;

public interface ILookupService
{
    Task<List<CustomerLookupDto>> GetCustomerLookupAsync();
    Task<List<OrderStatusLookupDto>> GetOrderStatusLookupAsync();
}
