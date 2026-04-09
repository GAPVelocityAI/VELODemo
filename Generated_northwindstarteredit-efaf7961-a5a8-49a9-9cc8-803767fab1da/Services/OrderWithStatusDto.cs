namespace Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Services;

public sealed class OrderWithStatusDto
{
    public int OrderID { get; set; }
    public int EmployeeID { get; set; }
    public int CustomerID { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime? ShippedDate { get; set; }
    public DateTime? PaidDate { get; set; }
    public string? Notes { get; set; }
    public int StatusID { get; set; }
    public string? AddedBy { get; set; }
    public DateTime? AddedOn { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public string StatusName { get; set; } = string.Empty;
}
