namespace Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Services;

public sealed class CustomerListDto
{
    public int CustomerID { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Zip { get; set; } = string.Empty;
    public string? BusinessPhone { get; set; }
    public string? PrimaryContactLastName { get; set; }
    public string? PrimaryContactFirstName { get; set; }
    public string? PrimaryContactJobTitle { get; set; }
    public string? PrimaryContactEmailAddress { get; set; }
    public string PrimaryContact { get; set; } = string.Empty;
    public string BusinessAddress { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public string? Website { get; set; }
    public string? AddedBy { get; set; }
    public DateTime? AddedOn { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public string PlainTextNotes { get; set; } = string.Empty;
}
