using System.ComponentModel.DataAnnotations;

namespace RiodeApp.ViewModels.CategoryVMs;

public record CreateCategoryVM
{
    [Required]
    public string Name { get; set; }
}
