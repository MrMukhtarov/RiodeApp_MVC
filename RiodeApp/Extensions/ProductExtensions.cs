using RiodeApp.Models;

namespace RiodeApp.Extensions;

public static class ProductExtensions
{
    public static string GetRandomProductImageName(this Product product)
    {
        if (product == null || product.ProductImages == null)
            return "";

        var validImages = product.ProductImages
            .Where(pi => pi != null && !string.IsNullOrEmpty(pi.Name))
            .ToList();

        if (!validImages.Any())
            return "";

        return validImages.OrderBy(r => Guid.NewGuid()).FirstOrDefault()?.Name ?? "";
    }
}
