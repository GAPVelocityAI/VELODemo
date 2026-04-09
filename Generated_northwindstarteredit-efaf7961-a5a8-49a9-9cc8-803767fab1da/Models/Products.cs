using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Models;

public class Products
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ProductID { get; set; }

    [Required]
    [MaxLength(20)]
    public string ProductCode { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string ProductName { get; set; } = string.Empty;

    public string? ProductDescription { get; set; }

    [Column(TypeName = "decimal(18,4)")]
    public decimal UnitPrice { get; set; }

    [MaxLength(255)]
    public string? AddedBy { get; set; }

    public DateTime? AddedOn { get; set; }

    [MaxLength(255)]
    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    [InverseProperty(nameof(OrderDetails.Product))]
    public ICollection<OrderDetails> OrderDetailRecords { get; set; } = new List<OrderDetails>();
}
