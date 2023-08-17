using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RiodeApp.Services.Implements;
using RiodeApp.Services.Interfaces;
using RiodeApp.ViewModels.ColorVMs;

namespace RiodeApp.Areas.Admin.Controllers;
[Area("Admin")]
public class ColorController : Controller
{
    readonly IColorService _colorServoce;

    public ColorController(IColorService colorServoce)
    {
        _colorServoce = colorServoce;
    }
    public async Task<IActionResult> Index()
    {
        return View(await _colorServoce.GetALl(true));
    }
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateColorVM vm)
    {
        if (!ModelState.IsValid) return View();
        if (await _colorServoce.GetTable.AnyAsync(c => c.ColorCode == vm.ColorCode))
        {
            ViewBag.SameName = "Already Color Name!! Please change!!";
            return View();
        }
        await _colorServoce.Create(vm);
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Update(int? id)
    {
        if (id == null || id <= 0) return BadRequest();
        var entity = await _colorServoce.GetById(id);
        if(entity == null) return BadRequest();
        UpdateColorGETVM vm = new UpdateColorGETVM();
        vm.ColorCode = entity.ColorCode;
        return View(vm);
    }
    [HttpPost]
    public async Task<IActionResult> Update(UpdateColorGETVM vm,int? id)
    {
        if (id == null || id <= 0) return BadRequest();
        var entity = await _colorServoce.GetById(id);
        if (entity == null) return BadRequest();
        UpdateColorVM upvm = new UpdateColorVM
        {
            ColorCode = vm.ColorCode,
        };
        
        if(await _colorServoce.GetTable.AnyAsync(c => c.ColorCode == vm.ColorCode))
        {
            ViewBag.SameName = "Already Color Name!! Please change!!";
            return View();
        }
        await _colorServoce.Update(id, upvm);
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || id <= 0) return BadRequest();
        try
        {
            await _colorServoce.Delete(id);
            return RedirectToAction(nameof(Index));
        }
        catch
        {
            TempData["HaveProduct"] = "You have a product in this color. First, delete the product, and then delete the color";
            return RedirectToAction(nameof(Index));
        }
    }
}
