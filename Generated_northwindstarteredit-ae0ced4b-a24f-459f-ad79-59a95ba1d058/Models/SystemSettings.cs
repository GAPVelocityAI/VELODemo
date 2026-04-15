using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Models;

public class SystemSettings
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SettingID { get; set; }

    [Required]
    [MaxLength(50)]
    public string SettingName { get; set; } = string.Empty;

    [MaxLength(255)]
    public string? SettingValue { get; set; }

    [MaxLength(255)]
    public string? Notes { get; set; }
}
