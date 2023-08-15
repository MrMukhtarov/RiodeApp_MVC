using System.ComponentModel.DataAnnotations;

namespace RiodeApp.ViewModels.CategoryVMs;

public record UpdateCategoryVm
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
}
