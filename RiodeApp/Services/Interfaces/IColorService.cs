using RiodeApp.Models;
using RiodeApp.ViewModels.ColorVMs;

namespace RiodeApp.Services.Interfaces;

public interface IColorService
{
    IQueryable<Color> GetTable { get; }
    Task<ICollection<Color>> GetALl(bool takeAll);
    Task<Color> GetById(int? id, bool takeAll = false);
    Task Create(CreateColorVM vm);
    Task Update(int? id, UpdateColorVM vm);
    Task Delete(int? id);
}
