using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Models;

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
