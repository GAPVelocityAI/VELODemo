using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Models;

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

    public ICollection<Orders> Orders { get; set; } = new List<Orders>();
}
