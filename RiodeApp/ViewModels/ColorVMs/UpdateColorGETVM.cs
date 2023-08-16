using System.ComponentModel.DataAnnotations;

namespace RiodeApp.ViewModels.ColorVMs;

public record UpdateColorGETVM
{
    public int Id { get; set; }
    [Required]
    public string ColorCode { get; set; }
}
