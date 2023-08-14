using System.ComponentModel.DataAnnotations;

namespace RiodeApp.ViewModels.SliderVMs;

public record CreateSliderVM
{
    [Required]
    public IFormFile ImageFile { get; set; }
    [MaxLength(50)]
    public string Title { get; set; }
    [Required, MaxLength(200)]
    public string Description { get; set; }
    [Required, MaxLength(50)]
    public string ButtonText { get; set; }
    [Required]
    public bool iSright { get; set; }
}
