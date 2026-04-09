using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Models;

public class Customers
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CustomerID { get; set; }

    [Required]
    [MaxLength(50)]
    public string CustomerName { get; set; } = string.Empty;

    [MaxLength(30)]
    public string? PrimaryContactLastName { get; set; }

    [MaxLength(20)]
    public string? PrimaryContactFirstName { get; set; }

    [MaxLength(50)]
    public string? PrimaryContactJobTitle { get; set; }

    [MaxLength(255)]
    public string? PrimaryContactEmailAddress { get; set; }

    [MaxLength(20)]
    public string? BusinessPhone { get; set; }

    [Required]
    [MaxLength(255)]
    public string Address { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    public string City { get; set; } = string.Empty;

    [Required]
    [MaxLength(2)]
    public string State { get; set; } = string.Empty;

    [Required]
    [MaxLength(10)]
    public string Zip { get; set; } = string.Empty;

    public string? Website { get; set; }

    public string? Notes { get; set; }

    [MaxLength(255)]
    public string? AddedBy { get; set; }

    public DateTime? AddedOn { get; set; }

    [MaxLength(255)]
    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    [InverseProperty(nameof(Orders.Customer))]
    public ICollection<Orders> OrderRecords { get; set; } = new List<Orders>();
}
