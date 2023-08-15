using System.ComponentModel.DataAnnotations;

namespace RiodeApp.ViewModels.CategoryVMs;

public record UpdateCategoryGETVM
{
    [Required]
    public string Name { get; set; }
}
