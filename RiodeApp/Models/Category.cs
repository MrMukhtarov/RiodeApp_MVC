using System.ComponentModel.DataAnnotations;

namespace RiodeApp.Models;

public class Category : BaseEntity
{
    [Required]
    public string Name { get; set; }
    public ICollection<ProductCategory>? ProductCategories { get; set; }
}
