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

    public HomeController(ISliderService sliderService, IProductService productService)
    {
        _sliderService = sliderService;
        _productService = productService;
    }

    public async Task<IActionResult> Index()
    {
        HomeVM vm = new HomeVM
        {
            Sliders = await _sliderService.GetAll(false),
            Products = await _productService.GetAll(false),
        };
        return View(vm);
    }
}