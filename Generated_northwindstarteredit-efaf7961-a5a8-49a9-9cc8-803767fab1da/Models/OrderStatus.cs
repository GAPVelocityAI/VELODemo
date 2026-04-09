using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Models;

public class OrderStatus
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int StatusID { get; set; }

    [Required]
    [MaxLength(5)]
    public string StatusCode { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string StatusName { get; set; } = string.Empty;

    public byte SortOrder { get; set; }

    [MaxLength(255)]
    public string? AddedBy { get; set; }

    public DateTime? AddedOn { get; set; }

    [MaxLength(255)]
    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    [InverseProperty(nameof(Orders.OrderStatus))]
    public ICollection<Orders> OrderRecords { get; set; } = new List<Orders>();
}
