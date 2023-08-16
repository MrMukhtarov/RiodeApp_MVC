using Microsoft.EntityFrameworkCore;
using RiodeApp.DataAccess;
using RiodeApp.Models;
using RiodeApp.Services.Interfaces;
using RiodeApp.ViewModels.ColorVMs;

namespace RiodeApp.Services.Implements;

public class ColorService : IColorService
{
    readonly RiodeDbContext _context;

    public ColorService(RiodeDbContext context)
    {
        _context = context;
    }

    public IQueryable<Color> GetTable { get => _context.Set<Color>(); }

    public async Task Create(CreateColorVM vm)
    {
        Color c = new Color();
        if (await _context.Colors.AnyAsync(c => c.ColorCode == vm.ColorCode))
        {
            throw new Exception();
        }
        c.ColorCode = vm.ColorCode;
        await _context.Colors.AddAsync(c);
        await _context.SaveChangesAsync();
    }

    public Task Delete(int? id)
    {
        throw new NotImplementedException();
    }

    public async Task<ICollection<Color>> GetALl(bool takeAll)
    {
        if (takeAll)
        {
            return await _context.Colors.ToListAsync();
        }
        else
        {
            return await _context.Colors.Where(c => c.IsDeleted == false).ToListAsync();
        }
    }

    public async Task<Color> GetById(int? id, bool takeAll = false)
    {
        if (id == null || id <= 0) throw new ArgumentNullException();
        Color? color;
        if (takeAll)
        {
            color = await _context.Colors.FindAsync(id);
        }
        else
        {
            color = await _context.Colors.SingleOrDefaultAsync(c => c.IsDeleted == false && c.Id == id);
        }
        if (color == null) throw new Exception();
        return color;
    }

    public async Task Update(int? id, UpdateColorVM vm)
    {
        var entity = await GetById(id);
        if(await _context.Colors.AnyAsync(c => c.ColorCode == vm.ColorCode))
        {
            throw new Exception();
        }
        entity.ColorCode = vm.ColorCode;
        await _context.SaveChangesAsync();
    }
}
