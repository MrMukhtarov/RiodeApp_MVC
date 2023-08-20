using RiodeApp.Models;

namespace RiodeApp.ViewModels.ShopVMs;

public class CategoryAndProductVM
{
    public List<Product> Products { get; set; }
    public List<ShopVM> ShopVMs { get; set; }
}
