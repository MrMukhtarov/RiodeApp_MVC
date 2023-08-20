using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RiodeApp.DataAccess;
using RiodeApp.Extensions;
using RiodeApp.ViewModels.CategoryVMs;

namespace RiodeApp.ViewComponents;

public class CategoryHome : ViewComponent
{
    readonly RiodeDbContext _context;

    public CategoryHome(RiodeDbContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        //List<ShowCount> categoriesWithProductCounts = await _context.Categories.Take(4)
        //.Select(c => new ShowCount
        //{
        //    Name = c.Name,
        //    Count = c.ProductCategories.Count
        //})
        //.ToListAsync();

        //return View(categoriesWithProductCounts);
        var img = await _context.Products.ToListAsync();
        List<ShowCount> categoriesWithProductCounts = await _context.Categories
         .Take(4)
         .Select(c => new ShowCount
         {
             Name = c.Name,
             Count = c.ProductCategories.Count,
             Image = img
         })
         .ToListAsync();

        return View(categoriesWithProductCounts);
    }
}
