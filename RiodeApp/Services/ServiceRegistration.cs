using RiodeApp.ExtensionServices.Implements;
using RiodeApp.ExtensionServices.Interfaces;
using RiodeApp.Services.Implements;
using RiodeApp.Services.Interfaces;

namespace RiodeApp.Services;

public static class ServiceRegistration
{
    public static void AddService(this IServiceCollection services)
    {
        services.AddScoped<ISliderService, SliderService>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IColorService, ColorService>();
    }
}
