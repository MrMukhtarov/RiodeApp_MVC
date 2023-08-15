using System.ComponentModel.DataAnnotations;

namespace RiodeApp.ViewModels.ProductVMs;

public record UpdateProductVM
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    [Required, Range(0, Double.MaxValue)]
    public decimal Price { get; set; }
    [Required, Range(0, 100)]
    public byte Discount { get; set; }
    [Required, Range(0, 5)]
    public byte Rating { get; set; }
    [Required]
    public IFormFile MainImageFile { get; set; }
    [Required, Range(0, Int32.MaxValue)]
    public int StockCount { get; set; }
    public ICollection<IFormFile>? ProductImageFiles { get; set; }
    public List<int> CategoryIds { get; set; }
}
