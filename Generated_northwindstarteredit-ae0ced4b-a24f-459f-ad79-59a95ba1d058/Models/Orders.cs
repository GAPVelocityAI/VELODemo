using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Models;

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

    [ForeignKey(nameof(Status))]
    public int StatusID { get; set; }

    [MaxLength(255)]
    public string? AddedBy { get; set; }

    public DateTime? AddedOn { get; set; }

    [MaxLength(255)]
    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public Customers Customer { get; set; } = null!;

    public Employees Employee { get; set; } = null!;

    public OrderStatus Status { get; set; } = null!;

    public ICollection<OrderDetails> OrderDetails { get; set; } = new List<OrderDetails>();
}
