using RiodeApp.Models;
using RiodeApp.ViewModels.SliderVMs;

namespace RiodeApp.Services.Interfaces;

public interface ISliderService
{
    Task<ICollection<Slider>> GetAll(bool takeAll);
    Task Create(CreateSliderVM vm);
    Task Update(UpdateSliderVM vm, int? id);
    Task Delete(int? id);
    Task<Slider> GetById(int? id, bool takeAll = false);
    Task SoftDelete(int? id);
    IQueryable<Slider> GetTable { get; }
}