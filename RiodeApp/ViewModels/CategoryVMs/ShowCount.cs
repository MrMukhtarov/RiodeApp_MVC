using RiodeApp.Models;

namespace RiodeApp.ViewModels.CategoryVMs;

public record ShowCount
{
    public int Count { get; set; }
    public List<Product> Image { get; set; }
    public string Name { get; set; }
}
