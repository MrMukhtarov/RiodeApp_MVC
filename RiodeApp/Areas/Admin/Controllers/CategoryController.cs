using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RiodeApp.DataAccess;
using RiodeApp.Services.Interfaces;
using RiodeApp.ViewModels.CategoryVMs;
using System.Runtime.Intrinsics.X86;

namespace RiodeApp.Areas.Admin.Controllers;
[Area("Admin")]
public class CategoryController : Controller
{
    readonly ICategoryService _categoryService;
    readonly IProductService _productService;
    readonly RiodeDbContext _context;

    public CategoryController(ICategoryService categoryService, IProductService productService, RiodeDbContext context)
    {
        _categoryService = categoryService;
        _productService = productService;
        _context = context;
    }
    public async Task<IActionResult> Index()
    {
        return View(await _categoryService.GetAll(true));
    }
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryVM vm)
    {
        if (!ModelState.IsValid) return View();
        await _categoryService.Create(vm);
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Update(int? id)
    {
        if (id == null || id <= 0) return BadRequest();
        var entity = await _categoryService.GetById(id);
        if (entity == null) return BadRequest();
        UpdateCategoryGETVM vm = new UpdateCategoryGETVM();
        vm.Name = entity.Name;
        return View(vm);
    }
    [HttpPost]
    public async Task<IActionResult> Update(UpdateCategoryGETVM vm, int? id)
    {
        if (id == null || id <= 0) return BadRequest();
        var entity = await _categoryService.GetById(id);
        if (entity == null) return BadRequest();
        UpdateCategoryVm upmv = new UpdateCategoryVm();
        upmv.Name = vm.Name;
        if (await _categoryService.GetTable.AnyAsync(c => c.Name == vm.Name))
        {
            ViewBag.SameName = "Already Category Name!! Please change!!";
            return View();

        }
        await _categoryService.Update(id, upmv);
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Delete(int? id)
    {
        try
        {
            await _categoryService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception)
        {
            TempData["HaveProduct"] = "You have a product in this category. First, delete the product, and then delete the category";
            return RedirectToAction(nameof(Index));
        }
    }
    public async Task<IActionResult> ChangeStatus(int? id)
    {
        try
        {
            await _categoryService.SoftDelete(id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception)
        {
            TempData["HaveProduct"] = "You have a product in this category. First, delete the product, and then delete the category";
            return RedirectToAction(nameof(Index));
        }
    }
}
