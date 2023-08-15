using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RiodeApp.DataAccess;
using RiodeApp.Services.Interfaces;
using RiodeApp.ViewModels.HomeVMs;

namespace RiodeApp.Controllers;

public class HomeController : Controller
{
    readonly ISliderService _sliderService;
    readonly IProductService _productService;
    readonly ICategoryService _categoryService;

    public HomeController(ISliderService sliderService, IProductService productService, 
        ICategoryService categoryService)
    {
        _sliderService = sliderService;
        _productService = productService;
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Index()
    {
        HomeVM vm = new HomeVM
        {
            Sliders = await _sliderService.GetAll(false),
            Products = await _productService.GetAll(false),
            Categories = await _categoryService.GetTable.Take(4).ToListAsync(),
        };
        return View(vm);
    }
}