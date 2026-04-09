namespace Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Services;

public sealed class SalesByEmployeeReportDto
{
    public int EmployeeID { get; set; }
    public decimal OrderTotal { get; set; }
    public string? FullNameFNLN { get; set; }
    public string MonthYear { get; set; } = string.Empty;
    public string MonthYearSort { get; set; } = string.Empty;
}
