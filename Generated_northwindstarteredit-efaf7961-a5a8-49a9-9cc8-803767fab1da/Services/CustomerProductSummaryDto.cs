namespace Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Services;

public sealed class CustomerProductSummaryDto
{
    public int CustomerID { get; set; }
    public int ProductID { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public int OrderCount { get; set; }
    public DateTime FirstOrdered { get; set; }
    public DateTime LastOrdered { get; set; }
}
