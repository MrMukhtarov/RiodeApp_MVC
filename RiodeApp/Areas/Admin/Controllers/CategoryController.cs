using Microsoft.AspNetCore.Mvc;
using RiodeApp.Services.Interfaces;
using RiodeApp.ViewModels.CategoryVMs;
using System.Runtime.Intrinsics.X86;

namespace RiodeApp.Areas.Admin.Controllers;
[Area("Admin")]
public class CategoryController : Controller
{
    readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
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
        if(id == null || id <= 0) return BadRequest();
        var entity = await _categoryService.GetById(id);
        if (entity == null) return BadRequest();
        UpdateCategoryGETVM vm = new UpdateCategoryGETVM();
        vm.Name = entity.Name;
        return View(vm);
    }
    [HttpPost]
    public async Task<IActionResult> Update(UpdateCategoryGETVM vm,int? id)
    {
        if (id == null || id <= 0) return BadRequest();
        var entity = await _categoryService.GetById(id);
        if (entity == null) return BadRequest();
        UpdateCategoryVm upmv = new UpdateCategoryVm();
        upmv.Name = vm.Name;
        await _categoryService.Update(id, upmv);
        return RedirectToAction(nameof(Index));
    }
}
