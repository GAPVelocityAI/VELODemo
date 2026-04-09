using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Models;

public class Welcome
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }

    [Column("Welcome")]
    public string? WelcomeText { get; set; }

    public string? About { get; set; }

    public string? Learn { get; set; }

    public string? DataMacro { get; set; }
}

public class AdminInternetOrdersInput
{
    [Range(typeof(short), "1", "500")]
    public short OrderCount { get; set; } = 25;
}

public class WelcomeSettingsInput
{
    [Required]
    public bool ShowWelcome { get; set; } = true;
}
