using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Models;

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
