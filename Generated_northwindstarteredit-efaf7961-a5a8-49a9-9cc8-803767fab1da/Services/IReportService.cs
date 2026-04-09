namespace Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Services;

public interface IReportService
{
    Task<List<CustomerProductSummaryDto>> GetCustomerProductSummaryAsync();
    Task<List<InvoiceDto>> GetInvoiceAsync();
    Task<List<SalesBySalesRepDto>> GetSalesBySalesRepAsync();
    Task<decimal> GetSalesYtdAsync();
    Task<List<EmployeeEmailListDto>> GetEmployeeEmailListAsync();
    Task<List<EmployeePhoneListDto>> GetEmployeePhoneListAsync();
    Task<List<SalesByEmployeeReportDto>> GetReportSalesByEmployeeAsync(DateTime startDate, DateTime endDate);
    Task<List<SalesByProductReportDto>> GetReportSalesByProductAsync(DateTime startDate, DateTime endDate);
    Task<List<SalesByEmployeeReportDto>> GetRptSalesByEmployeeAsync(DateTime startDate, DateTime endDate);
    Task<List<SalesByProductReportDto>> GetRptSalesByProductAsync(DateTime startDate, DateTime endDate);
}
