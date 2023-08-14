using Microsoft.EntityFrameworkCore;
using RiodeApp.DataAccess;
using RiodeApp.ExtensionServices.Interfaces;
using RiodeApp.Models;
using RiodeApp.Services.Interfaces;
using RiodeApp.ViewModels.SliderVMs;

namespace RiodeApp.Services.Implements;

public class SliderService : ISliderService
{
    readonly RiodeDbContext _context;
    readonly IFileService _service;

    public IQueryable<Slider> GetTable { get => _context.Set<Slider>(); }

    public SliderService(RiodeDbContext context, IFileService service)
    {
        _context = context;
        _service = service;
    }

    public async Task<ICollection<Slider>> GetAll(bool takeAll)
    {
        if (takeAll)
        {
            return await _context.Sliders.ToListAsync();
        }
        return await _context.Sliders.Where(s => s.IsDeleted == false).ToListAsync();
    }
    public async Task Create(CreateSliderVM vm)
    {
        await _context.Sliders.AddAsync(new Slider
        {
            ImageUrl = await _service.UploadAsync(vm.ImageFile, Path.Combine("Assets", "imgs", "slider"), 
            "image", 2),
            Title = vm.Title,
            ButtonText = vm.ButtonText,
            Description = vm.Description,
            iSright = vm.iSright,
        });
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int? id)
    {
        var entity = await GetById(id,true);
        _context.Sliders.Remove(entity);
        _service.Delete(entity.ImageUrl);
        await _context.SaveChangesAsync();
    }
    public async Task<Slider> GetById(int? id, bool takeAll = false)
    {
        if (id == null || id <= 0) throw new ArgumentException();
        Slider? entity;
        if (takeAll)
        {
            entity = await _context.Sliders.FindAsync(id);
        }
        else
        {
            entity = await _context.Sliders.SingleOrDefaultAsync(s => s.IsDeleted == false && s.Id == id);
        }
        if (entity == null) throw new NullReferenceException();
        return entity;
    }
    public async Task Update(UpdateSliderVM vm, int? id)
    {
        var entity = await GetById(id);
        entity.Title = vm.Title;
        entity.ButtonText = vm.ButtonText;
        entity.Description = vm.Description;
        entity.iSright = vm.iSright;
        if (vm.ImageFile != null)
        {
            _service.Delete(entity.ImageUrl);
            entity.ImageUrl = await _service.UploadAsync(vm.ImageFile, Path.Combine("Assets", "imgs", "slider"));
        }
        await _context.SaveChangesAsync();

    }
    public async Task SoftDelete(int? id)
    {
        var entity = await GetById(id,true);
        entity.IsDeleted = !entity.IsDeleted;
        await _context.SaveChangesAsync();
    }
}
