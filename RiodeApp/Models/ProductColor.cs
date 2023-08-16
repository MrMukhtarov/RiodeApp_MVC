using System.ComponentModel.DataAnnotations;

namespace RiodeApp.Models;

public class ProductColor : BaseEntity
{
    [Required]
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    [Required]
    public int ColorId { get; set; }
    public Color? Cplor { get; set; }
}
