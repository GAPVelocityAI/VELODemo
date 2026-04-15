using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Models;

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

    public ICollection<Orders> Orders { get; set; } = new List<Orders>();
}
