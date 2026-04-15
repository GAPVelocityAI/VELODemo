namespace Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Services;

public interface IReportService
{
    Task<List<InvoiceDto>> GetInvoiceAsync();
    Task<List<ProductSummaryDto>> GetProductsSummaryAsync();
    Task<List<SalesBySalesRepDto>> GetSalesBySalesRepAsync();
    Task<List<SalesYtdDto>> GetSalesYtdAsync();
    Task<List<SalesByEmployeeReportDto>> GetSalesByEmployeeReportAsync(DateTime startDate, DateTime endDate);
    Task<List<SalesByProductReportDto>> GetSalesByProductReportAsync(DateTime startDate, DateTime endDate);
}
