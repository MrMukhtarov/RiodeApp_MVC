using RiodeApp.Models;

namespace RiodeApp.ViewModels.HomeVMs;

public class HomeVM
{
    public ICollection<Slider> Sliders { get; set; }
    public ICollection<Product> Products { get; set; }
}
