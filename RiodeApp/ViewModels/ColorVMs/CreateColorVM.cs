using System.ComponentModel.DataAnnotations;

namespace RiodeApp.ViewModels.ColorVMs;

public record CreateColorVM
{
    [Required]
    public string ColorCode { get; set; }
}
