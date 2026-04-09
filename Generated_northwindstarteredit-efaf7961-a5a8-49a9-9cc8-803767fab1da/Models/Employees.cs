using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Models;

public class Employees
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int EmployeeID { get; set; }

    [Required]
    [MaxLength(20)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(30)]
    public string LastName { get; set; } = string.Empty;

    [MaxLength(243)]
    public string? FullNameFNLN { get; set; }

    [MaxLength(243)]
    public string? FullNameLNFN { get; set; }

    [MaxLength(255)]
    public string? EmailAddress { get; set; }

    [Required]
    [MaxLength(50)]
    public string JobTitle { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? PrimaryPhone { get; set; }

    [MaxLength(20)]
    public string? SecondaryPhone { get; set; }

    [MaxLength(20)]
    public string? Title { get; set; }

    public string? Notes { get; set; }

    public byte[]? Attachments { get; set; }

    [MaxLength(50)]
    public string? WindowsUserName { get; set; }

    [MaxLength(255)]
    public string? AddedBy { get; set; }

    public DateTime? AddedOn { get; set; }

    [MaxLength(255)]
    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    [InverseProperty(nameof(Orders.Employee))]
    public ICollection<Orders> OrderRecords { get; set; } = new List<Orders>();
}
