using Microsoft.EntityFrameworkCore;
using RiodeApp.DataAccess;
using RiodeApp.Models;
using RiodeApp.Services.Interfaces;
using RiodeApp.ViewModels.CategoryVMs;

namespace RiodeApp.Services.Implements;

public class CategoryService : ICategoryService
{
    readonly RiodeDbContext _context;

    public CategoryService(RiodeDbContext context)
    {
        _context = context;
    }
    public IQueryable<Category> GetTable { get => _context.Set<Category>(); }

    public async Task Create(CreateCategoryVM vm)
    {
        if (await _context.Categories.AnyAsync(c => c.Name == vm.Name))
        {
            throw new Exception();
        }
        Category pr = new Category() { Name =  vm.Name };
        await _context.AddAsync(pr);
        await _context.SaveChangesAsync();
    }

    public async Task<ICollection<Category>> GetAll(bool takeAll)
    {
        if (takeAll)
        {
            return await _context.Categories.ToListAsync();
        }
        return await _context.Categories.Where(c => c.IsDeleted == false).ToListAsync();
    }

    public async Task<Category> GetById(int? id, bool takeAll = false)
    {
        if (id == null || id <= 0) throw new ArgumentNullException();
        Category? category;
        if (takeAll)
        {
            category = await _context.Categories.FindAsync(id);
        }
        else
        {
            category = await _context.Categories.SingleOrDefaultAsync(c => c.IsDeleted == false && c.Id == id);
        }
        if (category == null) throw new NullReferenceException();
        return category;
    }
    public async Task Update(int? id, UpdateCategoryVm vm)
    {
        var entity = await GetById(id);
        if (await _context.Categories.AnyAsync(c => c.Name == vm.Name)) throw new Exception();
        entity.Name = vm.Name;
        await _context.SaveChangesAsync();
    }
    public async Task<bool> IsAllExist(List<int> ids)
    {
        foreach (var id in ids)
        {
            if(!await IsExist(id))
            {
                return false;
            }
        }
        return true;
    }

    public async Task<bool> IsExist(int id)
    {
        return await _context.Categories.AnyAsync(c => c.Id == id);
    }
}
