﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Versioning;
using RiodeApp.DataAccess;
using RiodeApp.Models;
using RiodeApp.Services.Interfaces;
using RiodeApp.ViewModels.BasketVMs;
using RiodeApp.ViewModels.CategoryVMs;
using RiodeApp.ViewModels.HomeVMs;

namespace RiodeApp.Controllers;

public class HomeController : Controller
{
    readonly ISliderService _sliderService;
    readonly IProductService _productService;
    readonly ICategoryService _categoryService;
    readonly RiodeDbContext _context;

    public HomeController(ISliderService sliderService, IProductService productService,
        ICategoryService categoryService, RiodeDbContext context)
    {
        _sliderService = sliderService;
        _productService = productService;
        _categoryService = categoryService;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        HomeVM vm = new HomeVM
        {
            Sliders = await _sliderService.GetAll(false),
            Products = await _productService.GetAll(false),
            Categories = await _categoryService.GetTable.Take(4).ToListAsync(),
        };
        ViewBag.ProductCount = await _productService.GetTable.CountAsync();
        return View(vm);
    }
    public async Task<IActionResult> AddBasket(int? id)
    {
        if (id == null || id <= 0) return BadRequest();
        if (!await _productService.GetTable.AnyAsync(p => p.Id == id)) return BadRequest();
        var basket = HttpContext.Request.Cookies["basket"];
        List<BasketItemVM> items = basket == null ? new List<BasketItemVM>():
            JsonConvert.DeserializeObject<List<BasketItemVM>>(basket);
        var item = items.SingleOrDefault(i => i.Id == id);
        if (item == null)
        {
            item = new BasketItemVM
            {
                Id = (int)id,
                Count = 1
            };
            items.Add(item);
        }
        else
        {
            item.Count++;
        }
        HttpContext.Response.Cookies.Append("basket", JsonConvert.SerializeObject(items));
        return Ok();
    }
    public async Task<IActionResult> GetBasket()
    {

        var basket = JsonConvert.DeserializeObject<List<BasketItemVM>>(HttpContext.Request.Cookies["basket"]);
        List<BasketItemProductVM> vm = new List<BasketItemProductVM>();
        foreach (var item in basket)
        {
            vm.Add(new BasketItemProductVM
            {
                Count = item.Count,
                Product = await _productService.GetById(item.Id)
            });
        }
        ViewBag.ProductCount = await _productService.GetTable.CountAsync();
        return PartialView("_BasketPartial", vm);
    }
    //public async Task<IActionResult> ShowCategoryCount()
    //{
    //    List<ShowCount> categoriesWithProductCounts = await _context.Categories
    //        .Select(c => new ShowCount
    //        {
    //            Name = c.Name,
    //            Count = c.ProductCategories.Count
    //        })
    //        .ToListAsync();

    //    return PartialView("__HomeCategoryPartial", categoriesWithProductCounts);
    //}
}