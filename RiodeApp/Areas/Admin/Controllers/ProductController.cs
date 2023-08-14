using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RiodeApp.Extensions;
using RiodeApp.Services.Interfaces;
using RiodeApp.ViewModels.ProductVMs;

namespace RiodeApp.Areas.Admin.Controllers;
[Area("Admin")]
public class ProductController : Controller
{
    readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }
    public async Task<IActionResult> Index()
    {
        return View(await _productService.GetAll(true));
    }
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(CreateProductVM vm)
    {
        if (vm.MainImageFile != null)
        {
            if (!vm.MainImageFile.IsTypeValid("image"))
            {
                ModelState.AddModelError("MainImageFile", "Image type is not valid");
            }
            if (!vm.MainImageFile.IsSizeValid(2))
            {
                ModelState.AddModelError("MainImageFile", "Image size does not valid");
            }
        }
        if (vm.ImageFiles != null)
        {
            foreach (var file in vm.ImageFiles)
            {
                if (!file.IsTypeValid("image"))
                {
                    ModelState.AddModelError("ImageFiles", "Image type is not valid");
                }
                if (!file.IsSizeValid(2))
                {
                    ModelState.AddModelError("ImageFiles", "Image size does not valid");
                }
            }
        }
        if (!ModelState.IsValid) return View();
        await _productService.Create(vm);
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Update(int? id)
    {
        if (id == null || id <= 0) return BadRequest();
        var entity = await _productService.GetTable.Include(p => p.ProductImages).
            SingleOrDefaultAsync(p => p.Id == id);
        if (entity == null) return BadRequest();
        UpdateProductGETVM vm = new UpdateProductGETVM()
        {
            Name = entity.Name,
            Description = entity.Description,
            Price = entity.Price,
            Discount = entity.Discount,
            Rating = entity.Rating,
            StockCount = entity.StockCount,
            MainImageUrl = entity.MainImage,
            ProductImages = entity.ProductImages
        };
       return View(vm);
    }
    [HttpPost]
    public async Task<IActionResult> Update(UpdateProductGETVM vm,int? id)
    {
        if (id == null || id <= 0) return BadRequest();
        var entity = await _productService.GetTable.Include(p => p.ProductImages).
            SingleOrDefaultAsync(p => p.Id == id);
        if (entity == null) return BadRequest();
        UpdateProductVM upmv = new UpdateProductVM()
        {
            Name = vm.Name,
            Description = vm.Description,
            Price = vm.Price,
            Discount = vm.Discount,
            Rating = vm.Rating,
            StockCount = vm.StockCount,
            MainImageFile = vm.MainImageFile,
            ProductImageFiles = vm.ImageFiles
        };
        await _productService.Update(upmv, id);
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> DeleteImage(int? id)
    {
        if (id == null || id <= 0) return BadRequest();
        await _productService.DeleteImage(id);
        return Ok();
    }
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || id <= 0) return BadRequest();
        var entity = await _productService.GetTable.Include(p => p.ProductImages).
            SingleOrDefaultAsync(p => p.Id == id);
        if(entity == null) return BadRequest();
        await _productService.Delete(entity.Id);
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> ChangeStatus(int? id)
    {
        await _productService.SoftDelete(id);
        TempData["IsDeleted"] = true;
        return RedirectToAction(nameof(Index));
    }
}
