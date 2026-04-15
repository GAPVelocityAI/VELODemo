using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Models;

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
