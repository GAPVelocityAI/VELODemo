namespace Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Services;

public sealed class ProductSalesSummaryDto
{
    public int ProductID { get; set; }
    public string ProductCode { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public string? ProductDescription { get; set; }
    public decimal UnitPrice { get; set; }
    public int QtySold { get; set; }
    public int SoldThisWeek { get; set; }
}
