using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Models;

public class OrderDetails
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OrderDetailID { get; set; }

    [ForeignKey(nameof(Order))]
    public int OrderID { get; set; }

    [ForeignKey(nameof(Product))]
    public int ProductID { get; set; }

    public short Quantity { get; set; }

    [Column(TypeName = "decimal(18,4)")]
    public decimal UnitPrice { get; set; }

    [MaxLength(255)]
    public string? AddedBy { get; set; }

    public DateTime? AddedOn { get; set; }

    [MaxLength(255)]
    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    [InverseProperty(nameof(Orders.OrderDetailRecords))]
    public Orders? Order { get; set; }

    [InverseProperty(nameof(Products.OrderDetailRecords))]
    public Products? Product { get; set; }
}
