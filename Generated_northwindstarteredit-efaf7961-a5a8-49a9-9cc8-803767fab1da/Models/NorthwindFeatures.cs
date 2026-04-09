using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Models;

public class NorthwindFeatures
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int NorthwindFeaturesID { get; set; }

    [Required]
    [MaxLength(255)]
    public string ItemName { get; set; } = string.Empty;

    [MaxLength(255)]
    public string? Description { get; set; }

    [Required]
    [MaxLength(255)]
    public string Navigation { get; set; } = string.Empty;

    public string? LearnMore { get; set; }

    [MaxLength(255)]
    public string? HelpKeywords { get; set; }
}
