using RiodeApp.Models;

namespace RiodeApp.ViewModels.BasketVMs;

public record BasketItemProductVM
{
    public Product Product { get; set; }
    public int Count { get; set; }
}
