namespace Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Services;

public interface ILookupService
{
    Task<List<CboCustomerDto>> GetCboCustomersAsync();
    Task<List<CboOrderStatusDto>> GetCboOrderStatusAsync();
}
