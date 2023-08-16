using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RiodeApp.Services.Interfaces;
using RiodeApp.ViewModels.ShopVMs;

namespace RiodeApp.Controllers;

public class ShopController : Controller
{
    readonly IProductService _prodService;

	public ShopController(IProductService prodService)
	{
		_prodService = prodService;
	}

	public async Task<IActionResult> Index()
    {
		ShopVM vm = new ShopVM
		{
			Products = await _prodService.GetTable.Include(p => p.ProductCategories).
			ThenInclude(pc => pc.Category).Take(10).ToListAsync()
		};
        return View(vm);
    }
	public async Task<IActionResult> Detail(int id)
	{
		var entity = await _prodService.GetTable.Include(p => p.ProductImages).Include(p => p.ProductCategories).
			ThenInclude(pc => pc.Category).SingleOrDefaultAsync(i => i.Id == id);
		return View(entity);
	}
}
