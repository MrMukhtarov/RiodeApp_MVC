using Microsoft.AspNetCore.Mvc;
using RiodeApp.Extensions;
using RiodeApp.Services.Interfaces;
using RiodeApp.ViewModels.SliderVMs;

namespace RiodeApp.Areas.Admin.Controllers;
[Area("Admin")]
public class SliderController : Controller
{
    readonly ISliderService _sliderService;

    public SliderController(ISliderService sliderService)
    {
        _sliderService = sliderService;
    }
    public async Task<IActionResult> Index()
    {
        return View(await _sliderService.GetAll(true));
    }
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateSliderVM vm)
    {
        if (vm.ImageFile != null)
        {
            if (!vm.ImageFile.IsTypeValid("image"))
            {
                ModelState.AddModelError("ImageFile", "Wrong FIle Type");
            }
            if (!vm.ImageFile.IsSizeValid(2))
            {
                ModelState.AddModelError("ImageFile", "File Size grater than 2 mb");
            }
            if (!ModelState.IsValid) return View();
            await _sliderService.Create(vm);
            return RedirectToAction(nameof(Index));
        }
        return View();
    }
    public async Task<IActionResult> Update(int? id)
    {
        if (id == null || id <= 0) return BadRequest();
        var entity = await _sliderService.GetById(id);
        UpdateSliderGETVM vm = new UpdateSliderGETVM 
        {
            Title = entity.Title,
            Description = entity.Description,
            ButtonText = entity.ButtonText,
            iSright = entity.iSright,
            Image = entity.ImageUrl
        };
        return View(vm);
    }
    [HttpPost]
    public async Task<IActionResult> Update(int? id, UpdateSliderGETVM vm)
    {
        if (id == null || id <= 0) return BadRequest();
        var entity = _sliderService.GetById(id);
        UpdateSliderVM upvm = new UpdateSliderVM
        {
            Title = vm.Title,
            Description = vm.Description,
            ButtonText = vm.ButtonText,
            iSright = vm.iSright,
            ImageFile = vm.ImageFile
        };
        await _sliderService.Update(upvm,id);
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Delete(int? id)
    {
        if(id == null || id <= 0) return BadRequest();
        await _sliderService.Delete(id);
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> ChangeStatus(int? id)
    {
        if (id == null || id <= 0) return BadRequest();
        await _sliderService.SoftDelete(id);
        TempData["IsDeleted"] = true;
        return RedirectToAction(nameof(Index));
    }
}