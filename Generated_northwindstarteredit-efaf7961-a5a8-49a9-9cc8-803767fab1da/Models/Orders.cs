using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Models;

public class Orders
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OrderID { get; set; }

    [ForeignKey(nameof(Employee))]
    public int EmployeeID { get; set; }

    [ForeignKey(nameof(Customer))]
    public int CustomerID { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime? ShippedDate { get; set; }

    public DateTime? PaidDate { get; set; }

    public string? Notes { get; set; }

    [ForeignKey(nameof(OrderStatus))]
    public int StatusID { get; set; }

    [MaxLength(255)]
    public string? AddedBy { get; set; }

    public DateTime? AddedOn { get; set; }

    [MaxLength(255)]
    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    [InverseProperty(nameof(Customers.OrderRecords))]
    public Customers? Customer { get; set; }

    [InverseProperty(nameof(Employees.OrderRecords))]
    public Employees? Employee { get; set; }

    [InverseProperty(nameof(OrderStatus.OrderRecords))]
    public OrderStatus? OrderStatus { get; set; }

    [InverseProperty(nameof(OrderDetails.Order))]
    public ICollection<OrderDetails> OrderDetailRecords { get; set; } = new List<OrderDetails>();
}
